using System.Collections.Generic;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    public class Subscription
    {
        public static void FollowUser(IEventPublisher eventPublisher, UserId follower, UserId followee)
        {
        }
    }
}