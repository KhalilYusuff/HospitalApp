using API.dto;
using API.Model;
using backend.API.dto;
using backend.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace backend.API.Endpoints
{
    public static class PatientEndpoints 
    {
        public static void MapPatientEndpoints(this IEndpointRouteBuilder app, WebApplication webApplication)
        {
            app.MapGet("/api/patients", async ([FromServices] IUserService<Patient, CreatePatientDto> patientService, HttpContext context) => {

                var results = await patientService.GetAllUsers();
                results.TraceID = context.TraceIdentifier;

                return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

            }).WithName("GetAllPatients").Produces<ApiResponse>(200);

            app.MapGet("/api/patients/{pId:int}", async ([FromServices] IUserService<Patient, CreatePatientDto> patientService, int pId, HttpContext context) =>
            {
                var results = await patientService.GetUserByID(pId);
                results.TraceID = context.TraceIdentifier;

                return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

            }).WithName("GetPatientID");

            app.MapPost("/api/patients/create", async (CreatePatientDto dto, [FromServices] IUserService<Patient, CreatePatientDto> userService, HttpContext context) =>
            {
                var result = await userService.CreateUser(dto);
                result.TraceID = context.TraceIdentifier;
                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);


            }).WithName("CreatePatient").Accepts<CreatePatientDto>("application/json").Produces<ApiResponse>(201).Produces(400);

            app.MapPut("api/patient/changepassword", async (string email, string oldPassword, string newPassowrd, [FromServices] IUserService<Patient, CreatePatientDto> patientService, HttpContext context) =>
            {
                var result = await patientService.ChangePasswordByEmail(email, newPassowrd, oldPassword);
                result.TraceID = context.TraceIdentifier;

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

            }).WithName("ChangePatientPassword").Produces<ApiResponse>(200).Produces(400);

            app.MapPost("api/patients/login", async ([FromServices] IUserService<Patient, CreatePatientDto> userService, LoginDto dto, HttpContext context) =>
            {
                var result = await userService.LogInUserByEmail(dto);
                result.TraceID = context.TraceIdentifier;

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            }).WithName("PatientLogIn").Produces<ApiResponse>(200).Produces(400);

<<<<<<< Updated upstream
=======
            app.MapDelete("api/patients/{id:int}/delete", async ([FromServices] IUserService<Patient, CreatePatientDto> userService, HttpContext context, int id) =>
            {
                var result = await userService.DeleteUser(id);
                result.TraceID = context.TraceIdentifier;

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

            }).WithName("DeleteUserByID").Produces<ApiResponse>(200).Produces(400);

>>>>>>> Stashed changes

        }



    }
}
