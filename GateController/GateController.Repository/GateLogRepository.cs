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
    public class GateLogRepository
    {
        public async Task<List<GateLog>> GetAll()
        {
            using (var context = new ApplicationDbContext())
            {
                return await context.GateLogs.Include(x => x.OfficeMember).AsNoTracking().OrderByDescending(x => x.LoginDate).ToListAsync();
            }
        }

        public async Task InsertLogAsync(long cardId, long userId)
        {
            using (var context = new ApplicationDbContext())
            {
                context.GateLogs.Add(new GateLog { CardId = cardId, OfficeMemberId = userId });
                await context.SaveChangesAsync();
            }
        }
    }
}
