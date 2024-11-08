using CarServiceApp.AuthManagement.Enums;
using CarServiceApp.AuthManagement.Service;
using CarServiceApp.AuthManagement.Service.Abstract;
using CarServiceApp.Domain.Config;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Extension;
using CarServiceApp.Domain.Repository;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service;
using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Domain.Service.Common;
using CarServiceApp.PdfConverter;
using CarServiceApp.PdfConverter.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ConnectionStrings"));


//builder.Services.AddCookieSettings();
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//   .AddCookie();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Cookie.HttpOnly = true;
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//        options.LoginPath = "/Account/Login"; // Путь к странице входа
//        options.AccessDeniedPath = "/Account/AccessDenied"; // Путь к странице доступа запрещен
//        options.SlidingExpiration = true;
//    }).AddIdentityCookies();

//builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
//    .AddIdentityCookies();

////Добавление служб аутентификации
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.RequireHttpsMetadata = false;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = "https://localhost:44368/",// Configuration["Jwt:Issuer"],
//        ValidAudience = "https://localhost:44368/", //Configuration["Jwt:Issuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key-here")) //Configuration["Jwt:Key"]
//    };
//})
////.AddIdentityCookies()
//;


// Добавление служб авторизации
//builder.Services.AddAuthorization();

//builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(typeof(IGenericRepositoryAsync<,>), typeof(GenericRepositoryAsync<,>));
builder.Services.AddScoped(typeof(IGenericServiceAsync<,>), typeof(GenericServiceAsync<,>));
builder.Services.AddScoped(typeof(IWorkLoadingRepository), typeof(WorkLoadingRepository));
builder.Services.AddScoped(typeof(IWorkLoadingService), typeof(WorkLoadingService));
builder.Services.AddScoped(typeof(IReportRepository), typeof(ReportRepository));
builder.Services.AddScoped(typeof(ICommonService), typeof(CommonService));
builder.Services.AddScoped(typeof(IDiagramService), typeof(DiagramService));
builder.Services.AddScoped(typeof(IReportService), typeof(ReportService));
builder.Services.AddScoped(typeof(IHtmlToPdfHelper), typeof(HtmlToPdfHelper));
builder.Services.AddScoped(typeof(ICarRepository), typeof(CarRepository));
builder.Services.AddScoped(typeof(ICarService), typeof(CarService));
builder.Services.AddScoped(typeof(IPreRecordService), typeof(PreRecordService));
//builder.Services.AddScoped(typeof(IPreRecordRepository), typeof(PreRecordRepository));
builder.Services.AddScoped(typeof(IClientService), typeof(ClientService));
//builder.Services.AddScoped(typeof(IClientRepository), typeof(ClientRepository));
builder.Services.AddScoped(typeof(IContractService), typeof(ContractService));
//builder.Services.AddScoped(typeof(IContractRepository), typeof(ContractRepository));
builder.Services.AddScoped(typeof(IEmployeeService), typeof(EmployeeService));
//builder.Services.AddScoped(typeof(IEmployeeRepository), typeof(EmployeeRepository));
builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
builder.Services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
builder.Services.AddScoped(typeof(IDbConnectionLayer), typeof(DbConnectionLayer));
builder.Services.AddScoped(typeof(IProviderService), typeof(ProviderService));
//builder.Services.AddScoped(typeof(IProviderRepository), typeof(ProvidertRepository));
builder.Services.AddScoped(typeof(ISparePartMaterialService), typeof(SparePartMaterialService));
builder.Services.AddScoped(typeof(IEmployeeService), typeof(EmployeeService));
builder.Services.AddScoped(typeof(IInvoiceService), typeof(InvoiceService));
builder.Services.AddScoped(typeof(IUsingPartMaterialService), typeof(UsingPartMaterialService));
builder.Services.AddScoped(typeof(IAcceptanceInvoiceService), typeof(AcceptanceInvoiceService));
builder.Services.AddScoped(typeof(IAcceptanceDocumentService), typeof(AcceptanceDocumentService));

//
// If using Kestrel:
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// If using IIS:
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});


builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateAsyncScope())
{
    var roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = EnumExtensions.GetEnumNamesAndValues<UserRoles>();

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}


using (var scope = app.Services.CreateAsyncScope())
{
    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    var emailTemplate = "_test@gmail.com";
    var roles = EnumExtensions.GetEnumNamesAndValues<UserRoles>();

    foreach (var role in roles)
    {
        if (await userManager.FindByEmailAsync($"{role}{emailTemplate}") == null)
        {
            Random random = new();
            var phone =  random.Next(1111111, 10000000);

            var identityUser = new IdentityUser
            {
                Email = $"{role}{emailTemplate}",
                EmailConfirmed = true,
                UserName = role,
                PhoneNumber = $"+375-29-{phone}",
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(identityUser, $"{role}_123");

            if (result.Succeeded)
                await userManager.AddToRoleAsync(identityUser, role);
        }
    }
}


app.Run();
