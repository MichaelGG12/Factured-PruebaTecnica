using Factured_PruebaTecnica_API.DTO;
using MediatR;

namespace Factured_PruebaTecnica_API.Models
{
    public class GetBoardByIdQuery : IRequest<BoardDto>
    {
        public int Id { get; set; }

        public GetBoardByIdQuery(int id)
        {
            Id = id;
        }
    }
}