using aknaIdentityApi.Business.Services;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Domain.Interfaces.Services;
using aknaIdentityApi.Domain.Interfaces.UnitOfWorks;
using aknaIdentityApi.Infrastructure.Contexts;
using aknaIdentityApi.Infrastructure.Repositories;
using aknaIdentityApi.Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://127.0.0.1:3000") // React uygulamanızın çalıştığı portlar
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDeviceInfoRepository, DeviceInfoRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddDbContext<AknaIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");


app.UseAuthorization();

app.MapControllers();

app.Run();
