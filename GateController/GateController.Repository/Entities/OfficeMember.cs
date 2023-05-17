using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateController.Repository.Entities
{
    public class OfficeMember
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<SystemCard> SystemCards { get; set; }
    }
}
