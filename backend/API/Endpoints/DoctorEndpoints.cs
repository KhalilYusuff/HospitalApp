using API.dto;
using API.Model;
using backend.API.dto;
using backend.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace backend.API.Endpoints
{
    public static class DoctorEndpoints
    {

        public static void MapDoctorEndpoints(this IEndpointRouteBuilder app, WebApplication webApplication)
        {
            app.MapGet("/api/doctors", async ([FromServices] IUserService<Doctor, CreateDoctorDto> doctorService, HttpContext context) => {

                var results = await doctorService.GetAllUsers();
                results.TraceID = context.TraceIdentifier;

                return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

            }).WithName("GetAllDoctors").Produces<ApiResponse>(200);

            app.MapGet("/api/doctors/{pId:int}", async (IUserService<Doctor, CreateDoctorDto> datientService, int pId, HttpContext context) =>
            {
                var results = await datientService.GetUserByID(pId);
                results.TraceID = context.TraceIdentifier;

                return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

            }).WithName("GetDoctorByID");

            app.MapPost("/api/doctors/create", async (CreateDoctorDto dto, [FromServices] IUserService<Doctor, CreateDoctorDto> userService, HttpContext context) =>
            {
                var result = await userService.CreateUser(dto);
                result.TraceID = context.TraceIdentifier;

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

            }).WithName("CreateDoctor").Accepts<CreateDoctorDto>("application/json").Produces<ApiResponse>(201).Produces(400);

            app.MapPut("api/doctors/changepassword", async (string email, string oldPassword, string newPassword, [FromServices] IUserService<Doctor, CreateDoctorDto> patientService, HttpContext context) =>
            {
                var result = await patientService.ChangePasswordByEmail(email, newPassword, oldPassword);
                result.TraceID = context.TraceIdentifier;

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);

            }).WithName("ChangeDoctorPassword").Produces<ApiResponse>(200).Produces(400);



            app.MapPost("api/doctors/login", async ([FromServices] IUserService<Doctor, CreateDoctorDto> userService, LoginDto dto, HttpContext context) =>
            {
                var result = await userService.LogInUserByEmail(dto);
                result.TraceID = context.TraceIdentifier;

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            }).WithName("DoctorLogIn").Produces<ApiResponse>(200).Produces(400);


        }

    }
}
