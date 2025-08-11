using API.Data;
using API.dto;
using API.Model;
using API.PasswordHelper;
using Azure;
using backend.API.dto;
using backend.API.Endpoints;
using backend.API.FieldValidator;
using backend.API.Model;
using backend.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.OpenApi.Models;
using Scalar;
using Scalar.AspNetCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<PasswordValidator>();

builder.Services.AddScoped(typeof(IUserService<,>), typeof(UserService<,>));

builder.Services.AddScoped<PasswordLogic>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<JournalEntryService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>   
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hospital API",
        Version = "v1",
        Description = "Khalil sitt prosjekt"
    });
});


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyMethod() 
        .AllowAnyHeader();
    });

});


var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

//Endpoints
app.MapPatientEndpoints(app);
app.MapDoctorEndpoints(app);
app.MapAppointmentEndpoints(app);
app.MapJournalEntryEndpoints(app);

app.Run();

