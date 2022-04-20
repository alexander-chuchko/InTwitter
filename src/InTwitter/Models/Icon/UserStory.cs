using InTwitter.Models.Base;
using System;
using System.Drawing;

namespace InTwitter.Models.Icon
{
    public class UserStory : EntityBase
    {
        public Guid UserId { get; set; }
        public string UserIcon { get; set; }
        public string Name { get; set; }
        public bool IsLoaded { get; set; }
        public bool IsAuthorized { get; set; }
        public Color OutlineСolor { get; set; }
    }
}
