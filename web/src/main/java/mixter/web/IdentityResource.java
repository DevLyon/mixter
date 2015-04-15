package mixter.web;

import mixter.domain.EventPublisher;
import mixter.domain.identity.UserId;
import mixter.domain.identity.UserIdentity;
import net.codestory.http.annotations.Post;
import net.codestory.http.annotations.Prefix;
import net.codestory.http.payload.Payload;

import javax.inject.Inject;

@Prefix("/api/identity")
public class IdentityResource {

    private EventPublisher eventPublisher;

    @Inject
    public IdentityResource(EventPublisher eventPublisher) {
        this.eventPublisher = eventPublisher;
    }

    @Post
    public Payload register(Registration registration) {
        UserId userId = new UserId(registration.getEmail());
        UserIdentity.register(eventPublisher, userId);
        return Payload.created("/api/sessions");
    }

    public static class Registration {
        public String getEmail() {
            return email;
        }

        public void setEmail(String email) {
            this.email = email;
        }

        private String email;
    }

}

