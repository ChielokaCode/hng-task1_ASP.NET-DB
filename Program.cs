global using hng_task1.Controllers;
global using hng_task1.Model;
global using hng_task1.Service;
global using hng_task1.ServiceImpl;
global using hng_task1.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ensure that appsettings.json is being loaded
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ILogger<UserController>, Logger<UserController>>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IIpService, IpService>();


// Add DbContext configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
Console.WriteLine($"Connection String: {connectionString}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    // options.UseMySQL(connectionString));
    options.UseNpgsql(connectionString));


// Configure the forwarded headers middleware to read the correct client IP address
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable forwarded headers middleware in the request pipeline
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
