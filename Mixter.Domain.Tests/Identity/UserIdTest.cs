using Mixter.Domain.Identity;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Identity
{
    public class UserIdTest
    {
        private const string UserEmail = "test@mixit.fr";

        [Fact]
        public void WhenCreate2IdWithSameEmailsThenAreEquals()
        {
            var userId1 = new UserId(UserEmail);
            var userId2 = new UserId(UserEmail);

            Check.That(userId1).IsEqualTo(userId2);
        }

        [Fact]
        public void WhenToStringIdThenEmail()
        {
            var userId1 = new UserId(UserEmail);

            Check.That(userId1.ToString()).IsEqualTo(UserEmail);
        }

        [Fact]
        public void WhenCreateUserIdWithEmptyEmailThenThrowUserEmailCannotBeEmpty()
        {
            Check.ThatCode(() => new UserId(string.Empty)).Throws<UserEmailCannotBeEmpty>();
        }
    }
}
