using System;

namespace Mixter.Domain.Core
{
    public class UserEmailCannotBeEmpty : Exception
    {
        public UserEmailCannotBeEmpty()
            : base("Email cannot be empty")
        {
        }
    }
}