using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using Mixter.Domain.Messages;
using NFluent;

namespace Mixter.Tests.Infrastructure
{
    [TestClass]
    public class TimelineMessagesRepositoryTest
    {
        [TestMethod]
        public void GivenAMessageSavedWhenGetMessagesOfUserThenMessageIsReturned()
        {
            var repository = new TimelineMessagesRepository();
            var ownerId = new UserId("joe@mixit.fr");

            repository.Save(new TimelineMessage(ownerId, new UserId("joe@mixit.fr"), "MessageA", MessageId.Generate()));

            Check.That(repository.GetMessagesOfUser(ownerId)).HasSize(1);
        }

        [TestMethod]
        public void WhenSaveTwoMessagesOfDifferentOwnersThenGetMessagesOfUserThisMessage()
        {
            var repository = new TimelineMessagesRepository();
            var ownerId = new UserId("joe@mixit.fr");

            repository.Save(new TimelineMessage(ownerId, new UserId("joe@mixit.fr"), "MessageA", MessageId.Generate()));
            repository.Save(new TimelineMessage(new UserId("florent@mixit.fr"), new UserId("joe@mixit.fr"), "MessageB", MessageId.Generate()));

            var messagesOfUser = repository.GetMessagesOfUser(ownerId).ToList();
            Check.That(messagesOfUser).HasSize(1);
            Check.That(messagesOfUser.First().Content).IsEqualTo("MessageA");
        }
    }

    public class TimelineMessagesRepository : ITimelineMessagesRepository
    {
        private readonly IList<TimelineMessage> _messages = new List<TimelineMessage>();

        public void Save(TimelineMessage message)
        {
            _messages.Add(message);
        }

        public IEnumerable<TimelineMessage> GetMessagesOfUser(UserId userId)
        {
            return _messages.Where(o => o.OwnerId.Equals(userId));
        }
    }
}
