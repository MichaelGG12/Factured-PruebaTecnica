using MediatR;

namespace Factured_PruebaTecnica_API.Models
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}