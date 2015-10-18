using Mixter.Domain.Core.Subscriptions.Events;

namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    public class UpdateFollowers
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
    }
}
