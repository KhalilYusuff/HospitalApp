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
//builder.Services.AddScoped<HttpContext>(); 
        

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


app.MapGet("/api/get-all-patients", async ([FromServices]IUserService<Patient, CreatePatientDto> patientService, HttpContext context) => {
    

    var results = await patientService.GetAllUsers();
    results.TraceID = context.TraceIdentifier; 

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results); 
 
}).WithName("GetAllPatients").Produces<ApiResponse>(200);


app.MapGet("/api/get-all-doctors", async ([FromServices] IUserService<Doctor, CreateDoctorDto> doctorService, HttpContext context) => {
    
    var results = await doctorService.GetAllUsers();
    results.TraceID = context.TraceIdentifier; 

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

}).WithName("GetAllDoctors").Produces<ApiResponse>(200);

//get all appointments (Need to create an AppointmentDto)
app.MapGet("/api/get-all-appointments", async ([FromServices] AppointmentService appointmentService, HttpContext context) => {

    var results = await appointmentService.GetAllAppointments();
    results.TraceID = context.TraceIdentifier; 

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results); 
 
}).WithName("GetAllAppointments").Produces<ApiResponse>(200); ;

app.MapGet("/api/patient-appointments{Id:int}", async ([FromServices] AppointmentService appointmentService, HttpContext context, int Id) =>
{
    var results = await appointmentService.GetAppointmentsForPatient(Id);
    results.TraceID = context.TraceIdentifier;

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results); 

}).WithName("GetAppointmentsByPatientID").Produces<ApiResponse>(200);

app.MapGet("/api/doctor-appointments{Id:int}", async ([FromServices] AppointmentService appointmentService, int Id, HttpContext context) =>
{
    var result = await appointmentService.GetAppointmentsForDoctor(Id);
    result.TraceID = context.TraceIdentifier;

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

}).WithName("GetAppointmentsByDoctorID");

//Get patient based on id
app.MapGet("/api/get-patient/{pId:int}", async ([FromServices] IUserService<Patient, CreatePatientDto> patientService, int pId, HttpContext context) =>
{
    var results = await patientService.GetUserByID(pId);
    results.TraceID = context.TraceIdentifier;

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

}).WithName("GetPatientID");


app.MapGet("/api/get-doctor/{pId:int}", async ( IUserService<Doctor, CreateDoctorDto> patientService, int pId, HttpContext context) =>
{
    var results = await patientService.GetUserByID(pId);
    results.TraceID = context.TraceIdentifier;

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

}).WithName("GetDoctorByID");


app.MapPost("/api/create-patient", async ( CreatePatientDto dto, [FromServices] IUserService<Patient, CreatePatientDto> userService, HttpContext context ) =>
{
    var result =  await userService.CreateUser(dto);
    result.TraceID = context.TraceIdentifier; 
    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

    
}).WithName("CreatePatient").Accepts<CreatePatientDto>("application/json").Produces<ApiResponse>(201).Produces(400);



app.MapPost("/api/create-doctor", async ( CreateDoctorDto dto, [FromServices] IUserService<Doctor, CreateDoctorDto> userService, HttpContext context) =>
{
    var result = await userService.CreateUser(dto);
    result.TraceID = context.TraceIdentifier;

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

}).WithName("CreateDoctor").Accepts<CreateDoctorDto>("application/json").Produces<ApiResponse>(201).Produces(400);



app.MapPost("/create-appointment", async ([FromServices] AppointmentService appointmentService, AppointmentDto dto, HttpContext context) =>
{
    var result = await appointmentService.CreateAppointment(dto);
    result.TraceID = context.TraceIdentifier;

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

}).WithName("CreateAppointment").Produces<ApiResponse>(200).Produces(404);



app.MapPost("/api/create-journalEntry", async ([FromServices] JournalEntryService journalEntryService, CreateJournalEntryDto dto, HttpContext context) =>
{
    var results = await journalEntryService.CreateJournalEntry(dto);
    results.TraceID = context.TraceIdentifier;

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results); 

}).WithName("CreateJournalEntry").Accepts<CreateJournalEntryDto>("application/json").Produces<ApiResponse>(201).Produces(400);


app.MapGet("/api/get-journalentry/{id:int}", async ([FromServices] JournalEntryService journalEntryService, int id, HttpContext context) =>
{
    var results = await journalEntryService.GetJournalEntryById(id);
    results.TraceID = context.TraceIdentifier;

    return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results); 

}).WithName("GetJournalByID").Produces<ApiResponse>(200).Produces(404);


app.MapPut("api/patient/change-password", async (string email, string oldPassword, string newPassowrd, [FromServices] IUserService<Patient, CreatePatientDto> patientService, HttpContext context) =>
{
    var result = await patientService.ChangePasswordByEmail(email, newPassowrd, oldPassword);
    result.TraceID = context.TraceIdentifier;

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

}).WithName("ChangePatientPassword").Produces<ApiResponse>(200).Produces(400);


app.MapPut("api/doctor/change-password", async (string email, string oldPassword, string newPassword, [FromServices] IUserService<Doctor, CreateDoctorDto> patientService, HttpContext context) =>
{
    var result = await patientService.ChangePasswordByEmail(email, newPassword, oldPassword);
    result.TraceID = context.TraceIdentifier;

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

}).WithName("ChangeDoctorPassword").Produces<ApiResponse>(200).Produces(400);


app.MapPost("api/loginPatient/", async ([FromServices] UserService<Patient, CreatePatientDto> userService, LoginDto dto, HttpContext context) =>
{
    var result = await userService.LogInUserByEmail(dto);
    result.TraceID = context.TraceIdentifier;

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
}).WithName("PatientLogIn").Produces<ApiResponse>(200).Produces(400);


app.MapPost("api/loginDoctor/", async ([FromServices] UserService<Doctor, CreateDoctorDto> userService, LoginDto dto, HttpContext context) =>
{
    var result = await userService.LogInUserByEmail(dto);
    result.TraceID = context.TraceIdentifier;

    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
}).WithName("DoctorLogIn").Produces<ApiResponse>(200).Produces(400);




app.Run();

