using System;

namespace Mixter.Domain.Identity
{
    public class UserEmailCannotBeEmpty : Exception
    {
        public UserEmailCannotBeEmpty()
            : base("Email cannot be empty")
        {
        }
    }
}