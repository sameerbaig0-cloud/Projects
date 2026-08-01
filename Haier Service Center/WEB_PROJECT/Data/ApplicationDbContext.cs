using ServicePlatform.Controllers;
using ServicePlatform.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace ServicePlatform.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration; //?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration Configuration { get; }

        /* COMPANY INFO - STARTS */

        public virtual DbSet<CompanyInfo> CompanyInfos { get; set; }

        /* COMPANY INFO - ENDS */

        public virtual DbSet<UserCreationLink> UserCreationLinks { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<ApplicationName> ApplicationNames { get; set; }
        public virtual DbSet<MenuItemRoleMapping> MenuItemRoleMappings { get; set; }
        public virtual DbSet<UserMenuAccess> UserMenuAccesses { get; set; }
        //public virtual DbSet<IndentMaster> IndentMasters { get; set; }
        //public virtual DbSet<IndentApprovalHistory> IndentApprovalHistorys { get; set; }
        //public virtual DbSet<ItemTypeMaster> ItemTypeMasters { get; set; }
        public virtual DbSet<ItemMaster> ItemMasters { get; set; }
        //public virtual DbSet<ColorMaster> ColorMasters { get; set; }
        public virtual DbSet<UnitMaster> UnitMasters { get; set; }
        //public virtual DbSet<SizeMaster> SizeMasters { get; set; }
        //public virtual DbSet<ItemColorMergeMaster> ItemColorMergeMasters { get; set; }
        //public virtual DbSet<SupplierMaster> SupplierMasters { get; set; }
        //public virtual DbSet<CategorySubMaster> CategorySubMasters { get; set; }
        //public virtual DbSet<CategoryMaster> CategoryMasters { get; set; }
        //public virtual DbSet<ProcurementApprovalMaster> ProcurementApprovalMasters { get; set; }
        //public virtual DbSet<ProcurementApprovalUserMapping> ProcurementApprovalUserMappings { get; set; }
        //public virtual DbSet<IndentApprovalHistory> IndentApprovalHistories { get; set; }
        //public virtual DbSet<TaxMaster> TaxMasters { get; set; }
        //public virtual DbSet<SupplierPaymentTerm> SupplierPaymentTerms { get; set; }
        //public virtual DbSet<IndentMasterFile> IndentMasterFiles { get; set; }
        //public virtual DbSet<IndentMasterMaterialDisplay> IndentMasterMaterialDisplays { get; set; }
        //public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<PageAccessLog> PageAccessLogs { get; set; }
        public virtual DbSet<UserPageAuthorization> UserPageAuthorizations { get; set; }
        public virtual DbSet<ClickEventAuthorization> ClickEventAuthorizations { get; set; }
        //public virtual DbSet<LabelNew> LabelNews { get; set; }
        //public virtual DbSet<OrderMasterTrackingCodeQRCode> OrderMasterTrackingCodeQRCodes { get; set; }


        /* POS TABLES - STARTS */
        public virtual DbSet<POS_RetailStore> POS_RetailStores { get; set; }
        //public virtual DbSet<OrderMaster> OrderMasters { get; set; }
        //public virtual DbSet<OrderMasterTrackingCode> OrderMasterTrackingCodes { get; set; }
        //public virtual DbSet<ArticleMaster> ArticleMasters { get; set; }
        //public virtual DbSet<ArticlePicture> ArticlePictures { get; set; }
        //public virtual DbSet<ArticleBrandModel> ArticleBrandModels { get; set; }
        public virtual DbSet<POS_Customers> POS_Customers { get; set; }
        public virtual DbSet<POS_PaymentType> POS_PaymentTypes { get; set; }
        public virtual DbSet<POS_Invoice> POS_Invoices { get; set; }
        public virtual DbSet<POS_InvoiceItem> POS_InvoiceItems { get; set; }
        public virtual DbSet<POS_RetailPrice> POS_RetailPrices { get; set; }
        public virtual DbSet<POS_SalesScanning> POS_SalesScannings { get; set; }
        public virtual DbSet<POS_Taxation> POS_Taxations { get; set; }
        public virtual DbSet<POS_PackingToRetailTransfer> POS_PackingToRetailTransfers { get; set; }


        /* SERVICE TABLE - STARTS */
        public virtual DbSet<ShowRoom> ShowRooms { get; set; }
        public virtual DbSet<Location> Places { get; set; }


        // Add DbSet for the view
        //public DbSet<POS_SalesScanningSummary> POS_SalesScanningSummary { get; set; }
        //public DbSet<TrailingOrderVw> TrailingOrderVw { get; set; }


        //public List<OrderMasterTrackingCodeGenResult> GetOrderMasterTrackingCode(string orderNo, string article, string size, bool isSizeWise)
        //{
        //	var orderNoParam = new SqlParameter("@OrderNo", orderNo);
        //	var articleParam = new SqlParameter("@Article", article);
        //	var sizeParam = new SqlParameter("@Size", size);
        //	var isSizeWiseParam = new SqlParameter("@IsSizeWise", isSizeWise);

        //	var result = this.Set<OrderMasterTrackingCodeGenResult>()
        //		.FromSqlRaw("EXEC dbo.OrderMasterTrackingCodeGen_SP @OrderNo, @Article, @Size, @IsSizeWise", orderNoParam, articleParam, sizeParam, isSizeWiseParam)
        //		.ToList();

        //	return result;
        //}


        /* POS TABLES - ENDS */

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(@"server=ITSOFTWARE\MSSQLSERVER22;database=PMC_Test;Integrated Security=true;TrustServerCertificate=true;MultipleActiveResultSets=true;");

                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Connectionstr"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            //modelBuilder.Entity<ItemTypeMaster>()
            //    .HasMany(e => e.IndentMaster)
            //    .WithOne(e => e.ItemTypeMaster)
            //    .HasForeignKey(e => e.ItemType)
            //    .IsRequired();

            //modelBuilder.Entity<ColorMaster>()
            //    .HasMany(e => e.IndentMaster)
            //    .WithOne(e => e.ColorMaster)
            //    .HasForeignKey(e => e.ItemColor)
            //    .IsRequired();

            /* APPLICATION IDENTITY TABLE DESIGN - STARTS */


            base.OnModelCreating(modelBuilder);
            //modelBuilder.HasDefaultSchema("identity");

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User", schema: "identity");
            });

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role", schema: "identity");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
             {
                 entity.ToTable("UserRoles", schema: "identity");
             });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", schema: "identity");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", schema: "identity");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims", schema: "identity");

            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", schema: "identity");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserCreationLink", schema: "identity");
            });


            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserMenuAccess", schema: "identity");
            });


            /* APPLICATION IDENTITY TABLE DESIGN - ENDS */


            modelBuilder.Entity<UserCreationLink>(entity =>
            {
                entity.HasKey("CreationID");

                entity.HasOne(u => u.ApplicationUser)
                .WithMany(x => x.UserCreationLinks)
                .HasPrincipalKey(u => u.Id)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.NoAction);
            });



            ////modelBuilder.HasDefaultSchema("dbo");
            //modelBuilder.Entity<ItemMaster>(entity =>
            //{
            //    entity.HasKey("ItemID");
            //    entity.Property<string>(a => a.ItemCode).IsRequired();
            //    entity.Property<string>(a => a.ItemName).IsRequired();

            //});


            //modelBuilder.Entity<ColorMaster>(entity =>
            //{
            //	entity.HasKey("SCode");
            //	entity.Property<string>(a => a.Code).IsRequired();
            //	entity.Property<string>(a => a.Name).IsRequired();
            //});


            modelBuilder.Entity<UnitMaster>(entity =>
            {
                entity.HasKey("UnitID");
                entity.Property<string>(a => a.UnitCode).IsRequired();
                entity.Property<string>(a => a.UnitName).IsRequired();
            });

            //modelBuilder.Entity<SizeMaster>(entity =>
            //{
            //	entity.HasKey("Name");
            //});

            //modelBuilder.Entity<SupplierMaster>(entity =>
            //{
            //	entity.HasKey("Code");
            //	entity.Property<string>(a => a.Code).IsRequired();
            //	entity.Property<string>(a => a.Name).IsRequired();
            //});


            //modelBuilder.Entity<ItemColorMergeMaster>(entity =>
            //{
            //	entity.HasKey("SNo");
            //	entity.Property(t => t.SNo).HasPrecision(18, 0);
            //	entity.Property<string>(a => a.ItemType).IsRequired();
            //	entity.Property<string>(a => a.ItemColor).IsRequired();
            //});


            //modelBuilder.Entity<CategorySubMaster>(entity =>
            //{
            //	entity.HasKey("Code");
            //	entity.Property<string>(a => a.Name).IsRequired();
            //	entity.Property<string>(a => a.MainCatName).IsRequired();
            //});


            //modelBuilder.Entity<CategoryMaster>(entity =>
            //{
            //	entity.HasKey("Code");
            //	entity.Property<string>(a => a.Name).IsRequired();
            //});


            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.ToTable("MenuItems")
                .HasKey(e => e.Id);
            });


            //modelBuilder.Entity<RoleUser>(entity =>
            //{
            //	entity.ToTable("Roles")
            //	.HasKey(e => e.RoleId);
            //});

            modelBuilder.Entity<ApplicationName>(entity =>
            {
                entity.ToTable("ApplicationName")
                .HasKey(e => e.SNo);
            });


            modelBuilder.Entity<UserMenuAccess>(entity =>
            {

                entity.HasOne(u => u.ApplicationUser)
                .WithMany(m => m.UserMenuAccesses)
                .HasPrincipalKey(u => u.Id)
                .HasForeignKey(m => m.UserID)
                .OnDelete(DeleteBehavior.NoAction);


                entity.HasOne(u => u.ApplicationName)
                .WithMany(m => m.UserMenuAccesses)
                .HasPrincipalKey(u => u.SNo)
                .HasForeignKey(m => m.ApplicationNameId)
                .OnDelete(DeleteBehavior.NoAction);

            });


            //modelBuilder.Entity<PurchaseOrder>(entity =>
            //{
            //	entity.HasKey("SNo");
            //});


            modelBuilder.Entity<MenuItemRoleMapping>(entity =>
            {
                entity.HasKey("Id");

                entity.HasOne(m => m.MenuItem)
                .WithMany(i => i.MenuItemRoleMappings)
                .HasPrincipalKey(m => m.Id)
                .HasForeignKey(i => i.MenuItemId)
                .OnDelete(DeleteBehavior.NoAction);


                entity.HasOne(m => m.ApplicationName)
                .WithMany(i => i.MenuItemRoleMappings)
                .HasPrincipalKey(m => m.SNo)
                .HasForeignKey(i => i.ApplicationNameId)
                .OnDelete(DeleteBehavior.NoAction);



            });


            //modelBuilder.Entity<ProcurementApprovalMaster>(entity =>
            //{
            //	entity.HasKey(e => e.Id);
            //});

            //modelBuilder.Entity<ProcurementApprovalUserMapping>(entity =>
            //{
            //	entity.HasKey(e => e.Id);
            //});


            //modelBuilder.Entity<IndentMasterMaterialDisplay>(entity =>
            //{
            //	entity.HasKey(e => e.ID);
            //});

            //    modelBuilder.Entity<IndentMaster>().HasKey(am => new
            //    {
            //        am.ItemType,
            //        am.ItemColor
            //    });


            //    modelBuilder.Entity<IndentMaster>().HasOne(m => m.ItemTypeMaster)
            //.WithMany(am => am.PurchaseRequisitions).HasPrincipalKey(am => am.Code).HasForeignKey(m => m.ItemType);

            //    modelBuilder.Entity<IndentMaster>().HasOne(a => a.ColorMaster)
            //    .WithMany(at => at.PurchaseRequisitions).HasPrincipalKey(at => at.SCode).HasForeignKey(a => a.ItemColor);


            modelBuilder.Entity<PageAccessLog>(entity =>
            {
                entity.HasOne(u => u.ApplicationUser)
                 .WithMany(p => p.PageAccesses)
                 .HasPrincipalKey(u => u.Id)
                 .HasForeignKey(p => p.UserId)
                 .OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<ClickEventAuthorization>(entity =>
            {
                entity.HasOne(u => u.ApplicationUser)
                 .WithMany(p => p.ClickEventAuthorizations)
                 .HasPrincipalKey(u => u.Id)
                 .HasForeignKey(p => p.UserId)
                 .OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasMany(y => y.Submenu)
                .WithOne()
                .HasForeignKey(y => y.ParentId);

            });


            modelBuilder.Entity<MenuItemRoleMapping>(entity =>
            {
                //		entity.HasOne(i => i.RoleUser)
                //.WithMany(p => p.MenuItemRoleMappings)
                //.HasPrincipalKey(i => i.RoleId)
                //.HasForeignKey(p => p.RoleId)
                //.OnDelete(DeleteBehavior.NoAction);


                entity.HasOne(i => i.ApplicationName)
                .WithMany(p => p.MenuItemRoleMappings)
                .HasPrincipalKey(i => i.SNo)
                .HasForeignKey(p => p.ApplicationNameId)
                .OnDelete(DeleteBehavior.NoAction);



                entity.HasOne(i => i.MenuItem)
                .WithMany(p => p.MenuItemRoleMappings)
                .HasPrincipalKey(i => i.Id)
                .HasForeignKey(p => p.MenuItemId)
                .OnDelete(DeleteBehavior.NoAction);

            });

            modelBuilder.Entity<UserPageAuthorization>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasOne(i => i.ApplicationUser)
                .WithMany(u => u.UserPageAuthorizations)
                .HasPrincipalKey(i => i.Id)
                .HasForeignKey(u => u.UserID)
                .OnDelete(DeleteBehavior.NoAction);


                entity.HasOne(m => m.MenuItem)
                .WithMany(u => u.UserPageAuthorizations)
                .HasPrincipalKey(m => m.Id)
                .HasForeignKey(u => u.MenuItemsID)
                .OnDelete(DeleteBehavior.NoAction);
            });


            //	modelBuilder.Entity<CategorySubMaster>(entity =>
            //	{
            //		entity.HasOne(i => i.CategoryMaster)
            //				.WithMany(p => p.CategorySubMasters)
            //				.HasPrincipalKey(i => i.Code)
            //				.HasForeignKey(p => p.MainCatName)
            //				.OnDelete(DeleteBehavior.NoAction);
            //	});


            modelBuilder.Entity<ItemMaster>(entity =>
            {
                entity.HasKey(i => i.ItemID);

                entity.HasOne(i => i.UnitMaster)
                      .WithMany(u => u.ItemMasters)
                      .HasPrincipalKey(i => i.UnitID)
                      .HasForeignKey(u => u.UnitID)
                      .OnDelete(DeleteBehavior.NoAction);
            });


            //	modelBuilder.Entity<ItemTypeMaster>(entity =>
            //	{
            //		entity.HasOne(i => i.UnitMaster)
            //				.WithMany(p => p.ItemTypeMasters)
            //				.HasPrincipalKey(i => i.Code)
            //				.HasForeignKey(p => p.Unit)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		entity.HasOne(i => i.CategorySubMaster)
            //				.WithMany(p => p.ItemTypeMasters)
            //				.HasPrincipalKey(i => i.Code)
            //				.HasForeignKey(p => p.Catagory)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		//entity.HasMany(i => i.TaxMaster)
            //		//    .WithMany(x => x.ItemTypeMasters)

            //		entity.HasOne(i => i.TaxMaster)
            //			  .WithMany(P => P.ItemTypeMasters)
            //			  .HasPrincipalKey(i => i.ID)
            //			  .HasForeignKey(p => p.HSNNo)
            //			  .OnDelete(DeleteBehavior.NoAction);

            //		//    .WithMany()
            //		//    .HasForeignKey(i => i.HSNNo)
            //		//    .HasPrincipalKey(t => t.HSN)
            //		//    .OnDelete(DeleteBehavior.NoAction)
            //		//    .HasConstraintName("FK_ItemTypeMaster_TaxMaster")
            //		//    .IsRequired(false) // Optional, if HSNNo is not always required
            //		//    .Metadata.PrincipalToDependent?.SetPropertyAccessMode(PropertyAccessMode.Field);
            //	});


            //	modelBuilder.Entity<IndentMaster>(entity =>
            //	{
            //		entity.HasOne(i => i.ItemTypeMaster)
            //				.WithMany(p => p.IndentMasters)
            //				.HasPrincipalKey(i => i.Code)
            //				.HasForeignKey(p => p.ItemType)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		entity.HasOne(i => i.ColorMaster)
            //				.WithMany(p => p.IndentMasters)
            //				.HasPrincipalKey(i => i.SCode)
            //				.HasForeignKey(p => p.ItemColor)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		entity.HasOne(i => i.SupplierMaster)
            //				.WithMany(p => p.IndentMasters)
            //				.HasPrincipalKey(i => i.Code)
            //				.HasForeignKey(p => p.SupplierID)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		entity.HasOne(i => i.SizeMaster)
            //				.WithMany(p => p.IndentMasters)
            //				.HasPrincipalKey(i => i.Name)
            //				.HasForeignKey(p => p.Size)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		entity.HasOne(i => i.UnitMaster)
            //				.WithMany(p => p.IndentMasters)
            //				.HasPrincipalKey(i => i.Name)
            //				.HasForeignKey(p => p.UOM)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		//entity.HasOne(i => i.IndentApprovalHistory)
            //		//              .WithMany(p => p.IndentMasters)
            //		//              .HasPrincipalKey(i => i.IndentMasterId)
            //		//              .HasForeignKey(p => p.Sno)
            //		//              .OnDelete(DeleteBehavior.NoAction);


            //		entity.HasOne(i => i.SupplierPaymentTerm)
            //				.WithMany(p => p.IndentMasters)
            //				.HasPrincipalKey(i => i.ID)
            //				.HasForeignKey(p => p.SupplierPaymentTermID)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		entity.HasMany(i => i.PurchaseOrders)
            //				.WithOne()
            //				.HasForeignKey(i => i.IndentNo)
            //				.OnDelete(DeleteBehavior.NoAction);



            //		entity.HasMany(i => i.IndentMasterFiles)
            //			  .WithOne()
            //			  .HasForeignKey(i => i.IndentMasterID)
            //			  .OnDelete(DeleteBehavior.NoAction);




            //		entity.HasMany(i => i.IndentApprovalHistories)
            //			  .WithOne(p => p.IndentMaster)
            //			  .HasForeignKey(i => i.IndentMasterId)
            //			  .HasPrincipalKey(p => p.Sno)
            //			  .OnDelete(DeleteBehavior.NoAction);


            //		entity.HasOne(i => i.ApplicationUser)
            //				.WithMany(p => p.IndentMasters)
            //				.HasPrincipalKey(i => i.AccountID)
            //				.HasForeignKey(p => p.Loguser)
            //				.OnDelete(DeleteBehavior.NoAction);

            //	});


            //	modelBuilder.Entity<ProcurementApprovalUserMapping>(entity =>
            //{
            //	entity.HasOne(u => u.ApplicationUser)
            //			.WithMany(a => a.ProcurementApprovalUserMappings)
            //			.HasPrincipalKey(u => u.Id)
            //			.HasForeignKey(a => a.UserId)
            //			.OnDelete(DeleteBehavior.NoAction);

            //	entity.HasOne(p => p.ProcurementApprovalMaster)
            //			.WithMany(x => x.ProcurementApprovalUserMappings)
            //			.HasPrincipalKey(p => p.Id)
            //			.HasForeignKey(x => x.ApprovalMasterId)
            //			.OnDelete(DeleteBehavior.NoAction);

            //});


            //	modelBuilder.Entity<IndentApprovalHistory>(entity =>
            //	{
            //		entity.HasKey(e => e.Id);

            //		entity.HasOne(u => u.ApplicationUser)
            //			.WithMany(a => a.IndentApprovalHistories)
            //			.HasPrincipalKey(u => u.AccountID)
            //			.HasForeignKey(a => a.ApproverAccountID)
            //			.OnDelete(DeleteBehavior.NoAction);

            //		//entity.HasOne(p => p.IndentMaster)
            //		//		.WithMany(i => i.IndentApprovalHistories)
            //		//		.HasPrincipalKey(p => p.Sno)
            //		//		.HasForeignKey(i => i.IndentMasterId)
            //		//		.OnDelete(DeleteBehavior.NoAction);

            //		entity.HasOne(e => e.IndentMaster)
            //			  .WithMany()
            //			  .HasForeignKey(e => e.IndentMasterId)
            //			  .IsRequired();

            //	});


            //	modelBuilder.Entity<TaxMaster>(entity =>
            //	{
            //		entity.HasKey(e => e.ID);

            //		//entity.HasMany(i => i.ItemTypeMasters)
            //		//      .WithOne()
            //		//      .HasForeignKey(i => i.HSNNo)
            //		//      .HasPrincipalKey(t => t.HSN)
            //		//      .OnDelete(DeleteBehavior.NoAction);
            //		//      //.HasConstraintName("FK_TaxMaster_ItemTypeMaster");

            //		//entity.HasQueryFilter(t => t.IsActive);
            //		////The HasQueryFilter method is applied directly to the TaxMaster entity, ensuring that only active TaxMaster records are considered in the relationship.

            //	});


            //	modelBuilder.Entity<SupplierPaymentTerm>(entity =>
            //	{
            //		entity.HasKey(e => e.ID);

            //		//entity.HasMany(i => i.IndentMasters)
            //		//      .WithOne(x => x.SupplierPaymentTerm)
            //		//      .HasForeignKey(i => i.SupplierPaymentTermID)
            //		//      .HasPrincipalKey(x => x.ID)
            //		//      .OnDelete(DeleteBehavior.NoAction);                    

            //	});


            //	modelBuilder.Entity<IndentMasterFile>(entity =>
            //	{
            //		entity.HasKey(e => e.ID);

            //		//entity.HasOne(e => e.IndentMaster)
            //		//    .WithMany(x => x.IndentMasterFiles)
            //		//    .HasPrincipalKey(e => e.Sno)
            //		//    .HasForeignKey(x => x.IndentMasterID)
            //		//    .OnDelete(DeleteBehavior.NoAction);

            //		//entity.HasOne(i => i.IndentMaster)
            //		//   .WithMany()
            //		//   .HasForeignKey(x => x.IndentMasterID)
            //		//   .HasPrincipalKey(i => i.Sno)
            //		//   .IsRequired();

            //	});


            //	modelBuilder.Entity<SupplierItemTable>(entity =>
            //	{
            //		entity.HasOne(i => i.ItemTypeMaster)
            //				.WithMany(p => p.SupplierItemTables)
            //				.HasPrincipalKey(i => i.Code)
            //				.HasForeignKey(p => p.ItemType)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		entity.HasOne(i => i.ColorMaster)
            //				.WithMany(p => p.SupplierItemTables)
            //				.HasPrincipalKey(i => i.SCode)
            //				.HasForeignKey(p => p.ItemColor)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		entity.HasOne(i => i.SupplierMaster)
            //				.WithMany(p => p.SupplierItemTables)
            //				.HasPrincipalKey(i => i.Code)
            //				.HasForeignKey(p => p.SupplierName)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		entity.HasOne(i => i.SizeMaster)
            //				.WithMany(p => p.SupplierItemTables)
            //				.HasPrincipalKey(i => i.Name)
            //				.HasForeignKey(p => p.Size)
            //				.OnDelete(DeleteBehavior.NoAction);

            //	});


            //	modelBuilder.Entity<PurchaseOrder>(entity =>
            //	{
            //		entity.HasKey(e => e.SNo);

            //		entity.HasOne(i => i.IndentMaster)
            //				.WithMany(p => p.PurchaseOrders)
            //				.HasPrincipalKey(i => i.Refno)
            //				.HasForeignKey(p => p.IndentNo)
            //				.OnDelete(DeleteBehavior.NoAction);

            //		////entity.HasOne(i => i.ItemTypeMaster)
            //		////		.WithMany(p => p.PurchaseOrders)
            //		////		.HasPrincipalKey(i => i.Code)
            //		////		.HasForeignKey(p => p.ItemType)
            //		////		.OnDelete(DeleteBehavior.NoAction);

            //		////entity.HasOne(i => i.ColorMaster)
            //		////		.WithMany(p => p.PurchaseOrders)
            //		////		.HasPrincipalKey(i => i.SCode)
            //		////		.HasForeignKey(p => p.ItemColor)
            //		////		.OnDelete(DeleteBehavior.NoAction);

            //		////entity.HasOne(i => i.SupplierMaster)
            //		////		.WithMany(p => p.PurchaseOrders)
            //		////		.HasPrincipalKey(i => i.Code)
            //		////		.HasForeignKey(p => p.SupplierName)
            //		////		.OnDelete(DeleteBehavior.NoAction);

            //		////entity.HasOne(i => i.SizeMaster)
            //		////		.WithMany(p => p.PurchaseOrders)
            //		////		.HasPrincipalKey(i => i.Name)
            //		////		.HasForeignKey(p => p.Size)
            //		////		.OnDelete(DeleteBehavior.NoAction);

            //		////entity.HasOne(i => i.TaxMaster)
            //		////		.WithMany(p => p.PurchaseOrders)
            //		////		.HasPrincipalKey(i => i.ID)
            //		////		.HasForeignKey(p => p.TaxID)
            //		////		.OnDelete(DeleteBehavior.NoAction);

            //	});



            modelBuilder.Entity<CompanyInfo>(entity =>
        {
            entity.HasNoKey();
        });


            /* POS SALES - STARTS */

            modelBuilder.Entity<POS_RetailStore>(entity =>
            {
                entity.HasKey(s => s.StoreID);

            });


            modelBuilder.Entity<POS_Taxation>(entity =>
            {
                entity.HasKey(e => e.TaxID);

            });


            //modelBuilder.Entity<ArticleBrandModel>(entity =>
            //{
            //	entity.HasKey(e => e.ID);

            //});


            //modelBuilder.Entity<ArticleMaster>(entity =>
            //{
            //	entity.HasKey(e => e.Article);


            //	entity.HasOne(b => b.ArticleBrandModel)
            //	.WithMany(a => a.ArticleMasters)
            //	//.HasPrincipalKey(b => b.ID)
            //	.HasForeignKey(a => a.ArtBrand)
            //	.OnDelete(DeleteBehavior.NoAction);
            //});


            //modelBuilder.Entity<ArticlePicture>(entity =>
            //{
            //	entity.HasKey(e => e.PictureID);


            //	entity.HasOne(b => b.ApplicationUser)
            //	.WithMany(a => a.ArticlePictures)
            //	.HasPrincipalKey(b => b.Id)
            //	.HasForeignKey(a => a.UserID)
            //	.OnDelete(DeleteBehavior.NoAction);


            //	entity.HasOne(b => b.ArticleMaster)
            //	.WithMany(a => a.ArticlePictures)
            //	.HasPrincipalKey(b => b.Article)
            //	.HasForeignKey(a => a.Article)
            //	.OnDelete(DeleteBehavior.NoAction);
            //});


            // 2. PaymentType to Invoice (One-to-Many)
            modelBuilder.Entity<POS_Invoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceID);

                entity.HasOne(c => c.POS_Customer)
                     .WithMany(p => p.POS_Invoices)
                     .HasPrincipalKey(c => c.CustomerID)
                     .HasForeignKey(p => p.CustomerID)
                     .OnDelete(DeleteBehavior.NoAction);


                entity.HasOne(i => i.POS_PaymentType)
                    .WithMany(pt => pt.POS_Invoices)
                    .HasPrincipalKey(i => i.PaymentTypeID)
                    .HasForeignKey(pt => pt.PaymentTypeID)
                    .OnDelete(DeleteBehavior.NoAction);


                entity.HasOne(u => u.ApplicationUser)
                    .WithMany(pt => pt.POS_Invoices)
                    .HasPrincipalKey(u => u.Id)
                    .HasForeignKey(pt => pt.UserID)
                    .OnDelete(DeleteBehavior.NoAction);


                entity.HasOne(r => r.POS_RetailStore)
                    .WithMany(pt => pt.POS_Invoices)
                    .HasPrincipalKey(r => r.StoreID)
                    .HasForeignKey(pt => pt.StoreID)
                    .OnDelete(DeleteBehavior.NoAction);

            });



            //// 3. Invoice to InvoiceItem (One-to-Many)
            //modelBuilder.Entity<POS_InvoiceItem>(entity =>
            //{
            //	entity.HasKey(i => i.InvoiceItemID);


            //	entity.HasOne(i => i.POS_Invoice)
            //	.WithMany(it => it.POS_InvoiceItems)
            //	.HasPrincipalKey(i => i.InvoiceID)
            //	.HasForeignKey(it => it.InvoiceID)
            //	.OnDelete(DeleteBehavior.NoAction);


            //	entity.HasOne(a => a.ArticleMaster)
            //	.WithMany(it => it.POS_InvoiceItems)
            //	//.HasPrincipalKey(a => a.Article)
            //	.HasForeignKey(it => it.ArticleNo)
            //	.OnDelete(DeleteBehavior.NoAction);


            //	entity.HasOne(t => t.POS_Taxation)
            //	.WithMany(it => it.POS_InvoiceItems)
            //	.HasPrincipalKey(t => t.TaxID)
            //	.HasForeignKey(it => it.TaxID)
            //	.OnDelete(DeleteBehavior.NoAction);

            //});


            //modelBuilder.Entity<POS_RetailPrice>(entity =>
            //{
            //	entity.HasKey(p => p.PriceID);


            //	entity.HasOne(p => p.ArticleMaster)
            //	.WithMany(it => it.POS_RetailPrices)
            //	.HasPrincipalKey(p => p.Article)
            //	.HasForeignKey(it => it.ArticleNo)
            //	.OnDelete(DeleteBehavior.NoAction);


            //	entity.HasOne(t => t.POS_Taxation)
            //	.WithMany(r => r.POS_RetailPrices)
            //	.HasPrincipalKey(t => t.TaxID)
            //	.HasForeignKey(r => r.TaxID)
            //	.OnDelete(DeleteBehavior.NoAction);

            //	entity.HasOne(t => t.ApplicationUser)
            //	.WithMany(r => r.POS_RetailPrices)
            //	.HasPrincipalKey(t => t.Id)
            //	.HasForeignKey(r => r.UserID)
            //	.OnDelete(DeleteBehavior.NoAction);

            //});


            modelBuilder.Entity<POS_PackingToRetailTransfer>(entity =>
            {
                entity.HasKey(p => p.TransferID);


                entity.HasOne(p => p.ApplicationUser)
                .WithMany(it => it.POS_PackingToRetailTransfers)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(it => it.UserID)
                .OnDelete(DeleteBehavior.NoAction);


                entity.HasOne(t => t.POS_RetailStore)
                .WithMany(r => r.POS_PackingToRetailTransfers)
                .HasPrincipalKey(t => t.StoreID)
                .HasForeignKey(r => r.StoreID)
                .OnDelete(DeleteBehavior.NoAction);

            });



            //modelBuilder.Entity<POS_SalesScanning>(entity =>
            //{
            //	entity.HasOne(t => t.POS_Taxation)
            //	.WithMany(s => s.POS_SalesScannings)
            //	.HasPrincipalKey(t => t.TaxID)
            //	.HasForeignKey(s => s.TaxID)
            //	.OnDelete(DeleteBehavior.NoAction);

            //});

            /* POS SALES - ENDS */


            /* SHOW ROOM */

            modelBuilder.Entity<ShowRoom>(entity =>
            {
                entity.HasKey(e => e.ShowRoomId);

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Phone).IsUnique();

                entity.Property(e => e.GSTNumber)
                      .HasMaxLength(15);

                entity.Property(e => e.Latitude)
                      .HasColumnType("decimal(9,6)");

                entity.Property(e => e.Longitude)
                      .HasColumnType("decimal(9,6)");

                entity.Property(e => e.CreatedDate)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasQueryFilter(e => !e.IsDeleted);

            });


            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.SNo);
            });

        }



    }


    //public virtual DbSet<POS_Sales> POS_Sales { get; set; } = default!;

}

