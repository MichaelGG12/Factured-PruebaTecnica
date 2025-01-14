using Factured_PruebaTecnica_API.DTO;
using MediatR;

namespace Factured_PruebaTecnica_API.Models
{
    public class GetTaskQuery : IRequest<List<TaskDto>> 
    {
        public int BoardId { get; set; }
    }
}