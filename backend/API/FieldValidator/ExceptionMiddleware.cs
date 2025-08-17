using Azure.Core.Serialization;
using backend.API.dto;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Net;

using System.Net.NetworkInformation;
using System.Text.Json;
namespace backend.API.FieldValidator
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;    
        }


        public async Task InvokeAsync(HttpContext context)
        {
            
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occured: {StackTrace} {Message}", e.StackTrace ,e.Message);
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            var traceiD = context.TraceIdentifier; 

            var statusCode = context.Response.StatusCode = e switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                DBConcurrencyException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new ApiResponse()
            {
                IsSuccess = false,
                ErrorMessages = new List<string> { e.Message },
                TraceID = traceiD,
                StatusCode = (HttpStatusCode) statusCode,
                Timestamp = DateTime.UtcNow.ToString("O")
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json); 
        }

        
    }
}
