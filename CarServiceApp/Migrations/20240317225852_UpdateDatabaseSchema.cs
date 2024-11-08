using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarServiceApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarBrand",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    ViewBrand = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CarBrands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "CarColor",
                columns: table => new
                {
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    Hex = table.Column<string>(type: "nchar(7)", fixedLength: true, maxLength: 7, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsMetallic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CarColor_table", x => x.ColorId);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    NumberDriveLicense = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExpirationDateLicense = table.Column<DateOnly>(type: "date", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DateBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKClient", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    ChiefPersonalNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKDepartment", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedClient",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GeneratedClients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "ImportedStreet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedStreet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamePost = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKPost", x => x.PostId);
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    ProviderId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ZipCode = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CertificateNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKProvider", x => x.ProviderId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceType",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKServiceType", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "SparePartMaterial",
                columns: table => new
                {
                    PartId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Unit = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Manufacture = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKSparePartMaterial", x => x.PartId);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfPayment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    PaymentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKTypeOfPayment", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarModel",
                columns: table => new
                {
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CarModels", x => x.ModelId);
                    table.ForeignKey(
                        name: "FK_CarModels_CarBrands",
                        column: x => x.BrandId,
                        principalTable: "CarBrand",
                        principalColumn: "BrandId");
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    PersonnelNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResidentialAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DateBirthday = table.Column<DateOnly>(type: "date", nullable: true),
                    PassportPrivateNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PassportSeries = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    PassportNumber = table.Column<int>(type: "int", nullable: true),
                    PassportIssuedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassportIssuedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PassportValidityDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKEmployee", x => x.PersonnelNumber);
                    table.ForeignKey(
                        name: "Belongs",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Has",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ServiceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RateTime = table.Column<double>(type: "float", nullable: true),
                    PriceHour = table.Column<decimal>(type: "money", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKService", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_Service_ServiceType",
                        column: x => x.TypeId,
                        principalTable: "ServiceType",
                        principalColumn: "TypeId");
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypePostEmployee",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypePostEmployee", x => new { x.PostId, x.TypeId });
                    table.ForeignKey(
                        name: "FK_ServiceTypePostEmployee_Post",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "PostId");
                    table.ForeignKey(
                        name: "FK_ServiceTypePostEmployee_ServiceType",
                        column: x => x.TypeId,
                        principalTable: "ServiceType",
                        principalColumn: "TypeId");
                });

            migrationBuilder.CreateTable(
                name: "CarGeneration",
                columns: table => new
                {
                    GenerationId = table.Column<int>(type: "int", nullable: false),
                    GenerationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    YearBegin = table.Column<int>(type: "int", nullable: true, defaultValueSql: "(NULL)"),
                    YearEnd = table.Column<int>(type: "int", nullable: true, defaultValueSql: "(NULL)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___importe__F887D65A7C2A7AF2", x => x.GenerationId);
                    table.ForeignKey(
                        name: "FK__CarGeneration__CarModels",
                        column: x => x.ModelId,
                        principalTable: "CarModel",
                        principalColumn: "ModelId");
                });

            migrationBuilder.CreateTable(
                name: "AcceptDocument",
                columns: table => new
                {
                    AcceptanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonnelNumber = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    AcceptDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcceptDocument", x => x.AcceptanceId);
                    table.ForeignKey(
                        name: "Accept",
                        column: x => x.PersonnelNumber,
                        principalTable: "Employee",
                        principalColumn: "PersonnelNumber");
                    table.ForeignKey(
                        name: "BringParts",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId");
                });

            migrationBuilder.CreateTable(
                name: "ContractToEmployee",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonnelNumber = table.Column<int>(type: "int", nullable: false),
                    RecruitDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DismissDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Type = table.Column<string>(type: "nchar(40)", fixedLength: true, maxLength: 40, nullable: true),
                    Term = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractToEmployee", x => x.ContractId);
                    table.ForeignKey(
                        name: "Worked_by",
                        column: x => x.PersonnelNumber,
                        principalTable: "Employee",
                        principalColumn: "PersonnelNumber");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    LotNumber = table.Column<int>(type: "int", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DeliveryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PersonnelNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.LotNumber);
                    table.ForeignKey(
                        name: "Deliver",
                        column: x => x.ProviderId,
                        principalTable: "Provider",
                        principalColumn: "ProviderId");
                    table.ForeignKey(
                        name: "Take_in",
                        column: x => x.PersonnelNumber,
                        principalTable: "Employee",
                        principalColumn: "PersonnelNumber");
                });

            migrationBuilder.CreateTable(
                name: "PreRecord",
                columns: table => new
                {
                    RecordId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    MarkModelCar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IssueYear = table.Column<int>(type: "int", nullable: false),
                    DateMakingRecord = table.Column<DateTime>(type: "datetime", nullable: false),
                    PersonnelNumber = table.Column<int>(type: "int", nullable: false),
                    RegNumberCar = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsRejection = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRecord", x => x.RecordId);
                    table.ForeignKey(
                        name: "Record",
                        column: x => x.PersonnelNumber,
                        principalTable: "Employee",
                        principalColumn: "PersonnelNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarSeries",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    SeriesName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    GenerationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CarSeries", x => x.SeriesId);
                    table.ForeignKey(
                        name: "FK__CarSeries__CarGeneration",
                        column: x => x.GenerationId,
                        principalTable: "CarGeneration",
                        principalColumn: "GenerationId");
                    table.ForeignKey(
                        name: "FK__CarSeries__CarModels",
                        column: x => x.ModelId,
                        principalTable: "CarModel",
                        principalColumn: "ModelId");
                });

            migrationBuilder.CreateTable(
                name: "AcceptanceCustomSpart",
                columns: table => new
                {
                    PartId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    AcceptanceId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<double>(type: "float", nullable: true),
                    StateSpart = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKAcceptanceCustomSParts", x => new { x.PartId, x.AcceptanceId });
                    table.ForeignKey(
                        name: "Fixed",
                        column: x => x.PartId,
                        principalTable: "SparePartMaterial",
                        principalColumn: "PartId");
                    table.ForeignKey(
                        name: "Incl",
                        column: x => x.AcceptanceId,
                        principalTable: "AcceptDocument",
                        principalColumn: "AcceptanceId");
                });

            migrationBuilder.CreateTable(
                name: "AcceptanceInvoice",
                columns: table => new
                {
                    PositionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<double>(type: "float", nullable: true),
                    Price = table.Column<decimal>(type: "money", nullable: true),
                    LotNumber = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    TradeIncrease = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKInvoice", x => x.PositionId);
                    table.ForeignKey(
                        name: "Fixed_in",
                        column: x => x.PartId,
                        principalTable: "SparePartMaterial",
                        principalColumn: "PartId");
                    table.ForeignKey(
                        name: "IncludePart",
                        column: x => x.LotNumber,
                        principalTable: "Invoice",
                        principalColumn: "LotNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreRecordService",
                columns: table => new
                {
                    RecordId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PersonnelNumber = table.Column<int>(type: "int", nullable: true),
                    DateReservation = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRecordService", x => new { x.RecordId, x.ServiceId });
                    table.ForeignKey(
                        name: "BelongsPreRecord",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId");
                    table.ForeignKey(
                        name: "HasServ",
                        column: x => x.RecordId,
                        principalTable: "PreRecord",
                        principalColumn: "RecordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PreExec",
                        column: x => x.PersonnelNumber,
                        principalTable: "Employee",
                        principalColumn: "PersonnelNumber");
                });

            migrationBuilder.CreateTable(
                name: "CarModification",
                columns: table => new
                {
                    ModificationId = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    ModificationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartProductionYear = table.Column<int>(type: "int", nullable: true),
                    EndProductionYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CarModification", x => x.ModificationId);
                    table.ForeignKey(
                        name: "FK__CarModification__CarSeries",
                        column: x => x.SeriesId,
                        principalTable: "CarSeries",
                        principalColumn: "SeriesId");
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    VinNumber = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IssueYear = table.Column<int>(type: "int", nullable: true),
                    OwnerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DataSheetCar = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TransmissionType = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    ViewCar = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ModificationId = table.Column<int>(type: "int", nullable: true),
                    ColorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKCar", x => x.VinNumber);
                    table.ForeignKey(
                        name: "FK_Car__CarColor",
                        column: x => x.ColorId,
                        principalTable: "CarColor",
                        principalColumn: "ColorId");
                    table.ForeignKey(
                        name: "FK_Car__CarModification",
                        column: x => x.ModificationId,
                        principalTable: "CarModification",
                        principalColumn: "ModificationId");
                });

            migrationBuilder.CreateTable(
                name: "OrderService",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateMakingOrder = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateCompleting = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateFactCompleting = table.Column<DateTime>(type: "datetime", nullable: true),
                    ClientId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentMileageCar = table.Column<int>(type: "int", nullable: true),
                    VinNumber = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NumberWheelCaps = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: (byte)0),
                    NumberWipers = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: (byte)0),
                    NumberWipersArms = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: (byte)0),
                    IsAntenna = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsSpareWheel = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsCoverDecorEngine = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    FuelLevelPercent = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)50),
                    IsTuner = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKOrderServ", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_OrderServ_Car",
                        column: x => x.VinNumber,
                        principalTable: "Car",
                        principalColumn: "VinNumber");
                    table.ForeignKey(
                        name: "FK_OrderService_Client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_OrderService_TypeOfPayment",
                        column: x => x.PaymentId,
                        principalTable: "TypeOfPayment",
                        principalColumn: "PaymentId");
                });

            migrationBuilder.CreateTable(
                name: "ExecutingService",
                columns: table => new
                {
                    ServiceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateCompleting = table.Column<DateTime>(type: "datetime", nullable: true),
                    TakeTime = table.Column<double>(type: "float", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "money", nullable: true),
                    PersonnelNumber = table.Column<int>(type: "int", nullable: true),
                    TaxAddedValue = table.Column<int>(type: "int", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("XPKExecutingServices", x => new { x.ServiceId, x.OrderId });
                    table.ForeignKey(
                        name: "Contents_in",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId");
                    table.ForeignKey(
                        name: "Executes",
                        column: x => x.PersonnelNumber,
                        principalTable: "Employee",
                        principalColumn: "PersonnelNumber");
                    table.ForeignKey(
                        name: "Registr_in",
                        column: x => x.OrderId,
                        principalTable: "OrderService",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "RemarkToStateCar",
                columns: table => new
                {
                    RemarkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    XAxisPos = table.Column<int>(type: "int", nullable: false),
                    YAxisPos = table.Column<int>(type: "int", nullable: false),
                    RemarkText = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NumberType = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemarkToStateCar", x => x.RemarkId);
                    table.ForeignKey(
                        name: "FK_RemarkToStateCar_OrderServ",
                        column: x => x.OrderId,
                        principalTable: "OrderService",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "UsingCustomSpartMat",
                columns: table => new
                {
                    AcceptanceId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Number = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsingCustomSpartMat", x => new { x.AcceptanceId, x.PartId, x.ServiceId, x.OrderId });
                    table.ForeignKey(
                        name: "Apply_to2",
                        columns: x => new { x.ServiceId, x.OrderId },
                        principalTable: "ExecutingService",
                        principalColumns: new[] { "ServiceId", "OrderId" });
                    table.ForeignKey(
                        name: "Use_in2",
                        columns: x => new { x.PartId, x.AcceptanceId },
                        principalTable: "AcceptanceCustomSpart",
                        principalColumns: new[] { "PartId", "AcceptanceId" });
                });

            migrationBuilder.CreateTable(
                name: "UsingPartMaterial",
                columns: table => new
                {
                    PositionId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Number = table.Column<double>(type: "float", nullable: true),
                    CostPart = table.Column<decimal>(type: "money", nullable: true),
                    PersonnelNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsingPartMaterial", x => new { x.PositionId, x.ServiceId, x.OrderId });
                    table.ForeignKey(
                        name: "Accept_to_Work",
                        column: x => x.PersonnelNumber,
                        principalTable: "Employee",
                        principalColumn: "PersonnelNumber");
                    table.ForeignKey(
                        name: "Apply_to",
                        columns: x => new { x.ServiceId, x.OrderId },
                        principalTable: "ExecutingService",
                        principalColumns: new[] { "ServiceId", "OrderId" });
                    table.ForeignKey(
                        name: "Use_parts",
                        column: x => x.PositionId,
                        principalTable: "AcceptanceInvoice",
                        principalColumn: "PositionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceCustomSpart_AcceptanceId",
                table: "AcceptanceCustomSpart",
                column: "AcceptanceId");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceInvoice_LotNumber",
                table: "AcceptanceInvoice",
                column: "LotNumber");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceInvoice_PartId",
                table: "AcceptanceInvoice",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptDocument_ClientId",
                table: "AcceptDocument",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptDocument_PersonnelNumber",
                table: "AcceptDocument",
                column: "PersonnelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Car_ColorId",
                table: "Car",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_ModificationId",
                table: "Car",
                column: "ModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_CarGeneration_ModelId",
                table: "CarGeneration",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CarModel_BrandId",
                table: "CarModel",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_CarModification_SeriesId",
                table: "CarModification",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CarSeries_GenerationId",
                table: "CarSeries",
                column: "GenerationId");

            migrationBuilder.CreateIndex(
                name: "IX_CarSeries_ModelId",
                table: "CarSeries",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractToEmployee_PersonnelNumber",
                table: "ContractToEmployee",
                column: "PersonnelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PostId",
                table: "Employee",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutingService_OrderId",
                table: "ExecutingService",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutingService_PersonnelNumber",
                table: "ExecutingService",
                column: "PersonnelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_PersonnelNumber",
                table: "Invoice",
                column: "PersonnelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ProviderId",
                table: "Invoice",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderService_ClientId",
                table: "OrderService",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderService_PaymentId",
                table: "OrderService",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderService_VinNumber",
                table: "OrderService",
                column: "VinNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PreRecord_PersonnelNumber",
                table: "PreRecord",
                column: "PersonnelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PreRecordService_PersonnelNumber",
                table: "PreRecordService",
                column: "PersonnelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PreRecordService_ServiceId",
                table: "PreRecordService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RemarkToStateCar_OrderId",
                table: "RemarkToStateCar",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_TypeId",
                table: "Service",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypePostEmployee_TypeId",
                table: "ServiceTypePostEmployee",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsingCustomSpartMat_PartId_AcceptanceId",
                table: "UsingCustomSpartMat",
                columns: new[] { "PartId", "AcceptanceId" });

            migrationBuilder.CreateIndex(
                name: "IX_UsingCustomSpartMat_ServiceId_OrderId",
                table: "UsingCustomSpartMat",
                columns: new[] { "ServiceId", "OrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_UsingPartMaterial_PersonnelNumber",
                table: "UsingPartMaterial",
                column: "PersonnelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_UsingPartMaterial_ServiceId_OrderId",
                table: "UsingPartMaterial",
                columns: new[] { "ServiceId", "OrderId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ContractToEmployee");

            migrationBuilder.DropTable(
                name: "GeneratedClient");

            migrationBuilder.DropTable(
                name: "ImportedStreet");

            migrationBuilder.DropTable(
                name: "PreRecordService");

            migrationBuilder.DropTable(
                name: "RemarkToStateCar");

            migrationBuilder.DropTable(
                name: "ServiceTypePostEmployee");

            migrationBuilder.DropTable(
                name: "UsingCustomSpartMat");

            migrationBuilder.DropTable(
                name: "UsingPartMaterial");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PreRecord");

            migrationBuilder.DropTable(
                name: "AcceptanceCustomSpart");

            migrationBuilder.DropTable(
                name: "ExecutingService");

            migrationBuilder.DropTable(
                name: "AcceptanceInvoice");

            migrationBuilder.DropTable(
                name: "AcceptDocument");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "OrderService");

            migrationBuilder.DropTable(
                name: "SparePartMaterial");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "ServiceType");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "TypeOfPayment");

            migrationBuilder.DropTable(
                name: "Provider");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "CarColor");

            migrationBuilder.DropTable(
                name: "CarModification");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "CarSeries");

            migrationBuilder.DropTable(
                name: "CarGeneration");

            migrationBuilder.DropTable(
                name: "CarModel");

            migrationBuilder.DropTable(
                name: "CarBrand");
        }
    }
}
