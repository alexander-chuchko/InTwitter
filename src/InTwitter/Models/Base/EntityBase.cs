using System;

namespace InTwitter.Models.Base
{
    public abstract class EntityBase : IEntityBase
    {
        public Guid Id { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override bool Equals(object obj)
        {
            return this.Id == ((EntityBase)obj)?.Id;
        }
    }
}
