using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using NFluent;

namespace Mixter.Tests.Domain
{
    [TestClass]
    public class UserIdTest
    {
        [TestMethod]
        public void WhenCreate2IdWithSameEmailsThenAreEquals()
        {
            var userId1 = new UserId("test@mixit.fr");
            var userId2 = new UserId("test@mixit.fr");

            Check.That(userId1).IsEqualTo(userId2);
        }

        [TestMethod]
        public void WhenToStringIdThenEmail()
        {
            var userId1 = new UserId("test@mixit.fr");

            Check.That(userId1.ToString()).IsEqualTo("test@mixit.fr");
        }
    }
}
