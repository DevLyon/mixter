using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core;
using NFluent;

namespace Mixter.Tests.Domain.Core
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

        [TestMethod]
        public void WhenCreateUserIdWithEmptyEmailThenThrowUserEmailCannotBeEmpty()
        {
            Check.ThatCode(() => new UserId(string.Empty)).Throws<UserEmailCannotBeEmpty>();
        }
    }
}
