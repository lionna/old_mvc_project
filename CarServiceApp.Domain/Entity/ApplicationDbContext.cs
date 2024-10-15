using CarServiceApp.Domain.Config;
using CarServiceApp.Domain.Entity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarServiceApp.Domain.Entity
{
    public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
#pragma warning disable CS9113 // Parameter is unread.
        IOptions<ApplicationSettings> settings) : IdentityDbContext(options)
#pragma warning restore CS9113 // Parameter is unread.
    {
        #region MyRegion

        public virtual DbSet<AcceptanceDocument> AcceptanceDocument { get; set; }

        public virtual DbSet<AcceptanceCustomSpart> AcceptanceCustomSpart { get; set; }

        public virtual DbSet<AcceptanceInvoice> AcceptanceInvoice { get; set; }

        public virtual DbSet<Car> Car { get; set; }

        public virtual DbSet<CarBrand> CarBrand { get; set; }

        public virtual DbSet<CarColor> CarColor { get; set; }

        public virtual DbSet<CarGeneration> CarGeneration { get; set; }

        public virtual DbSet<CarModel> CarModel { get; set; }

        public virtual DbSet<CarModification> CarModification { get; set; }

        public virtual DbSet<CarSeries> CarSeries { get; set; }

        public virtual DbSet<Client> Client { get; set; }

        public virtual DbSet<ContractToEmployee> ContractToEmployee { get; set; }

        public virtual DbSet<Department> Department { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<ExecutingService> ExecutingService { get; set; }

        public virtual DbSet<FullStockOfSparePart> FullStockOfSparePart { get; set; }

        public virtual DbSet<GeneratedClient> GeneratedClient { get; set; }

        public virtual DbSet<ImportedStreet> ImportedStreet { get; set; }

        public virtual DbSet<Invoice> Invoice { get; set; }

        public virtual DbSet<OrderService> OrderService { get; set; }

        public virtual DbSet<Post> Post { get; set; }

        public virtual DbSet<PreRecord> PreRecord { get; set; }

        public virtual DbSet<PreRecordService> PreRecordService { get; set; }

        public virtual DbSet<Provider> Provider { get; set; }

        public virtual DbSet<RemarkToStateCar> RemarkToStateCar { get; set; }

        public virtual DbSet<Models.Service> Service { get; set; }

        public virtual DbSet<ServiceType> ServiceType { get; set; }

        public virtual DbSet<SparePartMaterial> SparePartMaterial { get; set; }

        public virtual DbSet<TypeOfPayment> TypeOfPayment { get; set; }

        public virtual DbSet<UsingCustomSpartMat> UsingCustomSpartMat { get; set; }

        public virtual DbSet<UsingPartMaterial> UsingPartMaterial { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer(
                "data source=DESKTOP-75RK6VE\\SQLEXPRESS;initial catalog=CarServiceDatabase;" +
                "integrated security=True; Encrypt=False;"
                // settings?.Value?.DefaultConnection
                , b => b.MigrationsAssembly("CarServiceApp"));


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        // Укажите строку подключения и сборку миграций здесь
        //        optionsBuilder.UseSqlServer("data source=DESKTOP-0P259QK\\LOCALMSSQLSERVER;initial catalog=CarServiceDatabase; integrated security=True; Encrypt=False;TrustServerCertificate=true");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AcceptanceDocument>(entity =>
            {
                entity.HasKey(e => e.AcceptanceId);

                entity.ToTable("AcceptDocument");

                entity.Property(e => e.AcceptanceId).HasColumnName("AcceptanceId");
                entity.Property(e => e.AcceptDate).HasColumnType("datetime");
                entity.Property(e => e.ClientId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ClientId");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.AcceptanceDocuments)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BringParts");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.AcceptanceDocuments)
                    .HasForeignKey(d => d.PersonnelNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Accept");
            });

            modelBuilder.Entity<AcceptanceCustomSpart>(entity =>
            {
                entity.HasKey(e => new { e.PartId, e.AcceptanceId })
                    .HasName("XPKAcceptanceCustomSParts");

                entity.ToTable("AcceptanceCustomSpart");

                entity.Property(e => e.PartId)
                    .HasMaxLength(40)
                    .HasColumnName("PartId");
                entity.Property(e => e.AcceptanceId).HasColumnName("AcceptanceId");
                entity.Property(e => e.StateSpart)
                    .HasMaxLength(20)
                    .HasColumnName("StateSpart");

                entity.HasOne(d => d.AcceptDocument)
                    .WithMany(p => p.AcceptanceCustomSparts)
                    .HasForeignKey(d => d.AcceptanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Incl");

                entity.HasOne(d => d.SparePartMaterial)
                    .WithMany(p => p.AcceptanceCustomSparts)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fixed");
            });

            modelBuilder.Entity<AcceptanceInvoice>(entity =>
            {
                entity.HasKey(e => e.PositionId).HasName("XPKInvoice");

                entity.ToTable("AcceptanceInvoice");

                entity.Property(e => e.PositionId).HasColumnName("PositionId");
                entity.Property(e => e.PartId)
                    .HasMaxLength(40)
                    .HasColumnName("PartId");
                entity.Property(e => e.Price).HasColumnType("money");
                entity.Property(e => e.TradeIncrease).HasColumnName("TradeIncrease");

                entity.HasOne(d => d.SparePartMaterial)
                    .WithMany(p => p.AcceptanceInvoices)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fixed_in");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.AcceptanceInvoices)
                    .HasForeignKey(d => d.LotNumber)
                    .HasConstraintName("IncludePart");
            });


            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(e => e.VinNumber).HasName("XPKCar");

                entity.ToTable("Car");

                entity.Property(e => e.VinNumber)
                    .HasMaxLength(17)
                    .HasColumnName("VinNumber");
                entity.Property(e => e.DataSheetCar).HasMaxLength(20);
                entity.Property(e => e.ColorId).HasColumnName("ColorId");
                entity.Property(e => e.ModificationId).HasColumnName("ModificationId");
                entity.Property(e => e.OwnerName).HasMaxLength(150);
                entity.Property(e => e.RegistrationNumber)
                    .HasMaxLength(20)
                    .HasColumnName("RegistrationNumber");
                entity.Property(e => e.TransmissionType)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.CarColor).WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("FK_Car__CarColor");

                entity.HasOne(d => d.CarModification).WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ModificationId)
                    .HasConstraintName("FK_Car__CarModification");
            });

            modelBuilder.Entity<CarBrand>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CarBrands");

                entity.ToTable("CarBrand");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("BrandId");
                entity.Property(e => e.Name)
                    .HasMaxLength(120)
                    .HasColumnName("BrandName");
                entity.Property(e => e.ViewBrand).HasColumnName("ViewBrand");
            });

            modelBuilder.Entity<CarColor>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__CarColor_table");

                entity.ToTable("CarColor");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ColorId");
                entity.Property(e => e.Hex)
                    .HasMaxLength(7)
                    .IsFixedLength()
                    .HasColumnName("Hex");
                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("Name");
                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("Description");
                entity.Property(e => e.IsMetallic).HasColumnName("IsMetallic");
            });

            modelBuilder.Entity<CarGeneration>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK___importe__F887D65A7C2A7AF2");

                entity.ToTable("CarGeneration");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("GenerationId");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("GenerationName");
                entity.Property(e => e.ModelId).HasColumnName("ModelId");
                entity.Property(e => e.YearBegin)
                    .HasDefaultValueSql("(NULL)")
                    .HasColumnName("YearBegin");
                entity.Property(e => e.YearEnd)
                    .HasDefaultValueSql("(NULL)")
                    .HasColumnName("YearEnd");

                entity.HasOne(d => d.CarModel).WithMany(p => p.CarGenerations)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CarGeneration__CarModels");
            });

            modelBuilder.Entity<CarModel>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CarModels");

                entity.ToTable("CarModel");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ModelId");
                entity.Property(e => e.BrandId).HasColumnName("BrandId");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("ModelName");

                entity.HasOne(d => d.CarBrand).WithMany(p => p.CarModels)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CarModels_CarBrands");

                entity.Ignore(c => c.DropdownList);
            });



            //modelBuilder.Entity<CarModel>()
            //.Ignore(c => c.DropdownList);


            modelBuilder.Entity<CarModification>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CarModification");

                entity.ToTable("CarModification");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ModificationId");
                entity.Property(e => e.EndProductionYear).HasColumnName("EndProductionYear");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("ModificationName");
                entity.Property(e => e.SeriesId).HasColumnName("SeriesId");
                entity.Property(e => e.StartProductionYear).HasColumnName("StartProductionYear");

                entity.HasOne(d => d.Series).WithMany(p => p.CarModifications)
                    .HasForeignKey(d => d.SeriesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CarModification__CarSeries");
            });

            modelBuilder.Entity<CarSeries>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CarSeries");

                entity.ToTable("CarSeries");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("SeriesId");
                entity.Property(e => e.GenerationId).HasColumnName("GenerationId");
                entity.Property(e => e.ModelId).HasColumnName("ModelId");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("SeriesName");

                entity.HasOne(d => d.CarGeneration).WithMany(p => p.CarSeries)
                    .HasForeignKey(d => d.GenerationId)
                    .HasConstraintName("FK__CarSeries__CarGeneration");

                entity.HasOne(d => d.CarModel).WithMany(p => p.CarSeries)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CarSeries__CarModels");

                entity.Ignore(c => c.DropdownList);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId).HasName("XPKClient");

                entity.ToTable("Client");

                entity.Property(e => e.ClientId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ClientId");
                entity.Property(e => e.UserId)
                   .HasColumnName("UserId");
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.FullName).HasMaxLength(150);
                entity.Property(e => e.Phone).HasMaxLength(100);
            });

            modelBuilder.Entity<ContractToEmployee>(entity =>
            {
                entity.HasKey(e => e.ContractId);

                entity.ToTable("ContractToEmployee");

                entity.Property(e => e.ContractId).HasColumnName("ContractId");
                entity.Property(e => e.Type)
                    .HasMaxLength(40)
                    .IsFixedLength();

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.ContractToEmployees)
                    .HasForeignKey(d => d.PersonnelNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Worked_by");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("XPKDepartment");

                entity.ToTable("Department");

                entity.Property(e => e.Id).HasColumnName("DepartmentId");
                entity.Property(e => e.Name).HasMaxLength(80);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.PersonnelNumber).HasName("XPKEmployee");

                entity.ToTable("Employee");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentId");
                entity.Property(e => e.PostId)
                      .HasColumnName("PostId");
                entity.Property(e => e.UserId)
                    .HasColumnName("UserId");
                entity.Property(e => e.FullName).HasMaxLength(150);
                entity.Property(e => e.PassportIssuedBy).HasMaxLength(50);
                entity.Property(e => e.PassportIssuedDate).HasColumnType("datetime");
                entity.Property(e => e.PassportPrivateNumber).HasMaxLength(20);
                entity.Property(e => e.PassportSeries)
                    .HasMaxLength(5)
                    .IsUnicode(false);
                entity.Property(e => e.PassportValidityDate).HasColumnType("datetime");
                entity.Property(e => e.Phone).HasMaxLength(100);
                entity.Property(e => e.ResidentialAddress).HasMaxLength(50);

                entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("Has");

                entity.HasOne(d => d.Post).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("Belongs");
            });

            modelBuilder.Entity<ExecutingService>(entity =>
            {
                entity.HasKey(e => new { e.ServiceId, e.OrderId })
                    .HasName("XPKExecutingServices");

                entity.Property(e => e.ServiceId)
                    .HasMaxLength(20)
                    .HasColumnName("ServiceId");
                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .HasColumnName("OrderId");
                entity.Property(e => e.DateCompleting).HasColumnType("datetime");
                entity.Property(e => e.DateStart).HasColumnType("datetime");
                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.OrderService)
                    .WithMany(p => p.ExecutingServices)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Registr_in");

                entity.HasOne(d => d.Service).WithMany(p => p.ExecutingServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Contents_in");

                entity.HasOne(d => d.Employee).WithMany(p => p.ExecutingServices)
                    .HasForeignKey(d => d.PersonnelNumber)
                    .HasConstraintName("Executes");
            });

            modelBuilder.Entity<FullStockOfSparePart>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("FullStockOfSpareParts");

                entity.Property(e => e.Cost).HasColumnType("money");
                entity.Property(e => e.PartId)
                    .HasMaxLength(40)
                    .HasColumnName("PartId");
                entity.Property(e => e.PositionId).HasColumnName("PositionId");
                entity.Property(e => e.Manufacture).HasMaxLength(150);
                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.FullName).HasMaxLength(150);
                entity.Property(e => e.Price).HasColumnType("money");
                entity.Property(e => e.TradeIncrease).HasColumnName("TradeIncrease");
                entity.Property(e => e.Unit)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GeneratedClient>(entity =>
            {
                entity.HasKey(e => e.ClientId).HasName("PK__GeneratedClients");

                entity.ToTable("GeneratedClient");

                entity.Property(e => e.ClientId).HasColumnName("ClientId");
                entity.Property(e => e.FullName).HasMaxLength(150);
            });

            modelBuilder.Entity<ImportedStreet>(entity =>
            {
                entity.ToTable("ImportedStreet");

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name).HasMaxLength(1000);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.LotNumber);

                entity.ToTable("Invoice");

                entity.Property(e => e.LotNumber).ValueGeneratedNever();
                entity.Property(e => e.ProviderId)
                    .HasMaxLength(20)
                    .HasColumnName("ProviderId");

                entity.HasOne(d => d.Provider).WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.ProviderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Deliver");

                entity.HasOne(d => d.Employee).WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.PersonnelNumber)
                    .HasConstraintName("Take_in");
            });



            modelBuilder.Entity<OrderService>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("XPKOrderServ");

                entity.ToTable("OrderService");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .HasColumnName("OrderId");
                entity.Property(e => e.DateCompleting).HasColumnType("datetime");
                entity.Property(e => e.DateFactCompleting).HasColumnType("datetime");
                entity.Property(e => e.DateMakingOrder).HasColumnType("datetime");
                entity.Property(e => e.FuelLevelPercent).HasDefaultValue((byte)50);
                entity.Property(e => e.ClientId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ClientId");
                entity.Property(e => e.PaymentId).HasColumnName("PaymentId");
                entity.Property(e => e.IsAntenna).HasDefaultValue(false);
                entity.Property(e => e.IsCoverDecorEngine).HasDefaultValue(false);
                entity.Property(e => e.IsSpareWheel).HasDefaultValue(false);
                entity.Property(e => e.IsTuner).HasDefaultValue(false);
                entity.Property(e => e.NumberWheelCaps).HasDefaultValue((byte)0);
                entity.Property(e => e.NumberWipers).HasDefaultValue((byte)0);
                entity.Property(e => e.NumberWipersArms).HasDefaultValue((byte)0);
                entity.Property(e => e.RejectionReason).HasMaxLength(150);
                entity.Property(e => e.VinNumber)
                    .HasMaxLength(17)
                    .HasColumnName("VinNumber");

                entity.HasOne(d => d.Client).WithMany(p => p.OrderServices)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderService_Client");

                entity.HasOne(d => d.TypeOfPayment)
                    .WithMany(p => p.OrderServices)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_OrderService_TypeOfPayment");

                entity.HasOne(d => d.Car).WithMany(p => p.OrderServices)
                    .HasForeignKey(d => d.VinNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderServ_Car");
            });


            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("XPKPost");

                entity.ToTable("Post");

                entity.Property(e => e.Id)
                    .HasColumnName("PostId");
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasMany(d => d.ServiceTypes).WithMany(p => p.Posts)
                    .UsingEntity<Dictionary<string, object>>(
                        "ServiceTypePostEmployee",
                        r => r.HasOne<ServiceType>().WithMany()
                            .HasForeignKey("TypeId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_ServiceTypePostEmployee_ServiceType"),
                        l => l.HasOne<Post>().WithMany()
                            .HasForeignKey("PostId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_ServiceTypePostEmployee_Post"),
                        j =>
                        {
                            j.HasKey("PostId", "TypeId");
                            j.ToTable("ServiceTypePostEmployee");
                            j.IndexerProperty<int>("PostId")
                                .HasColumnName("PostId");
                            j.IndexerProperty<int>("TypeId").HasColumnName("TypeId");
                        });
            });

            modelBuilder.Entity<PreRecord>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.ToTable("PreRecord");

                entity.Property(e => e.RecordId).HasColumnName("RecordId");
                entity.Property(e => e.DateMakingRecord).HasColumnType("datetime");
                entity.Property(e => e.MarkModelCar).HasMaxLength(100);
                entity.Property(e => e.FullName).HasMaxLength(150);
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(100)
                    .HasColumnName("PhoneNumber");
                entity.Property(e => e.RegNumberCar).HasMaxLength(10);

                entity.HasOne(d => d.Employee).WithMany(p => p.PreRecords)
                    .HasForeignKey(d => d.PersonnelNumber)
                    .HasConstraintName("Record");
            });

            modelBuilder.Entity<PreRecordService>(entity =>
            {
                entity.HasKey(e => new { e.RecordId, e.ServiceId });

                entity.Property(e => e.RecordId).HasColumnName("RecordId");
                entity.Property(e => e.ServiceId)
                    .HasMaxLength(20)
                    .HasColumnName("ServiceId");
                entity.Property(e => e.DateReservation).HasColumnType("datetime");

                entity.HasOne(d => d.PreRecord)
                    .WithMany(p => p.PreRecordServices)
                    .HasForeignKey(d => d.RecordId)
                    .HasConstraintName("HasServ");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.PreRecordServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BelongsPreRecord");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.PreRecordServices)
                    .HasForeignKey(d => d.PersonnelNumber)
                    .HasConstraintName("PreExec");
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("XPKProvider");

                entity.ToTable("Provider");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .HasColumnName("ProviderId");
                entity.Property(e => e.Address).HasMaxLength(150);
                entity.Property(e => e.CertificateNumber).HasMaxLength(30);
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("Email");
                entity.Property(e => e.Name).HasMaxLength(150);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            modelBuilder.Entity<RemarkToStateCar>(entity =>
            {
                entity.HasKey(e => e.RemarkId);

                entity.ToTable("RemarkToStateCar");

                entity.Property(e => e.RemarkId).HasColumnName("RemarkId");
                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .HasColumnName("OrderId");
                entity.Property(e => e.RemarkText).HasMaxLength(150);
                entity.Property(e => e.XAxisPos).HasColumnName("XAxisPos");
                entity.Property(e => e.YAxisPos).HasColumnName("YAxisPos");

                entity.HasOne(d => d.OrderService)
                    .WithMany(p => p.RemarkToStateCars)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RemarkToStateCar_OrderServ");
            });


            modelBuilder.Entity<Models.Service>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("XPKService");

                entity.ToTable("Service");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .HasColumnName("ServiceId");
                entity.Property(e => e.TypeId).HasColumnName("TypeId");
                entity.Property(e => e.PriceHour).HasColumnType("money");
                entity.Property(e => e.Name).HasMaxLength(300);

                entity.HasOne(d => d.ServiceType).WithMany(p => p.Services)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Service_ServiceType");

                entity.Ignore(c => c.DropdownList);
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("XPKServiceType");

                entity.ToTable("ServiceType");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("TypeId");
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<SparePartMaterial>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("XPKSparePartMaterial");

                entity.ToTable("SparePartMaterial");

                entity.Property(e => e.Id)
                    .HasMaxLength(40)
                    .HasColumnName("PartId");
                entity.Property(e => e.Manufacture).HasMaxLength(150);
                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Unit)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<TypeOfPayment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("XPKTypeOfPayment");

                entity.ToTable("TypeOfPayment");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("PaymentId");
                entity.Property(e => e.Name).HasMaxLength(50);
            });


            modelBuilder.Entity<UsingCustomSpartMat>(entity =>
            {
                entity.HasKey(e => new
                { e.AcceptanceId, e.PartId, e.ServiceId, e.OrderId });

                entity.ToTable("UsingCustomSpartMat");

                entity.Property(e => e.AcceptanceId).HasColumnName("AcceptanceId");
                entity.Property(e => e.PartId)
                    .HasMaxLength(40)
                    .HasColumnName("PartId");
                entity.Property(e => e.ServiceId)
                    .HasMaxLength(20)
                    .HasColumnName("ServiceId");
                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .HasColumnName("OrderId");

                entity.HasOne(d => d.AcceptanceCustomSpart)
                    .WithMany(p => p.UsingCustomSpartMats)
                    .HasForeignKey(d => new { d.PartId, d.AcceptanceId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Use_in2");

                entity.HasOne(d => d.ExecutingService)
                    .WithMany(p => p.UsingCustomSpartMats)
                    .HasForeignKey(d => new { d.ServiceId, d.OrderId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Apply_to2");
            });

            modelBuilder.Entity<UsingPartMaterial>(entity =>
            {
                entity.HasKey(e => new { e.PositionId, e.ServiceId, e.OrderId });

                entity.ToTable("UsingPartMaterial");

                entity.Property(e => e.PositionId).HasColumnName("PositionId");
                entity.Property(e => e.ServiceId)
                    .HasMaxLength(20)
                    .HasColumnName("ServiceId");
                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .HasColumnName("OrderId");
                entity.Property(e => e.CostPart)
                    .HasColumnType("money")
                    .HasColumnName("CostPart");

                entity.HasOne(d => d.AcceptanceInvoice)
                    .WithMany(p => p.UsingPartMaterials)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Use_parts");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.UsingPartMaterials)
                    .HasForeignKey(d => d.PersonnelNumber)
                    .HasConstraintName("Accept_to_Work");

                entity.HasOne(d => d.ExecutingService)
                    .WithMany(p => p.UsingPartMaterials)
                    .HasForeignKey(d => new { d.ServiceId, d.OrderId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Apply_to");
            });


            base.OnModelCreating(modelBuilder);

            // Disable primary key for IdentityUserLogin<string>
            // modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();

            //OnModelCreatingPartial(modelBuilder);

            //base.OnModelCreating(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}



////        public DbSet<AcceptanceInvoice> AcceptanceInvoice { get; set; }
////        public DbSet<AcceptanceCustomSParts> AcceptanceCustomSParts { get; set; }
////        public DbSet<AcceptDocument> AcceptDocument { get; set; }
////        public DbSet<Car> Car { get; set; }
////        public DbSet<CarBrands> CarBrands { get; set; }
////        public DbSet<CarColor> CarColor { get; set; }
////        public DbSet<CarGeneration> CarGeneration { get; set; }
////        public DbSet<CarModels> CarModels { get; set; }
////        public DbSet<CarModification> CarModification { get; set; }
////        public DbSet<CarSeries> CarSeries { get; set; }
////        public DbSet<Client> Client { get; set; }
////        public DbSet<ContractToEmployee> ContractToEmployee { get; set; }
////        public DbSet<Department> Department { get; set; }
////        public DbSet<Employee> Employee { get; set; }
////        public DbSet<ExecutingServices> ExecutingServices { get; set; }
////        public DbSet<GeneratedClient> GeneratedClient { get; set; }
////        public DbSet<Invoice> Invoice { get; set; }
////        public DbSet<OrderService> OrderService { get; set; }
////        public DbSet<Post> Post { get; set; }
////        public DbSet<PreRecord> PreRecord { get; set; }
////        public DbSet<PreRecordServices> PreRecordServices { get; set; }
////        public DbSet<Provider> Provider { get; set; }
////        public DbSet<RemarkToStateCar> RemarkToStateCar { get; set; }
////        public DbSet<entity.Service> Service { get; set; }
////        public DbSet<ServiceType> ServiceType { get; set; }
////        public DbSet<SparePartMaterial> SparePartMaterial { get; set; }
////        public DbSet<TypeOfPayment> TypeOfPayment { get; set; }
////        public DbSet<UsingCustomSPartMat> UsingCustomSPartMat { get; set; }
////        public DbSet<UsingPartMaterial> UsingPartMaterial { get; set; }

////        protected override void OnModelCreating(ModelBuilder modelBuilder)
////        {

////            modelBuilder.Entity<AcceptanceInvoice>()
////                .Property(e => e.Price)
////                .HasPrecision(19, 4);

////            //modelBuilder.Entity<AcceptanceInvoice>()
////            //    .HasMany(e => e.UsingPartMaterial)
////            //    .WithRequired(e => e.AcceptanceInvoice)
////            //    .WillCascadeOnDelete(false);

////            modelBuilder.Entity<AcceptanceInvoice>()
////                .HasMany(e => e.UsingPartMaterial)
////                .WithOne(e => e.AcceptanceInvoice)
////                .HasForeignKey(e => e.PositionId)
////                .OnDelete(DeleteBehavior.Cascade);

////            //modelBuilder.Entity<AcceptanceCustomSParts>()
////            //    .HasMany(e => e.UsingCustomSPartMat)
////            //    .WithRequired(e => e.AcceptanceCustomSParts)
////            //    .HasForeignKey(e => new { PartId = e.PartId, AcceptanceId = e.AcceptanceId })
////            //    .WillCascadeOnDelete(false);

////            modelBuilder.Entity<AcceptanceCustomSParts>()
////                .HasMany(e => e.UsingCustomSPartMat)
////                .WithOne(e => e.AcceptanceCustomSParts)
////                .HasForeignKey(e => new { e.PartId, e.AcceptanceId })
////                //.HasForeignKey(e => e.AcceptanceId)
////                .OnDelete(DeleteBehavior.Cascade);


////            modelBuilder.Entity<AcceptDocument>()
////                .Property(e => e.ClientId)
////                .IsUnicode(false);

////            //modelBuilder.Entity<AcceptDocument>()
////            //    .HasMany(e => e.AcceptanceCustomSParts)
////            //    .WithRequired(e => e.AcceptDocument)
////            //    .WillCascadeOnDelete(false);


////            modelBuilder.Entity<AcceptDocument>()
////                .HasMany(e => e.AcceptanceCustomSParts)
////                .WithOne(e => e.AcceptDocument)
////                .HasForeignKey(e => e.AcceptanceId)
////                .OnDelete(DeleteBehavior.Cascade);

////            modelBuilder.Entity<Car>()
////                .Property(e => e.TransmissionType)
////                .IsUnicode(false);

////            //modelBuilder.Entity<Car>()
////            //    .HasMany(e => e.OrderService)
////            //    .WithRequired(e => e.Car)
////            //    .WillCascadeOnDelete(false);

////            modelBuilder.Entity<Car>()
////                .HasMany(e => e.OrderService)
////                .WithOne(e => e.Car)
////                .HasForeignKey(e => e.VinNumber) 
////                .OnDelete(DeleteBehavior.Restrict);

////            //modelBuilder.Entity<CarBrands>()
////            //    .HasMany(e => e.CarModels)
////            //    .WithRequired(e => e.CarBrands)
////            //    .WillCascadeOnDelete(false);

////            modelBuilder.Entity<CarBrands>()
////                .HasMany(e => e.CarModels)
////                .WithOne(e => e.CarBrands)
////                .HasForeignKey(e => e.BrandId)  
////                .OnDelete(DeleteBehavior.Restrict); 

////            modelBuilder.Entity<CarColor>()
////                .Property(e => e.ColorHex)
////                .IsFixedLength();

////            //modelBuilder.Entity<CarColor>()
////            //    .HasMany(e => e.Car)
////            //    .WithOptional(e => e.CarColor)
////            //    .HasForeignKey(e => e.BodyColorId);

////            modelBuilder.Entity<CarColor>()
////                .HasMany(e => e.Car)
////                .WithOne(e => e.CarColor)
////                .HasForeignKey(e => e.ColorId)  
////                .OnDelete(DeleteBehavior.Restrict);


////            //modelBuilder.Entity<CarModels>()
////            //    .HasMany(e => e.CarGeneration)
////            //    .WithRequired(e => e.CarModels)
////            //    .WillCascadeOnDelete(false);

////            modelBuilder.Entity<CarModels>()
////                .HasMany(e => e.CarGeneration)
////                .WithOne(e => e.CarModels)
////                .HasForeignKey(e => e.ModelId) 
////                .OnDelete(DeleteBehavior.Restrict);


////            //modelBuilder.Entity<CarModels>()
////            //    .HasMany(e => e.CarSeries)
////            //    .WithRequired(e => e.CarModels)
////            //    .WillCascadeOnDelete(false);


////            modelBuilder.Entity<CarModels>()
////                .HasMany(e => e.CarSeries)
////                .WithOne(e => e.CarModels)
////                .HasForeignKey(e => e.ModelId)  
////                .OnDelete(DeleteBehavior.Restrict);


////            //modelBuilder.Entity<CarSeries>()
////            //    .HasMany(e => e.CarModification)
////            //    .WithRequired(e => e.CarSeries)
////            //    .WillCascadeOnDelete(false);

////            modelBuilder.Entity<CarSeries>()
////                .HasMany(e => e.CarModification)
////                .WithOne(e => e.CarSeries)
////                .HasForeignKey(e => e.SeriesId) 
////                .OnDelete(DeleteBehavior.Restrict);  



////            modelBuilder.Entity<Client>()
////                .Property(e => e.ClientId)
////                .IsUnicode(false);

////            //modelBuilder.Entity<Client>()
////            //    .HasMany(e => e.AcceptDocument)
////            //    .WithRequired(e => e.Client)
////            //    .WillCascadeOnDelete(false);


////            //modelBuilder.Entity<Client>()
////            //    .HasMany(e => e.OrderService)
////            //    .WithRequired(e => e.Client)
////            //    .WillCascadeOnDelete(false);


////            modelBuilder.Entity<AcceptDocument>()
////                .HasOne(e => e.Client)
////                .WithMany(e => e.AcceptDocument)
////                .HasForeignKey(e => e.ClientId)
////                .OnDelete(DeleteBehavior.Restrict);   

////            modelBuilder.Entity<OrderService>()
////                .HasOne(e => e.Client)
////                .WithMany(e => e.OrderService)
////                .HasForeignKey(e => e.ClientId)
////                .OnDelete(DeleteBehavior.Restrict); 



////            modelBuilder.Entity<ContractToEmployee>()
////                .Property(e => e.Type)
////                .IsFixedLength();

////            modelBuilder.Entity<Employee>()
////                .Property(e => e.PassSeries)
////                .IsUnicode(false);

////            modelBuilder.Entity<Employee>()
////                .Property(e => e.PostId)
////                .IsUnicode(false);

////            //modelBuilder.Entity<Employee>()
////            //    .HasMany(e => e.AcceptDocument)
////            //    .WithRequired(e => e.Employee)
////            //    .WillCascadeOnDelete(false);

////            //modelBuilder.Entity<Employee>()
////            //    .HasMany(e => e.ContractToEmployee)
////            //    .WithRequired(e => e.Employee)
////            //    .WillCascadeOnDelete(false);

////            modelBuilder.Entity<AcceptDocument>()
////                .HasOne(e => e.Employee)
////                .WithMany(e => e.AcceptDocument)
////                .HasForeignKey(e => e.PersonalNumberId)
////                .OnDelete(DeleteBehavior.Restrict);  

////            modelBuilder.Entity<ContractToEmployee>()
////                .HasOne(e => e.Employee)
////                .WithMany(e => e.ContractToEmployee)
////                .HasForeignKey(e => e.PersonalNumberId)
////                .OnDelete(DeleteBehavior.Restrict);   


////            modelBuilder.Entity<ExecutingServices>()
////                .Property(e => e.Price)
////                .HasPrecision(19, 4);

////            //modelBuilder.Entity<ExecutingServices>()
////            //    .HasMany(e => e.UsingPartMaterial)
////            //    .WithRequired(e => e.ExecutingServices)
////            //    .HasForeignKey(e => new { ServiceId = e.ServiceId, OrderId = e.OrderId })
////            //    .WillCascadeOnDelete(false);

////            //modelBuilder.Entity<ExecutingServices>()
////            //    .HasMany(e => e.UsingCustomSPartMat)
////            //    .WithRequired(e => e.ExecutingServices)
////            //    .HasForeignKey(e => new { ServiceId = e.ServiceId, OrderId = e.OrderId })
////            //    .WillCascadeOnDelete(false);

////            modelBuilder.Entity<ExecutingServices>()
////                .HasMany(e => e.UsingPartMaterial)
////                .WithOne(e => e.ExecutingServices)
////                //.HasForeignKey(e => e.ServiceId)
////                .HasForeignKey(e => new { e.ServiceId, e.OrderId })
////                .OnDelete(DeleteBehavior.Restrict);

////            modelBuilder.Entity<ExecutingServices>()
////                .HasMany(e => e.UsingCustomSPartMat)
////                .WithOne(e => e.ExecutingServices)
////                //.HasForeignKey(e => e.ServiceId)
////                .HasForeignKey(e => new { e.ServiceId, e.OrderId })
////                .OnDelete(DeleteBehavior.Restrict);




////            modelBuilder.Entity<OrderService>()
////                .Property(e => e.ClientId)
////                .IsUnicode(false);

////            //modelBuilder.Entity<OrderService>()
////            //    .HasMany(e => e.ExecutingServices)
////            //    .WithRequired(e => e.OrderServ)
////            //    .WillCascadeOnDelete(false);

////            //modelBuilder.Entity<OrderService>()
////            //    .HasMany(e => e.RemarkToStateCar)
////            //    .WithRequired(e => e.OrderServ)
////            //    .WillCascadeOnDelete(false);

////            modelBuilder.Entity<OrderService>()
////                .HasMany(e => e.ExecutingServices)
////                .WithOne(e => e.OrderService)
////                .HasForeignKey(e => e.OrderId)
////                .OnDelete(DeleteBehavior.Restrict); 

////            modelBuilder.Entity<OrderService>()
////                .HasMany(e => e.RemarkToStateCar)
////                .WithOne(e => e.OrderService)
////                .HasForeignKey(e => e.OrderId)
////                .OnDelete(DeleteBehavior.Restrict);  


////            modelBuilder.Entity<Post>()
////                .Property(e => e.PostId)
////                .IsUnicode(false);

////            //modelBuilder.Entity<Post>()
////            //    .HasMany(e => e.ServiceType)
////            //    .WithMany(e => e.Post)
////            //    .Map(m => m.ToTable("ServiceType_PostEmployee").MapLeftKey("PostId").MapRightKey("TypeId"));

////            //modelBuilder.Entity<Provider>()
////            //    .HasMany(e => e.Invoice)
////            //    .WithRequired(e => e.Provider)
////            //    .WillCascadeOnDelete(false);


////            modelBuilder.Entity<Post>()
////                .HasMany(e => e.ServiceType)
////                .WithMany(e => e.Post)
////                .UsingEntity(j => j.ToTable("ServiceType_PostEmployee"));

////            modelBuilder.Entity<Provider>()
////                .HasMany(e => e.Invoice)
////                .WithOne(e => e.Provider)
////                .HasForeignKey(e => e.ProviderId)
////                .OnDelete(DeleteBehavior.Restrict); 


////            modelBuilder.Entity<entity.Service>()
////                .Property(e => e.PriceHour)
////                .HasPrecision(19, 4);

////            //modelBuilder.Entity<entity.Service>()
////            //    .HasMany(e => e.ExecutingServices)
////            //    .WithRequired(e => e.Service)
////            //    .WillCascadeOnDelete(false);

////            //modelBuilder.Entity<entity.Service>()
////            //    .HasMany(e => e.PreRecordServices)
////            //    .WithRequired(e => e.Service)
////            //    .WillCascadeOnDelete(false);


////            modelBuilder.Entity<entity.Service>()
////                .HasMany(e => e.ExecutingServices)
////                .WithOne(e => e.Service)
////                .HasForeignKey(e => e.ServiceId)
////                .OnDelete(DeleteBehavior.Restrict);

////            modelBuilder.Entity<entity.Service>()
////                .HasMany(e => e.PreRecordServices)
////                .WithOne(e => e.Service)
////                .HasForeignKey(e => e.ServiceId)
////                .OnDelete(DeleteBehavior.Restrict);


////            modelBuilder.Entity<SparePartMaterial>()
////                .Property(e => e.Name)
////                .IsUnicode(false);

////            modelBuilder.Entity<SparePartMaterial>()
////                .Property(e => e.Unit)
////                .IsUnicode(false);

////            //modelBuilder.Entity<SparePartMaterial>()
////            //    .HasMany(e => e.AcceptanceInvoice)
////            //    .WithRequired(e => e.SparePartMaterial)
////            //    .WillCascadeOnDelete(false);

////            //modelBuilder.Entity<SparePartMaterial>()
////            //    .HasMany(e => e.AcceptanceCustomSParts)
////            //    .WithRequired(e => e.SparePartMaterial)
////            //    .WillCascadeOnDelete(false);

////            modelBuilder.Entity<SparePartMaterial>()
////                .HasMany(e => e.AcceptanceInvoice)
////                .WithOne(e => e.SparePartMaterial)
////                .HasForeignKey(e => e.PartId)
////                .OnDelete(DeleteBehavior.Restrict);

////            modelBuilder.Entity<SparePartMaterial>()
////                .HasMany(e => e.AcceptanceCustomSParts)
////                .WithOne(e => e.SparePartMaterial)
////                .HasForeignKey(e => e.PartId)
////                .OnDelete(DeleteBehavior.Restrict);


////            modelBuilder.Entity<UsingPartMaterial>()
////                .Property(e => e.CostPart)
////                .HasPrecision(19, 4);

////            base.OnModelCreating(modelBuilder);
////        }
////    }
////}