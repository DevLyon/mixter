using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Core.Messages
{
    public class UpdateTimelineTest
    {
        private const string Content = "Hello";

        private static readonly UserId Author = new UserId("author@mixit.fr");
        private static readonly MessageId MessageId = MessageId.Generate();

        private readonly UpdateTimeline _handler;
        private readonly TimelineMessageRepository _repository;

        public UpdateTimelineTest()
        {
            _repository = new TimelineMessageRepository();
            _handler = new UpdateTimeline(_repository);
        }

        [Fact]
        public void WhenHandleMessageQuackedThenSaveTimelineMessageProjectionForAuthor()
        {
            _handler.Handle(new MessageQuacked(MessageId, Author, Content));

            Check.That(_repository.GetMessagesOfUser(Author))
                 .ContainsExactly(new TimelineMessageProjection(Author, Author, Content, MessageId));
        }
    }
}
