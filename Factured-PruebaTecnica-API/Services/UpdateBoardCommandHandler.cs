using Factured_PruebaTecnica_API.DTO;
using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;
using MediatR;

namespace Factured_PruebaTecnica_API.Services
{
    public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, BoardDto>
    {
        private readonly PostgresDbContext _context;

        public UpdateBoardCommandHandler(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<BoardDto> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
        {
            var board = await _context.Boards.FindAsync(new object[] { request.Id }, cancellationToken);

            if (board == null)
            {
                throw new KeyNotFoundException($"Board with ID {request.Id} not found.");
            }

            board.Name = request.Name;
            board.Description = request.Description;

            _context.Boards.Update(board);
            await _context.SaveChangesAsync(cancellationToken);

            return new BoardDto
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                CreatedAt = board.CreatedAt
            };
        }
    }
}