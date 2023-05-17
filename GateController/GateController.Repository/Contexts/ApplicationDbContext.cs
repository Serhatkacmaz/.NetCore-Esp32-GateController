using GateController.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateController.Repository.Contexts
{
    public class ApplicationDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=sql.poseidon.domainhizmetleri.com;Database=metince1_gateDb;User Id=metince1_gatedbUser;Password=wvE5.624u;TrustServerCertificate=true");
        }

        public virtual DbSet<OfficeMember> OfficeMembers { get; set; }
        public virtual DbSet<SystemCard> SystemCards { get; set; }
        public virtual DbSet<SystemDevice> SystemDevices { get; set; }
        public virtual DbSet<GateLog> GateLogs { get; set; }
    }
}
