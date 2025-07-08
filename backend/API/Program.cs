using API.Data;
using API.dto;
using API.Model;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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

//Get ALl patients from Db
app.MapGet("/patients", async (AppDbContext db, ILogger<Program> _logger) => {
    _logger.Log(LogLevel.Information, "Getting all patients...");
    var patients = await db.Patients.ToListAsync();

    return patients.Select(p => p.toDto()).ToList();
 
}).WithName("GetAllPatients").Produces<IEnumerable<ResponsePatientDto>>(200);

app.MapGet("/Doctors", async (AppDbContext db) =>
{
    var doctors = await db.Doctors.ToListAsync();
    return doctors.Select(d => d.toDto()).ToList();
});

//get all appointments (Need to create an AppointmentDto)
app.MapGet("/appointments", async (AppDbContext db) => {
    var appoinments = await db.Appointments.ToListAsync();
    return appoinments.Select(a => a.toDto()).ToList();
    });


//Get patient based on id
app.MapGet("/Patient/{pId:int}", async (AppDbContext db, int pId) =>
{
    var patient = await db.Patients.FirstOrDefaultAsync( p => p.Id == pId);
    if (patient == null)
    {
        return Results.NotFound($"Did not find patient with id: {pId}");
    }
    return Results.Ok(patient.toDto());
}).WithName("GetPatient");

app.MapGet("/Doctor/{dId:int}", async (AppDbContext db, int dId) =>
{
    var doctor = await db.Doctors.FirstOrDefaultAsync(d => d.Id == dId);
    if (doctor == null)
    {
        return Results.NotFound($"Did not find the doctor with id: {dId}");
    }

    return Results.Ok(doctor.toDto());

});


//add a patientDto to DB
app.MapPost("/patients", async (AppDbContext db, CreatePatientDto dto) =>
{
    var patient = dto.ToPatient();

    db.Patients.Add(patient);

    await db.SaveChangesAsync();
    
    return Results.Created($"/patients/{patient.Id}", patient.toDto());
}).WithName("CreatePatient").Produces<ResponsePatientDto>(201).Produces(400);

app.MapPost("/appointment", async (AppDbContext db, AppointmentDto dto) =>
{
    var p =  await db.Patients.FirstOrDefaultAsync(p => p.Id == dto.PatientID);
    var d = await db.Doctors.FirstOrDefaultAsync(d => d.Id == dto.DoctorID);


    if (p == null || d == null) {

        return Results.NotFound("Doctor or patient not found");
    };
    var appointment = new Appointment
   {
       Patient = p,
       Doctor = d, 
       AppointmentDate = dto.Date,
       Status = dto.Status
    };

    db.Appointments.Add(appointment);
    await db.SaveChangesAsync();

    return Results.Created($"/appointment/{appointment.Id}", appointment.toDto());

});

app.MapPost("/Doctor/", async (AppDbContext db, CreateDoctorDto dto) =>
{
    var doctor = dto.toDoctor();
    db.Doctors.Add(doctor);

    await db.SaveChangesAsync();
    return Results.Created($"/Doctor/{doctor.Id}", doctor.toDto());

});

app.Run();

