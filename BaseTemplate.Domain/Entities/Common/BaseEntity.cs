using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Domain.Entities.Common
{
    public abstract class BaseEntity : ICloneable
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
