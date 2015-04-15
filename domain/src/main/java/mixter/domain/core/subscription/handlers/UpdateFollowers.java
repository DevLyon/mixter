package mixter.domain.core.subscription.handlers;

import mixter.doc.Handler;
import mixter.domain.core.subscription.FollowerRepository;
import mixter.domain.core.subscription.events.UserFollowed;

@Handler
public class UpdateFollowers {
    private FollowerRepository repository;

    public UpdateFollowers(FollowerRepository repository) {
        this.repository = repository;
    }

    public void apply(UserFollowed event) {
        repository.saveFollower(event.getSubscriptionId().getFollowee(), event.getSubscriptionId().getFollower());
    }
}
