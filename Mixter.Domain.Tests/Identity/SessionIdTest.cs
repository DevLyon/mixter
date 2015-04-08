using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Identity;
using NFluent;

namespace Mixter.Domain.Tests.Identity
{
    [TestClass]
    public class SessionIdTest
    {
        [TestMethod]
        public void WhenGenerate2SessionIdThenIsNotEquals()
        {
            var sessionId1 = SessionId.Generate();
            var sessionId2 = SessionId.Generate();

            Check.That(sessionId1).IsNotEqualTo(sessionId2);
        }
    }
}
