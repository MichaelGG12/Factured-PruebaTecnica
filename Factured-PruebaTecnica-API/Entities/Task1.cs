using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Factured_PruebaTecnica_API.Entities
{
    [Table("Task")]
    public class Task1
    {
        [Key, Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = string.Empty;
        public int Status { get; set; } // Pending = 0, InProgress = 1, Completed = 2 
        public DateTime CreatedAt { get; set; }
        public int BoardId { get; set; }
    }
}