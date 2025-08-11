using API.dto;
using backend.API.dto;
using backend.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Endpoints
{
    public static class AppointmentEndpoints
    {
        public static void MapAppointmentEndpoints(this IEndpointRouteBuilder app, WebApplication webApplication)
        {
            app.MapGet("/api/appointments", async ([FromServices] AppointmentService appointmentService, HttpContext context) => {

                var results = await appointmentService.GetAllAppointments();
                results.TraceID = context.TraceIdentifier;

                return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

            }).WithName("GetAllAppointments").Produces<ApiResponse>(200); ;

            app.MapGet("/api/appointments/patient{Id:int}", async ([FromServices] AppointmentService appointmentService, HttpContext context, int Id) =>
            {
                var results = await appointmentService.GetAppointmentsForPatient(Id);
                results.TraceID = context.TraceIdentifier;

                return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

            }).WithName("GetAppointmentsByPatientID").Produces<ApiResponse>(200);

            app.MapGet("/api/appointments/doctor{Id:int}", async ([FromServices] AppointmentService appointmentService, int Id, HttpContext context) =>
            {
                var result = await appointmentService.GetAppointmentsForDoctor(Id);
                result.TraceID = context.TraceIdentifier;

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

            }).WithName("GetAppointmentsByDoctorID");

            app.MapGet("/api/appointments/upcomming/patient{id:int}", async ([FromServices] AppointmentService appointmentService, int id, HttpContext context) =>
            {
                var result = await appointmentService.GetUpcomingAppointmentsForPatient(id);
                result.TraceID = context.TraceIdentifier;

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

            }).WithName("GetUpcomingAppointments").Produces<ApiResponse>(200).Produces(400);

            app.MapGet("/api/appointments/previous/patient{id:int}", async ([FromServices] AppointmentService appointmentService, int id, HttpContext context) =>
            {
                var result = await appointmentService.GetPreviousAppointmensForPatient(id);
                result.TraceID = context.TraceIdentifier;

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            });


            app.MapPost("/appointments/create", async ([FromServices] AppointmentService appointmentService, AppointmentDto dto, HttpContext context) =>
            {
                var result = await appointmentService.CreateAppointment(dto);
                result.TraceID = context.TraceIdentifier;

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

            }).WithName("CreateAppointment").Produces<ApiResponse>(200).Produces(404);
        }

    }
}
