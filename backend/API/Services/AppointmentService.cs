using API.Data;
using API.dto;
using API.Model;
using backend.API.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace backend.API.Services
{
    public class AppointmentService
    {

        private readonly AppDbContext _context;
        
        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ApiResponse> CreateAppointment(AppointmentDto dto)
        {

            var response = new ApiResponse();

            if (dto.EndDate <= dto.StarteDate)
            {
                throw new Exception("End date cannot be less than or equal to start date");
            }

            Patient? patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == dto.PatientID);
            Doctor? doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == dto.DoctorID);

            if (patient is null || doctor is null)
            {
                throw new Exception("User does not exist");
                
            }

            bool isDoctorBusy = await _context.Appointments.AnyAsync(a => a.DoctorId == dto.DoctorID
                                                                && a.StartDate < dto.EndDate
                                                               && a.EndDate > dto.StarteDate);
           
            if (isDoctorBusy)
            {
                throw new ValidationException("Doctor is busy at the chosen appointment time");
            }

            Appointment appointment = new() { Patient = patient, Doctor = doctor, 
                                                StartDate = dto.StarteDate, EndDate = dto.EndDate ,Status = dto.Status };

            await _context.Appointments.AddAsync(appointment);

            await _context.SaveChangesAsync();

            response.Result = appointment.ToDto(); 
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return response; 
        }


        public async Task<ApiResponse> GetAllAppointments()
        {
            var response = new ApiResponse(); 

            var result = await _context.Appointments.ToListAsync();

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            if (!result.IsNullOrEmpty())
            {
               response.Result = result.Select(a => a.ToDto()).ToList();
            }

            response.Note = $"No appointments found";
            return response; 
        }


        public async Task<ApiResponse> GetAppointmentsForPatient(int id)
        {
            var response = new ApiResponse(); 

            var patient = await _context.Patients.FindAsync(id);
            if (patient is null)
            {
                throw new Exception("Given ID does not exist or does not belong to a patient");
            }

            var patientAppointments = await _context.Appointments.Where(a => a.PatientId == id).ToListAsync();

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
          
            if (!patientAppointments.IsNullOrEmpty())
            {
                response.Result = patientAppointments.Select(a => a.ToDto()).ToList();
            }

            response.Note = $"There are no Appointments for {patient.FirstName} {patient.LastName}";

            return response; 

        }
        public async Task<ApiResponse> GetUpcomingAppointmentsForPatient(int id)
        {
            var response = new ApiResponse();

            var patient = await _context.Patients.FindAsync(id) ?? throw new Exception("User with the given ID does not exist");

            var upcomingAppointments = await _context.Appointments.Where(a => a.PatientId == id
                                                                         && a.EndDate > DateTime.Now).ToListAsync();
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;

            if (!upcomingAppointments.IsNullOrEmpty())
            {
                response.Result = upcomingAppointments.Select(a => a.ToDto()).ToList(); 
                return response;  
            }


            response.Note = $"There are no upcoming appointments for {patient.FirstName} {patient.LastName}";
            return response; 



        }

        public async Task<ApiResponse> GetPreviousAppointmensForPatient(int id)
        {
            var response = new ApiResponse();

            var patient = await _context.Patients.FindAsync(id); 
            if (patient is null)
            {
                throw new Exception("User does not exist"); 
            }

            var previousAppoiontments = await _context.Appointments.Where(a => a.PatientId == id
                                                                           && a.EndDate < DateTime.Now).ToListAsync();
            
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true; 

            if (!previousAppoiontments.IsNullOrEmpty())
            {
                response.Result = previousAppoiontments.Select(a => a.ToDto()).ToList();
                return response; 
                
            }

            response.Note = $"There are no upcoming appointments for {patient.FirstName} {patient.LastName}";
            return response; 
        }

        public async Task<ApiResponse> GetAppointmentsForDoctor(int id)
        {
            var response = new ApiResponse(); 

            var doctor = await _context.Doctors.FindAsync(id); 
            if (doctor is null)
            {
                throw new Exception("Given ID does not exist or does not belong a doctor"); 
            }

            var doctorAppointments = await _context.Appointments.Where(a => a.DoctorId == id).ToListAsync();
            response.IsSuccess = true;

            response.StatusCode = HttpStatusCode.OK;

            if (!doctorAppointments.IsNullOrEmpty())
            {
                response.Result = doctorAppointments.Select(a => a.ToDto()).ToList();
                return response;  
            }
            response.Note = $"There are no Appointments for {doctor.FirstName} {doctor.LastName}";

            return response;

        }

    }
}
