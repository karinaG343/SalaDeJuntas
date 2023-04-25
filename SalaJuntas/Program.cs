using Microsoft.EntityFrameworkCore;
using SalaJuntas.Data;

var builder = WebApplication.CreateBuilder(args);
string cors = "ConfigurarCors";

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["sqlconnection:connectionString"]); });

builder.Services.AddCors(options =>
{
    options.AddPolicy(cors, policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddControllersWithViews();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(cors);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
