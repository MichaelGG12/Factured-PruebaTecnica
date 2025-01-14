using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Factured_PruebaTecnica_API.Entities
{
    [Table("Board")]
    public class Board
    {
        [Key, Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}