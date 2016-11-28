using GwcltdApp.Data.Configurations;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Data
{
    public class GwcltdAppContext : DbContext
    {
        public GwcltdAppContext()
            : base("GwcltdApp5")
        {
            Database.SetInitializer<GwcltdAppContext>(null);
        }

        #region Entity Sets
        public IDbSet<User> UserSet { get; set; }
        public IDbSet<Role> RoleSet { get; set; }
        public IDbSet<UserRole> UserRoleSet { get; set; }
        public IDbSet<Option> OptionSet { get; set; }
        public IDbSet<OptionType> OptionTypeSet { get; set; }
        public IDbSet<WSystem> WSystemSet { get; set; }
        public IDbSet<Production> ProductionSet { get; set; }
        public IDbSet<Error> ErrorSet { get; set; }
        public IDbSet<SMS_IN> SMS_INSet { get; set; }
        #endregion

        public virtual void Commit()
        {
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new WaterTypeConfiguration());
            modelBuilder.Configurations.Add(new TypeOptionConfiguration());
            modelBuilder.Configurations.Add(new WSystemConfiguration());
            modelBuilder.Configurations.Add(new ProductionConfiguration());
        }
    }
}
