using Factured_PruebaTecnica_API.DTO;
using MediatR;

namespace Factured_PruebaTecnica_API.Models
{
    public class CreateTaskCommand : IRequest<TaskDto>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int BoardId { get; set; }
    }
}