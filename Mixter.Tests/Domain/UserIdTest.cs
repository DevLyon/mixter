using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain;
using NFluent;

namespace Mixter.Tests.Domain
{
    [TestClass]
    public class UserIdTest
    {
        private const string UserEmail = "test@mixit.fr";

        [TestMethod]
        public void WhenCreate2IdWithSameEmailsThenAreEquals()
        {
            var userId1 = new UserId(UserEmail);
            var userId2 = new UserId(UserEmail);

            Check.That(userId1).IsEqualTo(userId2);
        }

        [TestMethod]
        public void WhenToStringIdThenEmail()
        {
            var userId1 = new UserId(UserEmail);

            Check.That(userId1.ToString()).IsEqualTo(UserEmail);
        }
    }
}
