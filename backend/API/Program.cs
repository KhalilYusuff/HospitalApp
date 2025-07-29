using API.Data;
using API.dto;
using API.Model;
using API.PasswordHelper;
using Azure;
using backend.API.dto;
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

 

app.UseHttpsRedirection();


app.MapGet("/api/all-patients", async ([FromServices]IUserService<Patient, CreatePatientDto> patientService, [FromServices] ILogger<Program> _logger) => {
    _logger.Log(LogLevel.Information, "Getting all patients...");

    var results = await patientService.GetAllUsers();

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results); 
 
}).WithName("GetAllPatients").Produces<ApiResponse>(200);


app.MapGet("/api/all-doctors", async ([FromServices] IUserService<Doctor, CreateDoctorDto> doctorService, [FromServices] ILogger<Program> _logger) => {
    _logger.Log(LogLevel.Information, "Getting all patients...");

    var results = await doctorService.GetAllUsers();

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

}).WithName("GetAllDoctors").Produces<ApiResponse>(200);

//get all appointments (Need to create an AppointmentDto)
app.MapGet("/api/appointments", async ([FromServices] AppointmentService appointmentService) => {

    var results = await appointmentService.GetAllAppointments();

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results); 
 
}).WithName("GetAllAppointments").Produces<ApiResponse>(200); ;


//Get patient based on id
app.MapGet("/api/Patient/{pId:int}", async ([FromServices] IUserService<Patient, CreatePatientDto> patientService, int pId) =>
{
    var results = await patientService.GetUserByID(pId);

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

}).WithName("GetPatientID");

app.MapGet("/api/Doctor/{pId:int}", async ([FromServices] IUserService<Doctor, CreateDoctorDto> patientService, int pId) =>
{
    var results = await patientService.GetUserByID(pId);

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

}).WithName("GetDoctorByID");


app.MapPost("/api/create-patient", async ([FromServices] CreatePatientDto dto, [FromServices] UserService<Patient, CreatePatientDto> userService ) =>
{
    var result =  await userService.CreateUser(dto);

    return result.IsSuccess ? Results.Created() : Results.BadRequest(result);

    
}).WithName("CreatePatient").Accepts<CreatePatientDto>("application/json").Produces<ApiResponse>(201).Produces(400);



app.MapPost("/api/create-doctor", async ([FromServices] CreateDoctorDto dto, [FromServices] UserService<Doctor, CreateDoctorDto> userService) =>
{
    var result = await userService.CreateUser(dto);

    return result.IsSuccess ? Results.Created() : Results.BadRequest(result);

}).WithName("CreateDoctor").Accepts<CreateDoctorDto>("application/json").Produces<ApiResponse>(201).Produces(400);



app.MapPost("/appointment", async ([FromServices] AppointmentService appointmentService, AppointmentDto dto) =>
{
    var result = await appointmentService.CreateAppointment(dto);

    return result.IsSuccess ? Results.Ok() : Results.BadRequest(result);

}).WithName("CreateAppointment").Produces<ApiResponse>(200).Produces(404);



app.MapPost("/api/create-journalEntry", async ([FromServices] JournalEntryService journalEntryService, CreateJournalEntryDto dto) =>
{
    var results = await journalEntryService.CreateJournalEntry(dto);

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results); 

}).WithName("CreateJournalEntry").Accepts<CreateJournalEntryDto>("application/json").Produces<ApiResponse>(201).Produces(400);


app.MapGet("/api/JournalEntry/{id:int}", async ([FromServices] JournalEntryService journalEntryService, int id) =>
{
    var results = await journalEntryService.GetJournalEntryById(id);

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results); 

}).WithName("GetJournalByID").Produces<ApiResponse>(200).Produces(404);


app.MapPut("api/patient/change-password", async (string email, string oldPassword, string newPassowrd, [FromServices] UserService<Patient, CreatePatientDto> patientService) =>
{
    var result = await patientService.ChangePasswordByEmail(email, newPassowrd, oldPassword);

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

}).WithName("ChangePatientPassword").Produces<ApiResponse>(200).Produces(400);


app.MapPut("api/doctor/change-password", async (string email, string oldPassword, string newPassowrd, [FromServices] UserService<Doctor, CreateDoctorDto> patientService) =>
{
    var result = await patientService.ChangePasswordByEmail(email, newPassowrd, oldPassword);

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

}).WithName("ChangeDoctorPassword").Produces<ApiResponse>(200).Produces(400);


app.MapPost("api/loginPatient/", async ([FromServices] UserService<Patient, CreatePatientDto> userService, LoginDto dto) =>
{
    var result = await userService.LogInUserByEmail(dto);

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
}).WithName("PatientLogIn").Produces<ApiResponse>(200).Produces(400);


app.MapPost("api/loginDoctor/", async ([FromServices] UserService<Doctor, CreateDoctorDto> userService, LoginDto dto) =>
{
    var result = await userService.LogInUserByEmail(dto);

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
}).WithName("DoctorLogIn").Produces<ApiResponse>(200).Produces(400);




app.Run();

