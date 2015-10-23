package mixter.web;

import mixter.domain.EventPublisher;
import mixter.domain.core.message.*;
import mixter.domain.identity.SessionId;
import mixter.domain.identity.SessionProjection;
import mixter.domain.identity.SessionProjectionRepository;
import mixter.domain.identity.UserId;
import net.codestory.http.Request;
import net.codestory.http.annotations.Get;
import net.codestory.http.annotations.Post;
import net.codestory.http.annotations.Prefix;
import net.codestory.http.constants.Headers;
import net.codestory.http.payload.Payload;

import java.util.*;

import static net.codestory.http.constants.HttpStatus.UNAUTHORIZED;

@Prefix("/api/:userId/messages")
public class MessageResource {
    private SessionProjectionRepository sessionRepository;
    private TimelineMessageRepository timelineMessageRepository;
    private EventPublisher eventPublisher;

    public MessageResource(SessionProjectionRepository sessionRepository, TimelineMessageRepository timelineMessageRepository, EventPublisher eventPublisher) {
        this.sessionRepository = sessionRepository;
        this.timelineMessageRepository = timelineMessageRepository;
        this.eventPublisher = eventPublisher;
    }

    @Post
    public Payload publish(String ownerIdStr, String content, Request request) {
        String sessionId = request.header("X-App-Session");
        UserId ownerId = new UserId(ownerIdStr);
        Optional<SessionProjection> maybeSession = sessionRepository.getById(new SessionId(sessionId));
        return maybeSession.filter(s -> s.getUserId().equals(ownerId)).map(session -> {
            UserId userId = session.getUserId();
            MessageId messageId = Message.quack(userId,content, eventPublisher);
            return Payload.created("/api/" + userId.toString() + "/messages/" + messageId.toString());
        }).orElse(
                new Payload(UNAUTHORIZED).withHeader(Headers.LOCATION, "/api/sessions")
        );
    }

    @Get
    public List<TimelineMessageProjection> timeline(String userId, Request request){
        String sessionId = request.header("X-App-Session");
        UserId ownerId = new UserId(userId);
        List<TimelineMessageProjection> list = new ArrayList<>();
        Iterator<TimelineMessageProjection> messageOfUser = timelineMessageRepository.getMessageOfUser(ownerId);
        messageOfUser.forEachRemaining(list::add);
        return list;
    }
}
