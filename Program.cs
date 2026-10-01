using GPACARICOMAPI.Configuration;
using GPACARICOMAPI.Models;
using GPACARICOMAPI.Repositories;
using GPACARICOMAPI.Repositories.Interface;
using GPACARICOMAPI.Services;
using GPACARICOMAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddScoped<IArticleRepository,
                           ArticleRepository>();
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
builder.Services.AddScoped<IWorkAndTravelRepository, WorkAndTravelRepository>();
builder.Services.AddScoped<IWorkAndTravelService, WorkAndTravelService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVerificationService, VerificationService>();
builder.Services.AddScoped<ITestimonialRepository, TestimonialRepository>();
builder.Services.AddScoped<IConnectionFactory,
                           ConnectionFactory>();
builder.Services.Configure<FileStorageOptions>(
    builder.Configuration.GetSection("FileStorage"));

builder.Services.AddScoped<
    IFileStorageService,
    FileStorageService>();

builder.Services.AddScoped<
    IWATDocumentRepository,
    WATDocumentRepository>();

builder.Services.AddScoped<
    IWATDocumentService,
    WATDocumentService>();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); // Required for minimal APIs / mapping
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
    options.AddPolicy("ProdCors", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("https://myProductionSite.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;
SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(

        tokenKeyString != null ? tokenKeyString : ""
        )
    );
TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
{
    IssuerSigningKey = tokenKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = false,
    ValidateAudience = false,
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();   // Serves the generated JSON payload
    app.UseSwaggerUI();

    app.UseCors("DevCors");
}
else {
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}
app.UseSwagger();   // Serves the generated JSON payload
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
