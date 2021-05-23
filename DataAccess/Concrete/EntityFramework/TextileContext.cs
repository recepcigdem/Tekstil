using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class TextileContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=Textile;Trusted_Connection=true");
            //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Textile;User ID=sa;Password=1234Recep;TrustServerCertificate=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CS_AI");
        }

        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<CountryShippingMultiplier> CountryShippingMultiplier { get; set; }
        public DbSet<CsNoDeliveryDate> CsNoDeliveryDate { get; set; }
        public DbSet<CsNoDeliveryDateHistory> CsNoDeliveryDateHistory { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Definition> Definition { get; set; }
        public DbSet<DefinitionTitle> DefinitionTitles { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<Hierarchy> Hierarchy { get; set; }
        public DbSet<Label> Label { get; set; }
        public DbSet<ModelSeasonRowNumber> ModelSeasonRowNumber { get; set; }
        public DbSet<PaymentMethodShare> PaymentMethodShare { get; set; }
        public DbSet<Phone> Phone { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<SeasonCurrency> SeasonCurrency { get; set; }
        public DbSet<SeasonPlaning> SeasonPlaning { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffAuthorization> StaffAuthorization { get; set; }
        public DbSet<StaffEmail> StaffEmail { get; set; }
        public DbSet<StaffPhone> StaffPhone { get; set; }
        public DbSet<TariffNo> TariffNo { get; set; }
        public DbSet<TariffNoDetail> TariffNoDetail { get; set; }
        public DbSet<Test> Test { get; set; }
    }
}
