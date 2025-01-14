using Factured_PruebaTecnica_API.DTO;
using MediatR;

namespace Factured_PruebaTecnica_API.Models
{
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        public int Id { get; set; }
    }
}
