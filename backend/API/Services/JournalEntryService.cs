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

            JournalEntry journalEntry = dto.ToJournalEntry() ?? throw new Exception("Journal entry cannot be null. Patient or Doctor object with the specified ID might not exist");

            _context.JournalEntries.Add(journalEntry);

            await _context.SaveChangesAsync(); 

            response.Result = journalEntry.toDto();    
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;

            return response;

        }



        public async Task<ApiResponse> GetJournalEntryById(int id)
        {
            var response = new ApiResponse(); 

            var entry = await _context.JournalEntries.FindAsync(id) ?? throw new Exception("Journal entry not found");

            response.Result = entry.toDto();
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }


    }
}
