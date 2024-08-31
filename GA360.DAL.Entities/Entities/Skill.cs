using GA360.DAL.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.Entities
{
    public class Skill: Audit
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
