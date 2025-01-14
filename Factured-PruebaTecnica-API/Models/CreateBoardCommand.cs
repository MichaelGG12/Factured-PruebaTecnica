using Factured_PruebaTecnica_API.DTO;
using MediatR;

namespace Factured_PruebaTecnica_API.Models
{
    public class CreateBoardCommand : IRequest<BoardDto>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}