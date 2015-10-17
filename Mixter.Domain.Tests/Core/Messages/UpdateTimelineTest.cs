using Mixter.Domain.Core.Messages.Handlers;

namespace Mixter.Domain.Tests.Core.Messages
{
    public class UpdateTimelineTest
    {
        private readonly UpdateTimeline _handler;

        public UpdateTimelineTest()
        {
            _handler = new UpdateTimeline();
        }
    }
}
