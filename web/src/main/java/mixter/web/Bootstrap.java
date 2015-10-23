package mixter.web;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.fasterxml.jackson.datatype.jsr310.JSR310Module;
import mixter.domain.core.message.TimelineMessageRepository;
import mixter.domain.core.message.events.MessageQuacked;
import mixter.domain.core.message.events.MessageRequacked;
import mixter.domain.core.message.handlers.UpdateTimeline;
import mixter.domain.core.subscription.FollowerRepository;
import mixter.domain.core.subscription.SubscriptionRepository;
import mixter.domain.core.subscription.events.UserFollowed;
import mixter.domain.core.subscription.events.UserUnfollowed;
import mixter.domain.core.subscription.handlers.NotifyFollowerOfFolloweeMessage;
import mixter.domain.core.subscription.handlers.UpdateFollowers;
import mixter.domain.identity.SessionProjectionRepository;
import mixter.domain.identity.events.UserConnected;
import mixter.domain.identity.events.UserDisconnected;
import mixter.domain.identity.handlers.RegisterSession;
import mixter.infra.EventStore;
import mixter.infra.InMemoryEventStore;
import mixter.infra.PersistingEventPublisher;
import mixter.infra.SynchronousEventPublisher;
import mixter.infra.repositories.*;
import net.codestory.http.WebServer;
import net.codestory.http.extensions.Extensions;
import net.codestory.http.misc.Env;

public class Bootstrap {
    public static void main(String[] args) {
        EventStore eventStore = new InMemoryEventStore();
        SynchronousEventPublisher dispatcher = new SynchronousEventPublisher();
        PersistingEventPublisher eventPublisher = new PersistingEventPublisher(eventStore, dispatcher);

        // Identity
        SessionProjectionRepository sessionProjectionRepository = new InMemorySessionProjectionRepository();
        EventSessionRepository sessionRepository = new EventSessionRepository(eventStore);
        EventUserIdentityRepository userIdentityRepository = new EventUserIdentityRepository(eventStore);

        RegisterSession registerSessionHandler = new RegisterSession(sessionProjectionRepository);
        dispatcher.register(UserConnected.class, registerSessionHandler::apply);
        dispatcher.register(UserDisconnected.class, registerSessionHandler::apply);

        IdentityResource identityResource = new IdentityResource(eventPublisher);
        SessionResource sessionResource = new SessionResource(userIdentityRepository, sessionRepository, eventPublisher);

        //Message
        TimelineMessageRepository timelineMessageRepository = new InMemoryTimelineMessageRepository();
        MessageResource messageResource = new MessageResource(sessionProjectionRepository, timelineMessageRepository, eventPublisher);
        UpdateTimeline updateTimeline = new UpdateTimeline(timelineMessageRepository);

        dispatcher.register(MessageQuacked.class, updateTimeline::apply);

        //Subscription
        FollowerRepository followerRepository= new InMemoryFollowerRepository();
        SubscriptionRepository subscriptionRepository = new InMemorySubscriptionRepository(eventStore,followerRepository);


        UpdateFollowers updateFollowers = new UpdateFollowers(followerRepository);
        NotifyFollowerOfFolloweeMessage notifyFollowerOfFolloweeMessageHandler = new NotifyFollowerOfFolloweeMessage(followerRepository, subscriptionRepository,eventPublisher);

        dispatcher.register(UserFollowed.class, updateFollowers::apply);
        dispatcher.register(UserUnfollowed.class, updateFollowers::apply);
        dispatcher.register(MessageQuacked.class,notifyFollowerOfFolloweeMessageHandler::apply);
        dispatcher.register(MessageRequacked.class,notifyFollowerOfFolloweeMessageHandler::apply);


        new WebServer().configure(routes -> routes
                        .setExtensions(new JsonExtension())
                        .get("/", "yeah")
                        .add(identityResource)
                        .add(sessionResource)
                        .add(messageResource)
        ).start();
    }
}

class JsonExtension implements Extensions {
    @Override
    public ObjectMapper configureOrReplaceObjectMapper(ObjectMapper defaultObjectMapper, Env env) {
        defaultObjectMapper
                .registerModule(new JSR310Module())
                .configure(SerializationFeature.WRITE_EMPTY_JSON_ARRAYS, true)
                .configure(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS, false);
        return defaultObjectMapper;
    }
}
