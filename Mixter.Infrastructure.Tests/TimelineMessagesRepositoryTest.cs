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

        [Fact]
        public void WhenSaveTwoSameMessagesThenOnlyOneIsSaved()
        {
            const string messageA = "MessageA";

            var timelineMessage = new TimelineMessageProjection(_ownerId, _authorId, messageA, MessageId.Generate());
            _repository.Save(timelineMessage);
            _repository.Save(timelineMessage);

            var messagesOfUser = _repository.GetMessagesOfUser(_ownerId).ToList();
            Check.That(messagesOfUser).HasSize(1);
        }

        [Fact]
        public void GivenAMessageSavedForSeveralUsersWhenRemoveThisMessageThenRemoveThisMessageOfAllUsers()
        {
            const string messageA = "MessageA";

            var messageId = MessageId.Generate();
            _repository.Save(new TimelineMessageProjection(_ownerId, _authorId, messageA, messageId));
            var message2 = new TimelineMessageProjection(_ownerId, _authorId, messageA, MessageId.Generate());
            _repository.Save(message2);
            var ownerId2 = new UserId("owner2@mix-it.fr");
            _repository.Save(new TimelineMessageProjection(ownerId2, _authorId, messageA, messageId));

            _repository.Delete(messageId);

            var messagesOfOwner1 = _repository.GetMessagesOfUser(_ownerId);
            Check.That(messagesOfOwner1).HasSize(1).And.ContainsExactly(message2);
            Check.That(_repository.GetMessagesOfUser(ownerId2)).IsEmpty();
        }
    }
}
