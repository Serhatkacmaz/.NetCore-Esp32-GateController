using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateController.Repository.Entities
{
    public class SystemCard
    {
        public long Id { get; set; }
        public string CardHex { get; set; }
        public long OfficeMemberId { get; set; }
        public OfficeMember OfficeMember { get; set; }
    }
}
