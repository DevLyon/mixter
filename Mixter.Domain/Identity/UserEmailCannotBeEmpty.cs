namespace Mixter.Domain.Identity
{
    public class UserEmailCannotBeEmpty : DomainException
    {
        public UserEmailCannotBeEmpty()
            : base("Email cannot be empty")
        {
        }
    }
}