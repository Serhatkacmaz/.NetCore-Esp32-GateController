using GateController.Repository.Contexts;
using GateController.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateController.Repository
{
    public class OfficeMemberRepository
    {
        public async Task<List<OfficeMember>> GetAll()
        {
            using (var context = new ApplicationDbContext())
            {
                return await context.OfficeMembers.AsNoTracking().ToListAsync();
            }
        }
    }
}
