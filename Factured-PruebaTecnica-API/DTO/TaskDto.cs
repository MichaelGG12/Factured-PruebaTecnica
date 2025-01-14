namespace Factured_PruebaTecnica_API.DTO
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BoardId { get; set; }
    }
}