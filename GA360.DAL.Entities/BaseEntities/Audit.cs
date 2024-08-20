using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.BaseEntities
{
    public class Audit:BaseEntity
    {
        //
        // Summary:
        //     Is this entity Deleted?
        public virtual bool IsDeleted { get; set; }

        //
        // Summary:
        //     Which user deleted this entity?
        public virtual long? DeleterUserId { get; set; }

        //
        // Summary:
        //     Deletion time of this entity.
        public virtual DateTime? DeletionTime { get; set; }
    }
}
