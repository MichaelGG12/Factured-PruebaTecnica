using MediatR;

namespace Factured_PruebaTecnica_API.Models
{
    public class DeleteBoardCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}