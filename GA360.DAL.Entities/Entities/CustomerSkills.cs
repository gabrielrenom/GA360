using GA360.DAL.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA360.DAL.Entities.Entities
{
    public class CustomerSkills : Audit, IModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int SkillId {  get; set; }
        public Customer Customer { get; set; }
        public Skill Skill { get; set; }
    }
}