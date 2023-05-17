using GateController.Repository.Contexts;
using GateController.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GateController.Repository
{
    public class SystemCardsRepository
    {
        public SystemCard GetCard(string cardHex)
        {
            using (var context = new ApplicationDbContext())
            {
                var card = context.SystemCards.Include(x => x.OfficeMember).FirstOrDefault(x => x.CardHex == cardHex);
                return card;
            }
        }

        public async Task<bool> NewCard(SystemCard card)
        {
            using (var context = new ApplicationDbContext())
            {
                context.SystemCards.Add(card);
                var result = await context.SaveChangesAsync();
                return result > 1;
            }
        }

    }
}
