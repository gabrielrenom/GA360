using Duende.Bff;
using Duende.Bff.Yarp;
using GA360.DAL.Infrastructure.Contexts;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.DAL.Infrastructure.Repositories;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CRMDbContext>(options =>
options
.UseSqlServer(builder.Configuration.GetConnectionString("CRM")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddControllers();

builder.Services.AddScoped<DbContext, CRMDbContext>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddBff(x => {
    x.AntiForgeryHeaderValue = "Dog";
})
.AddRemoteApis();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";
}).AddCookie("cookie", options =>
{
    options.Cookie.Name = "__Host-bff";
    options.Cookie.SameSite = SameSiteMode.Strict;
}).AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://www.auth.signos.io";
    //options.Authority = "https://demo.duendesoftware.com";
    //options.ClientId = "interactive";
    options.ClientId = "interactive.bff.lms.portal";
    //options.ClientId = "IgnacioTest2";
    options.ClientSecret = "J6atmybilSHWwL9RRLakEA==";
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

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
app.MapBffManagementEndpoints();

app.MapControllers();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapRemoteBffApiEndpoint("/customers", "https://localhost:7168/Customers")
//        .RequireAccessToken(TokenType.User);
//});

app.UseEndpoints(endpoints =>
{
    endpoints.MapRemoteBffApiEndpoint("/menu", "https://localhost:7030/menu").AllowAnonymous();
        //.RequireAccessToken(TokenType.User);
});
app.MapFallbackToFile("/index.html");

app.Run();
    