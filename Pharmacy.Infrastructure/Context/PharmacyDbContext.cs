using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Account;
using Pharmacy.Domain.Entities.Contact;
using Pharmacy.Domain.Entities.Product;
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
        public  DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SiteBanner> SiteBanners { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<ProductDiscountUse> ProductDiscountUses { get; set; }

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
