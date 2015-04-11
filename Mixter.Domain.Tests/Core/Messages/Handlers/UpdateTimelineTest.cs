using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using NFluent;

namespace Mixter.Domain.Tests.Core.Messages.Handlers
{
    [TestClass]
    public class UpdateTimelineTest
    {
        private const string Content = "Hello";

        private static readonly UserId Author = new UserId("author@mixit.fr");
        private static readonly MessageId MessageId = MessageId.Generate();

        private TimelineMessagesRepository _repository;
        private UpdateTimeline _handler;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new TimelineMessagesRepository();
            _handler = new UpdateTimeline(_repository);
        }

        [TestMethod]
        public void WhenHandleMessagePublishedThenSaveTimelineMessageProjectionForAuthor()
        {
            _handler.Handle(new MessagePublished(MessageId, Author, Content));

            Check.That(_repository.GetMessagesOfUser(Author))
                 .ContainsExactly(new TimelineMessageProjection(Author, Author, Content, MessageId));
        }
    }
}
