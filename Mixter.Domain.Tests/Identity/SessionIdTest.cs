using Mixter.Domain.Identity;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Identity
{
    public class SessionIdTest
    {
        [Fact]
        public void WhenGenerate2SessionIdThenIsNotEquals()
        {
            var sessionId1 = SessionId.Generate();
            var sessionId2 = SessionId.Generate();

            Check.That(sessionId1).IsNotEqualTo(sessionId2);
        }
    }
}
