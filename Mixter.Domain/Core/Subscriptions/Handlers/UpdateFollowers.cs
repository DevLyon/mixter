using Mixter.Domain.Core.Subscriptions.Events;

namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    public class UpdateFollowers : 
        IEventHandler<UserFollowed>,
        IEventHandler<UserUnfollowed>
    {
        private readonly IFollowersRepository _repository;

        public UpdateFollowers(IFollowersRepository repository)
        {
            _repository = repository;
        }

        public void Handle(UserFollowed evt)
        {
            var subscriptionId = evt.SubscriptionId;
            _repository.Save(new FollowerProjection(subscriptionId.Followee, subscriptionId.Follower));
        }

        public void Handle(UserUnfollowed evt)
        {
            var subscriptionId = evt.SubscriptionId;
            _repository.Remove(new FollowerProjection(subscriptionId.Followee, subscriptionId.Follower));
        }
    }
}
