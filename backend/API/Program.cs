using API.Data;
using API.dto;
using API.Model;
using API.PasswordHelper;
using Azure;
using backend.API.dto;
using backend.API.FieldValidator;
using backend.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<PasswordValidator>();
builder.Services.AddScoped(typeof(UserService<>));
builder.Services.AddScoped<PasswordLogic>();

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

    ApiResponse response = new();

    var patients = await db.Patients.ToListAsync();

    response.Result = patients.Select(p => p.toDto()).ToList();
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);
 
}).WithName("GetAllPatients").Produces<ApiResponse>(200);

app.MapGet("/Doctors", async (AppDbContext db) =>
{
    ApiResponse response = new();

    var doctors = await db.Doctors.ToListAsync();

    response.Result = doctors.Select(d => d.toDto()).ToList();
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);
}).WithName("GetAllDoctors").Produces<ApiResponse>(200);

//get all appointments (Need to create an AppointmentDto)
app.MapGet("/appointments", async (AppDbContext db) => {

    ApiResponse response = new();
    var appoinments = await db.Appointments.ToListAsync();

    response.Result = appoinments.Select(a => a.toDto()).ToList();
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);

    }).WithName("GetAllAppointments").Produces<ApiResponse>(200); ;


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
    ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = new List<string>() }; 

    var p =  await db.Patients.FirstOrDefaultAsync(p => p.Id == dto.PatientID);
    var d = await db.Doctors.FirstOrDefaultAsync(d => d.Id == dto.DoctorID);


    if (p == null || d == null) {

        response.ErrorMessages.Add("Doctor or patient not found");

        return Results.NotFound(response);
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

    response.Result = appointment.toDto();
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);

}).WithName("CreateAppointment").Accepts<AppointmentDto>("json/application").Produces<ApiResponse>(201).Produces(404);

app.MapPost("/Doctor/", async (AppDbContext db, CreateDoctorDto dto) =>
{
    ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() }; 


    var doctor = dto.toDoctor();

    if (doctor == null)
    {
        response.ErrorMessages.Add("Could not create the Doctor object!");
        return Results.BadRequest(response);
    }

    db.Doctors.Add(doctor);
    await db.SaveChangesAsync();

    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    

    return Results.Ok(response);

}).WithName("CreateDoctor").Accepts<CreateDoctorDto>("application(json").Produces<ApiResponse>(201).Produces(400);


app.MapPost("/JournalEntry", async (AppDbContext db, CreateJournalEntryDto dto) =>
{

    ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = new List<string>() };

    var p = await db.Patients.FirstOrDefaultAsync(p => p.Id == dto.PatientID);
    var d = await db.Doctors.FirstOrDefaultAsync(d => d.Id == dto.DoctorID);

    if (p == null || d == null)
    {
        response.ErrorMessages.Add("Could not find the doctor or the patient!");

        return Results.NotFound(response);
    }

    JournalEntry entry = new JournalEntry
    {
        Notes = dto.Notes,
        DateNTime = dto.DateNTime,
        Patient = p,
        PatientID = dto.PatientID,
        Doctor = d,
        DoctorID = dto.DoctorID

    };

    db.JournalEntries.Add(entry);
    await db.SaveChangesAsync();

    response.Result = entry.toDto();
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    
    return Results.Ok(response);

}).WithName("CreateJournal").Accepts<CreateJournalEntryDto>("application/json").Produces<ApiResponse>(201).Produces(400);




app.MapGet("/JournalEntries/{id:int}", async (AppDbContext db, int id) =>
{
    ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = new List<string>() };
    
    var entry = await db.JournalEntries.FirstOrDefaultAsync(e => e.Id == id);
    
    if (entry == null)
    {
        response.ErrorMessages.Add("Could not find the journal entry");
        return Results.NotFound(response);
    }
     
    response.Result = entry.toDto();
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);

}).WithName("GetJournalByID").Produces<ApiResponse>(200).Produces(404);


app.MapPost("/Doctors{id:int}/password", async (AppDbContext db, int id, UserService<Doctor> docService, string password, PasswordValidator passwordValidator) =>
{
    ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

    //validate password
    if (!passwordValidator.PasswordValid(password))
    {
        response.ErrorMessages.Add("Could not validate the password, please choose a valid password!");
        return Results.BadRequest(response);
    }

    //hash the pass 
    response = await docService.SavePasswordToDB(id, password);


    return Results.Ok(response); 

}).WithName("CreatePassWordDoctor").Produces<ApiResponse>(200).Produces(400);

app.MapPost("/Patients{id:int}/password", async (AppDbContext db, int id, UserService<Patient> patientService, string password, PasswordValidator passwordValidator, ILogger<Program> _logger) =>
{
    ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

    _logger.Log(LogLevel.Information, "testing line");

    if (!passwordValidator.PasswordValid(password))
    {
        _logger.Log(LogLevel.Information, "testing second point");
        response.ErrorMessages.Add("Could not validate the password, please choose a valid password!");
        return Results.BadRequest(response);
    }
    
    response = await patientService.SavePasswordToDB(id, password);


    return Results.Ok(response);

}).WithName("CreatePassWordPatient").Produces<ApiResponse>(200).Produces(400);


 

app.Run();

