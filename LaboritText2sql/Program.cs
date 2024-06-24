using LaboritText2sql.Service;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(new TextToSqlService("sk-proj-3V9x8C633b019lqoFwIxT3BlbkFJHd4ss2IeezaDJAY2kGcj"));


builder.Services.AddSingleton(new OpenAIService("sk-proj-3V9x8C633b019lqoFwIxT3BlbkFJHd4ss2IeezaDJAY2kGcj"));

builder.Services.AddTransient<IDbConnection>((sp) => new MySqlConnection(builder.Configuration.GetConnectionString("NorthwindDatabase")));


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
