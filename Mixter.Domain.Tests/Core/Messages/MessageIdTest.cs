using Mixter.Domain.Core.Messages;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Core.Messages
{
    public class MessageIdTest
    {
        [Fact]
        public void WhenGenerate2IdThenIsNotEquals()
        {
            var id1 = MessageId.Generate();
            var id2 = MessageId.Generate();

            Check.That(id1).IsNotEqualTo(id2);
        }

        [Fact]
        public void WhenToStringIdThenId()
        {
            var id = MessageId.Generate();

            Check.That(id.ToString()).IsEqualTo(id.Id);
        }
    }
}
