using API.Data;
using API.dto;
using API.Model;
using System.Linq;
using System.Threading.Tasks;

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

//Get ALl patients from Db
app.MapGet("/patients", async (AppDbContext db) => { 
    var patients = await db.Patients.ToListAsync();

    return patients.Select(p => p.toDto()).ToList();
 
});

//get all appointments (Need to create an AppointmentDto)
app.MapGet("/appointments", async (AppDbContext db) => {
    var appoinments = await db.Appointments.ToListAsync();
    return appoinments.Select(a => a.toDto()).ToList();

    });


//Get patient based in id
app.MapGet("/patient", async (AppDbContext db, int pId) =>
{
    var patient = await db.Patients.FirstOrDefaultAsync( p => p.Id == pId);
    return patient.toDto();
});


//add a patientDto to DB
app.MapPost("/patients", async (AppDbContext db, CreatePatientDto dto) =>
{
    var patient = dto.ToPatient();

    db.Patients.Add(patient);

    await db.SaveChangesAsync();
    
    return Results.Created($"/patients{patient.Id}", patient.toDto());
});



app.MapPost("/appointment", async (AppDbContext db, AppointmentDto dto) =>
{
    var p = db.Patients.FirstOrDefaultAsync(p => p.Id == dto.PatientID);
    var d = db.Doctors.FirstOrDefaultAsync(d => d.Id == dto.DoctorID);
    var a = new Appointment
    {
       p,
       d,
       dto.Date,
       dto.Status
    };

    db.Appointments.Add(a);
    await db.SaveChangesAsync();

    return Results.Created($"/appointment{app.Id}", app);

});

app.Run();

