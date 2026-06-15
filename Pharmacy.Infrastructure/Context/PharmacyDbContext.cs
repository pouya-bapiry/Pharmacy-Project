using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Account;
using Pharmacy.Domain.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Context
{
    public class PharmacyDbContext:DbContext
    {
        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options) : base(options) { }


        #region DB Set

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<SiteSetting> SiteSettings { get; set; }

        public DbSet<AboutUs> AboutUs { get; set; }

        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion

    }
}
