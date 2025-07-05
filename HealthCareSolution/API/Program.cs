using API.Data;
using API.dto;
using API.model;
using System.Linq;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseHttpsRedirection();

app.MapGet("/patients", async (AppDbContext db) => { 
    var patients = await db.Patients.ToListAsync();

    return patients.Select(p => p.toDto()).ToList();
 
});

app.MapPost("/patients", async (AppDbContext db, PatientDto dto) =>
{
    var patient = dto.toPatient();

    db.Patients.Add(patient);

    await db.SaveChangesAsync();
    
    
    return Results.Created($"/patients{patient.Id}", patient.toDto());
});

app.MapGet("/appointments", async (AppDbContext db) =>
    await db.Appointments.ToListAsync());


app.Run();

