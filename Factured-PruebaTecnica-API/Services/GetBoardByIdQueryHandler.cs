using Factured_PruebaTecnica_API.DTO;
using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Factured_PruebaTecnica_API.Services
{
    public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, BoardDto>
    {
        private readonly MongoDbContext _context;

        public GetBoardByIdQueryHandler(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<BoardDto> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
        {
            Board board = await _context.Boards
                .Find(b => b.Id == request.Id) // Filter by ID
                .FirstOrDefaultAsync(cancellationToken);

            if (board == null)
            {
                throw new KeyNotFoundException($"Board with ID {request.Id} was not found.");
            }
           
            return new BoardDto // Map the Board entity to DTO
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                CreatedAt = board.CreatedAt
            };
        }
    }
}