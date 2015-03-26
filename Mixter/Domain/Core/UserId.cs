namespace Mixter.Domain.Core
{
    public struct UserId
    {
        public string Email { get; private set; }

        public UserId(string email) 
            :this()
        {
            Email = email;
        }

        public override string ToString()
        {
            return Email;
        }
    }
}