using System;
using Prism.Mvvm;

namespace InTwitter.Models.Base
{
    public abstract class EntityViewModelBase : BindableBase
    {
        public virtual Guid Id { get; set; }

        public override bool Equals(object obj) => (obj as EntityViewModelBase)?.Id == this.Id;

        public override int GetHashCode() => HashCode.Combine(Id);
    }
}
