using API.Data;
using API.dto;
using API.Model;
using backend.API.dto;
using Microsoft.EntityFrameworkCore;
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

            response.Result = await _context.Appointments.ToListAsync(); 

            if (response.Result is null)
            {
                response.ErrorMessages.Add("There are no appointments");
                return response; 
            }

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return response; 
        }


        public async Task<ApiResponse> GetAllPatientAppointmentsByPId(int id)
        {
            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

            var patientAppointments = await _context.Appointments.Where(a => a.PatientId == id).ToListAsync(); 

            if (patientAppointments is null)
            {
                response.ErrorMessages.Add("The specified patient has no appointments");
                return response;
            }

            response.IsSuccess = true;
            response.Result = patientAppointments;
            response.StatusCode = HttpStatusCode.OK;

            return response; 

        }

    }
}
