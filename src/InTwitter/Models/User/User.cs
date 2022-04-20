using InTwitter.Models.Base;
using System;

namespace InTwitter.Models.User
{
    public class User : EntityBase
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string HashPassword { get; set; }

        public string IconSource { get; set; }

        public string WallPaperSource { get; set; }

        public string SessionToken { get; set; }
    }
}
