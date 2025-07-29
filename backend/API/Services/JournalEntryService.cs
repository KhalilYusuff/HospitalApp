using API.Data;
using API.Model;
using backend.API.dto;
using backend.API.Model;
using System.Net;
using System.Numerics;

namespace backend.API.Services
{
    public class JournalEntryService
    {
        private readonly AppDbContext _context;

        public JournalEntryService(AppDbContext context)
        {
            _context = context; 
        }

        public async Task<ApiResponse> CreateJournalEntry(CreateJournalEntryDto dto)
        {

            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

            



           


            JournalEntry journalEntry = dto.ToJournalEntry();

            if (journalEntry is null)
            {
                response.ErrorMessages.Add("Journal entry cannot be null. Patient or Doctor object with the specified ID might not exist");
                return response;
            }

            _context.JournalEntries.Add(journalEntry);

            await _context.SaveChangesAsync(); 

            response.Result = journalEntry.toDto();    
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;

            return response;

        }



        public async Task<ApiResponse> GetJournalEntryById(int id)
        {
            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = new List<string>() };

            var entry = await _context.JournalEntries.FindAsync(id);

            if (entry == null)
            {
                response.ErrorMessages.Add("Could not find the journal entry");
                return response;
            }

            response.Result = entry.toDto();
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }


    }
}
