using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class TekstilContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Db_Tekstil;Trusted_Connection=true");

        }

        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<Arm> Arms { get; set; }
        public DbSet<Belt> Belts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Buttoning> Buttonings { get; set; }
        public DbSet<BuyingMethod> BuyingMethods { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryShippingMultiplier> CountryShippingMultipliers { get; set; }
        public DbSet<CsNoDeliveryDate> CsNoDeliveryDates { get; set; }
        public DbSet<CsNoDeliveryDateHistory> CsNoDeliveryDateHistories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Fit> Fits { get; set; }
        public DbSet<FoldedProduct> FoldedProducts { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Fur> Furs { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Handiwork> Handiworks { get; set; }
        public DbSet<Hierarchy> Hierarchies { get; set; }
        public DbSet<Hood> Hoods { get; set; }
        public DbSet<ImportedLocal> ImportedLocals { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Liner> Liners { get; set; }
        public DbSet<ManufacturingHeaders> ManufacturingHeaders { get; set; }
        public DbSet<MaterialContent> MaterialContents { get; set; }
        public DbSet<ModelSeasonRowNumber> ModelSeasonRowNumbers { get; set; }
        public DbSet<Neck> Necks { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<OutletSeason> OutletSeasons { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentMethodShare> PaymentMethodShares { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<SeasonCurrency> SeasonCurrencies { get; set; }
        public DbSet<SeasonPlaning> SeasonPlanings { get; set; }
        public DbSet<ShipmentMethod> ShipmentMethods { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffAuthorization> StaffAuthorizations { get; set; }
        public DbSet<StaffEmail> StaffEmails { get; set; }
        public DbSet<StaffPhone> StaffPhones { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<SubBrand> SubBrands { get; set; }
        public DbSet<SubDetailGroup> SubDetailGroups { get; set; }
        public DbSet<SubGroup> SubGroups { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierMethod> SupplierMethods { get; set; }
        public DbSet<TariffNo> TariffNos { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Waist> Waists { get; set; }
        public DbSet<Width> Widths { get; set; }
    }
}
