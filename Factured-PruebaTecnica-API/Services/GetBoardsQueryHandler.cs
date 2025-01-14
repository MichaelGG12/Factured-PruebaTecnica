using Factured_PruebaTecnica_API.DTO;
using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;
using MediatR;
using MongoDB.Driver;

namespace Factured_PruebaTecnica_API.Services
{
    public class GetBoardsQueryHandler : IRequestHandler<GetBoardsQuery, List<BoardDto>>
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Board> _boardsCollection;

        public GetBoardsQueryHandler(MongoDbContext context, IMongoCollection<Board> boardsCollection)
        {
            _context = context;
            _boardsCollection = boardsCollection;
        }

        public async Task<List<BoardDto>> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
        {
            var boards = await _boardsCollection
            .Find(FilterDefinition<Board>.Empty) // Empty filter to get all boards
            .ToListAsync(cancellationToken);

            return boards.Select(b => new BoardDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                CreatedAt = b.CreatedAt
            }).ToList();
        }
    }
}