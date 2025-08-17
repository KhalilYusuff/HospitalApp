namespace backend.API.Model
{
    using API.dto;
    using API.Model;
    using global::API.Model;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    public class Perscription : IConvertToDto<PerscriptionDto>
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string Dosage { get; set; } = "";
        [Required]
        public DateTime PrescribedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public string? Notes { get; set; }
        [Required]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        [Required]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        [Timestamp]
        public byte[] Version { get; set; }

        public PerscriptionDto ToDto()
        {
            throw new NotImplementedException();
        }
    }
}
