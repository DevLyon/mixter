using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Identity;
using NFluent;

namespace Mixter.Infrastructure.Tests
{
    [TestClass]
    public class TimelineMessagesRepositoryTest
    {
        private ITimelineMessagesRepository _repository;
        private readonly UserId _ownerId = new UserId("joe@mixit.fr");
        private readonly UserId  _authorId = new UserId("joe@mixit.fr");

        [TestInitialize]
        public void Initialize()
        {
            _repository = new TimelineMessagesRepository();
        }

        [TestMethod]
        public void GivenAMessageSavedWhenGetMessagesOfUserThenMessageIsReturned()
        {
            _repository.Save(new TimelineMessageProjection(_ownerId, _authorId, "MessageA", MessageId.Generate()));

            Check.That(_repository.GetMessagesOfUser(_ownerId)).HasSize(1);
        }

        [TestMethod]
        public void WhenSaveTwoMessagesOfDifferentOwnersThenGetMessagesOfUserThisMessage()
        {
            const string messageA = "MessageA";
            const string messageB = "MessageB";

            _repository.Save(new TimelineMessageProjection(_ownerId, _authorId, messageA, MessageId.Generate()));
            _repository.Save(new TimelineMessageProjection(new UserId("florent@mixit.fr"), _authorId, messageB, MessageId.Generate()));

            var messagesOfUser = _repository.GetMessagesOfUser(_ownerId).ToList();
            Check.That(messagesOfUser).HasSize(1);
            Check.That(messagesOfUser.First().Content).IsEqualTo(messageA);
        }

        [TestMethod]
        public void WhenSaveTwoSameMessagesThenOnlyOneIsSaved()
        {
            const string messageA = "MessageA";

            var timelineMessage = new TimelineMessageProjection(_ownerId, _authorId, messageA, MessageId.Generate());
            _repository.Save(timelineMessage);
            _repository.Save(timelineMessage);

            var messagesOfUser = _repository.GetMessagesOfUser(_ownerId).ToList();
            Check.That(messagesOfUser).HasSize(1);
        }
    }
}