using API.Data;
using API.dto;
using API.Model;
using backend.API.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

            Patient? patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == dto.PatientID);
            Doctor? doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == dto.DoctorID); 

            if (patient is null || doctor is null)
            {
                response.ErrorMessages.Add("Could not find the specified user");
                return response;
            }

            Appointment appointment = new() { Patient = patient, Doctor = doctor, AppointmentDate = dto.Date, Status = dto.Status };

            await _context.Appointments.AddAsync(appointment);

            await _context.SaveChangesAsync();

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return response; 
        }


        public async Task<ApiResponse> GetAllAppointments()
        {
            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

            var result = await _context.Appointments.ToListAsync();

            response.Result = result.Select(a => a.ToDto()).ToList(); 

            if (result.IsNullOrEmpty())
            {
                throw new Exception("No appointments found");
                
            }

            response.IsSuccess = true;
            
            response.StatusCode = HttpStatusCode.OK;
           

            return response; 


        }


        public async Task<ApiResponse> GetAppointmentsForPatient(int id)
        {
            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

            var patient = await _context.Patients.FindAsync(id);
            if (patient is null)
            {
                throw new Exception("Given ID does not exist or does not belong to a patient");
            }

            var patientAppointments = await _context.Appointments.Where(a => a.PatientId == id).ToListAsync();
            response.Result = patientAppointments.Select(a => a.ToDto()).ToList();

            if (patientAppointments.IsNullOrEmpty())
            {
                throw new Exception("Specified user has no appointments"); 
            }

            response.IsSuccess = true;
            
            response.StatusCode = HttpStatusCode.OK;

            return response; 

        }

        public async Task<ApiResponse> GetAppointmentsForDoctor(int id)
        {
            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

            var doctor = await _context.Doctors.FindAsync(id); 
            if (doctor is null)
            {
                throw new Exception("Given ID does not exist or does not a doctor"); 
            }

            var doctorAppointments = await _context.Appointments.Where(a => a.DoctorId == id).ToListAsync();
            response.Result = doctorAppointments.Select(a => a.ToDto()).ToList();

            if (doctorAppointments.IsNullOrEmpty())
            {
                throw new Exception("Given ID does not belong to a doctor"); 
            }

            response.IsSuccess = true;

            response.StatusCode = HttpStatusCode.OK;

            return response;

        }

    }
}
