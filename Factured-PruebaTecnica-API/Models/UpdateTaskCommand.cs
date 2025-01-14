using Factured_PruebaTecnica_API.DTO;
using MediatR;

namespace Factured_PruebaTecnica_API.Models
{
    public class UpdateTaskCommand : IRequest<TaskDto>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}
