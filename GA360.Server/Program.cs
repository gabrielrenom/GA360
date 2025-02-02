using Duende.Bff;
using Duende.Bff.Yarp;
using GA360.Commons.Settings;
using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.DAL.Infrastructure.Repositories;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Services;
using GA360.Server.Middlewares;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IdentitySettings>(builder.Configuration.GetSection("IdentitySettings")); 

// Add Authentication services
var identitySettings = new IdentitySettings();
builder.Configuration.GetSection("IdentitySettings").Bind(identitySettings);

// Add memory cache services
builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services.AddDbContext<CRMDbContext>(options =>
options
.UseSqlServer(builder.Configuration.GetConnectionString("CRM"))
.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddControllers();

builder.Services.AddScoped<DbContext, CRMDbContext>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITrainingCentreService, TrainingCentreService>();
builder.Services.AddScoped<ITrainingCentreRepository, TrainingCentreRepository>();
builder.Services.AddScoped<IEthnicityService, EthnicityService>();
builder.Services.AddScoped<IEthnicityRepository, EthnicityRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IAuditTrailService, AuditTrailService>();
builder.Services.Configure<BlobStorageSettings>(builder.Configuration.GetSection("BlobStorageSettings"));

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IQualificationService, QualificationService>();
builder.Services.AddScoped<IQualificationRepository, QualificationRepository>();

builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddScoped<IPermissionService, PermissionService>();
//builder.Services.AddSession(options =>
//{
//    options.Cookie.Name = "__Host-bff";
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//});
builder.Services.AddBff(x =>
{
    x.AntiForgeryHeaderValue = "Dog";
    //x.RequireLogoutSessionId = false;
    //x.BackchannelLogoutAllUserSessions = true;
})
.AddRemoteApis();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1048576000; // Set the limit to 1 GB or desired size
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1048576000; // Set the limit to 1 GB or desired size
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";
})
    .AddCookie("cookie")
//    .AddCookie("cookie", options =>
//{
//    options.Cookie.Name = "__Host-bff";
//    options.Cookie.SameSite = SameSiteMode.Strict;
//    options.Cookie.HttpOnly = false;
//})
    .AddOpenIdConnect("oidc", options =>
{
    options.Authority = identitySettings.Authority;
    options.ClientId = identitySettings.ClientId;
    options.ClientSecret = identitySettings.ClientSecret;
    options.ResponseType = "code";
    options.ResponseMode = "query";

    options.GetClaimsFromUserInfoEndpoint = true;
    options.MapInboundClaims = false;
    options.SaveTokens = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("api");
    options.Scope.Add("offline_access");

    options.TokenValidationParameters = new()
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});
builder.Services.AddSession();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

//app.UseMiddleware<CsrfMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseBff();
app.UseAuthorization();
app.UseSession();
app.MapBffManagementEndpoints();

app.MapControllers();
//app.MapControllers()
//    .RequireAuthorization()
//    .AsBffApiEndpoint();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapRemoteBffApiEndpoint("/customers", "https://localhost:7168/Customers")
//        .RequireAccessToken(TokenType.User);
//});

app.UseEndpoints(endpoints =>
{
    //endpoints.MapRemoteBffApiEndpoint("/menu", "https://localhost:7030/menu").AllowAnonymous();
    //.RequireAccessToken(TokenType.User);
});
app.MapFallbackToFile("/index.html");

app.Run();
