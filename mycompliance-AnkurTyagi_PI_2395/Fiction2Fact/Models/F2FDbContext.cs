using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using static Fiction2Fact.Models.General;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Web;
using System;
using System.Collections.Generic;
using Fiction2Fact.Libraries;
//using static Fiction2Fact.Models.Advertisement.AdvertisementMasters;

namespace Fiction2Fact.Models
{
    public class F2FDbContext : IdentityDbContext<ApplicationUser>
    {
        public F2FDbContext()
            : base(Global.AppDbType, throwIfV1Schema: false)
        {
            Database.SetInitializer<F2FDbContext>(null);
        }

        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<PageModel> Pages { get; set; }
        public DbSet<FeatureModel> Features { get; set; }
        public DbSet<CompanyProductModel> CompanyProducts { get; set; }
        public DbSet<CompanyProductFeatureModel> CompanyProductFeatures { get; set; }
        public DbSet<ProductMenuMappingModel> ProductMenuMappings { get; set; }
        public DbSet<F2FLogModel> F2FLogs { get; set; }

        public DbSet<MenuModel> Menus { get; set; }
        public ObjectResult<getMenuProductDetails> MenuString { get; set; }
        //public DbSet<CategoryModel> AdvtCategories { get; set; }
        //public DbSet<MediaModel> AdvtMedias { get; set; }
        //public DbSet<NatureModel> AdvtNatures { get; set; }
        //public DbSet<TypeModel> AdvtTypes { get; set; }


        public static F2FDbContext Create()
        {
            return new F2FDbContext();
        }

        public virtual ObjectResult<getRolesMapping_Result> getRolesMapping(string roleID)
        {
            var roleIDParameter = new ObjectParameter("RoleID", roleID);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getRolesMapping_Result>("getRolesMapping", roleIDParameter);
        }
    }
}