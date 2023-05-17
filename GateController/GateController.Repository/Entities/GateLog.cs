using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateController.Repository.Entities
{
    public class GateLog
    {
        public long Id { get; set; }
        public long CardId { get; set; }
        public long OfficeMemberId { get; set; }
        public DateTime LoginDate { get; set; } = DateTime.Now;
        public OfficeMember OfficeMember { get; set; }
    }
}
