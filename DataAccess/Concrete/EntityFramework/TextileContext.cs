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

        public DbSet<AgeGroup> AgeGroup { get; set; }
        public DbSet<Arm> Arm { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<Belt> Belt { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Buttoning> Buttoning { get; set; }
        public DbSet<BuyingMethod> BuyingMethod { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Collection> Collection { get; set; }
        public DbSet<Country> Countrie { get; set; }
        public DbSet<CountryShippingMultiplier> CountryShippingMultiplier { get; set; }
        public DbSet<CsNoDeliveryDate> CsNoDeliveryDate { get; set; }
        public DbSet<CsNoDeliveryDateHistory> CsNoDeliveryDateHistory { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Detail> Detail { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<Fit> Fit { get; set; }
        public DbSet<FoldedProduct> FoldedProduct { get; set; }
        public DbSet<Form> Form { get; set; }
        public DbSet<Fur> Fur { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Handiwork> Handiwork { get; set; }
        public DbSet<Hierarchy> Hierarchie { get; set; }
        public DbSet<Hood> Hood { get; set; }
        public DbSet<ImportedLocal> ImportedLocal { get; set; }
        public DbSet<Label> Label { get; set; }
        public DbSet<Liner> Liner { get; set; }
        public DbSet<ManufacturingHeaders> ManufacturingHeader { get; set; }
        public DbSet<MaterialContent> MaterialContent { get; set; }
        public DbSet<ModelSeasonRowNumber> ModelSeasonRowNumber { get; set; }
        public DbSet<Neck> Neck { get; set; }
        public DbSet<Origin> Origin { get; set; }
        public DbSet<OutletSeason> OutletSeason { get; set; }
        public DbSet<Pattern> Pattern { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<PaymentMethodShare> PaymentMethodShare { get; set; }
        public DbSet<Phone> Phone { get; set; }
        public DbSet<ProductGroup> ProductGroup { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<SeasonCurrency> SeasonCurrency { get; set; }
        public DbSet<SeasonPlaning> SeasonPlaning { get; set; }
        public DbSet<ShipmentMethod> ShipmentMethod { get; set; }
        public DbSet<ShippingMethod> ShippingMethod { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffAuthorization> StaffAuthorization { get; set; }
        public DbSet<StaffEmail> StaffEmail { get; set; }
        public DbSet<StaffPhone> StaffPhone { get; set; }
        public DbSet<Style> Style { get; set; }
        public DbSet<SubBrand> SubBrand { get; set; }
        public DbSet<SubDetailGroup> SubDetailGroup { get; set; }
        public DbSet<SubGroup> SubGroup { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<SupplierMethod> SupplierMethod { get; set; }
        public DbSet<TariffNo> TariffNo { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<Waist> Waist { get; set; }
        public DbSet<Width> Width { get; set; }
    }
}
