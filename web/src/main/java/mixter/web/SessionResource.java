package mixter.web;

import mixter.domain.EventPublisher;
import mixter.domain.identity.Session;
import mixter.domain.identity.SessionId;
import mixter.domain.identity.UserId;
import mixter.domain.identity.UserIdentity;
import mixter.infra.repositories.EventSessionRepository;
import mixter.infra.repositories.EventUserIdentityRepository;
import net.codestory.http.annotations.Delete;
import net.codestory.http.annotations.Post;
import net.codestory.http.annotations.Prefix;
import net.codestory.http.payload.Payload;

@Prefix("/api/sessions")
public class SessionResource {
    private EventUserIdentityRepository userIdentityRepository;
    private EventSessionRepository sessionRepository;
    private EventPublisher eventPublisher;

    public SessionResource(EventUserIdentityRepository userIdentityRepository, EventSessionRepository sessionRepository, EventPublisher eventPublisher) {
        this.userIdentityRepository = userIdentityRepository;
        this.sessionRepository = sessionRepository;
        this.eventPublisher = eventPublisher;
    }

    @Post
    public Payload login(SessionCreate session) {
        UserId userId = new UserId(session.getEmail());
        UserIdentity userIdentity = userIdentityRepository.getById(userId);
        SessionId sessionId = userIdentity.logIn(eventPublisher);
        return Payload.created("/api/sessions/" + sessionId).withHeader("X-App-Session", sessionId.toString());
    }

    @Delete("/:id")
    public Payload logout(String sessionIdStr) {
        SessionId sessionId = new SessionId(sessionIdStr);
        Session session = sessionRepository.getById(sessionId);
        session.logout(eventPublisher);
        return Payload.created("/api/sessions").withCode(204);
    }

    public static class SessionCreate {
        public String getEmail() {
            return email;
        }

        public void setEmail(String email) {
            this.email = email;
        }

        private String email;
    }
}
