using Eshop.DataAccess.Context;
using Eshop.DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Redis connection
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
	var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
	return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddDbContext<EshopDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

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
