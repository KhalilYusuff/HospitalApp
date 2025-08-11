using backend.API.dto;
using backend.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Endpoints
{
    public static class JournalEntryEndpoints
    {
        public static void MapJournalEntryEndpoints(this IEndpointRouteBuilder app, WebApplication webApplication)
        {
            app.MapPost("/api/journalentries/create", async ([FromServices] JournalEntryService journalEntryService, CreateJournalEntryDto dto, HttpContext context) =>
            {
                var results = await journalEntryService.CreateJournalEntry(dto);
                results.TraceID = context.TraceIdentifier;

                return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

            }).WithName("CreateJournalEntry").Accepts<CreateJournalEntryDto>("application/json").Produces<ApiResponse>(201).Produces(400);

            app.MapGet("/api/journalentries/{id:int}", async ([FromServices] JournalEntryService journalEntryService, int id, HttpContext context) =>
            {
                var results = await journalEntryService.GetJournalEntryById(id);
                results.TraceID = context.TraceIdentifier;

                return results.IsSuccess ? Results.Ok(results) : Results.BadRequest(results);

            }).WithName("GetJournalEntryByID").Produces<ApiResponse>(200).Produces(404);
        }

    }
}
