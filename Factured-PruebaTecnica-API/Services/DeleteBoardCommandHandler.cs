using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;
using MediatR;

namespace Factured_PruebaTecnica_API.Services
{
    public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand, bool>
    {
        private readonly PostgresDbContext _context;

        public DeleteBoardCommandHandler(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
        {
            var board = await _context.Boards.FindAsync(new object[] { request.Id }, cancellationToken);

            if (board == null)
            {
                return false; // Board wasn't found
            }
            _context.Boards.Remove(board);
            await _context.SaveChangesAsync(cancellationToken);
            return true; // Success
        }
    }
}