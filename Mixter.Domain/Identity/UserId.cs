namespace Mixter.Domain.Identity
{
    public struct UserId
    {
        public string Email { get; private set; }

        public UserId(string email)
            : this()
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new UserEmailCannotBeEmpty();
            }

            Email = email;
        }

        public override string ToString()
        {
            return Email;
        }
    }
}
