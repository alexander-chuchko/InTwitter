using System;

namespace InTwitter.Models.Base
{
    public interface IVersionController
    {
        Guid Id { get; set; }
        long Version { get; set; }
    }
}
