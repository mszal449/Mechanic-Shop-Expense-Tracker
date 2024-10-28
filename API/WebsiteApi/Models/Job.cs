using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteApi.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string CarModel { get; set; }

        [EnumDataType(typeof(Status))]
        public Status JobStatus { get; set; }

        public string? Supervisor { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public Decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Job()
        {
            this.JobStatus = Status.Pending;
        }
    }

    public enum Status
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
}