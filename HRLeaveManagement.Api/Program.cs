using HRLeaveManagement.Application;
using HRLeaveManagement.Infrastructure;
using HRLeaveManagement.Presistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
var configuration = builder.Configuration;
builder.Services.AddPersistenceServices(configuration);
builder.Services.AddInfrastructureServices(configuration);


builder.Services.AddControllers();
//Allowing cross origin requests from any origin, any header and any method.
//This is useful for development and testing purposes,
//but should be restricted in production environments for security reasons.
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("all", builder =>
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
    }
    );

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
