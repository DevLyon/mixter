using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Identity;
using NFluent;
using Xunit;

namespace Mixter.Infrastructure.Tests
{
    public class TimelineMessagesRepositoryTest
    {
        private readonly TimelineMessageRepository _repository;
        private readonly UserId _ownerId = new UserId("joe@mixit.fr");
        private readonly UserId _authorId = new UserId("joe@mixit.fr");

        public TimelineMessagesRepositoryTest()
        {
            _repository = new TimelineMessageRepository();
        }

        [Fact]
        public void GivenAMessageSavedWhenGetMessagesOfUserThenMessageIsReturned()
        {
            _repository.Save(new TimelineMessageProjection(_ownerId, _authorId, "MessageA", MessageId.Generate()));

            Check.That(_repository.GetMessagesOfUser(_ownerId)).HasSize(1);
        }

        [Fact]
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
    }
}
