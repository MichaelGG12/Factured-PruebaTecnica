using Factured_PruebaTecnica_API.DTO;
using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;
using MediatR;

namespace Factured_PruebaTecnica_API.Services
{
    public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, BoardDto>
    {
        private readonly PostgresDbContext _context;

        public CreateBoardCommandHandler(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<BoardDto> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            Board board = new()
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };
            _context.Boards.Add(board);
            await _context.SaveChangesAsync(cancellationToken); // Save to DB.

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