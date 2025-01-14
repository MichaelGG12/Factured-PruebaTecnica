using Factured_PruebaTecnica_API.DTO;
using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Factured_PruebaTecnica_API.Services
{
    public class GetTasksQueryHandler : IRequestHandler<GetTaskQuery, List<TaskDto>>
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Task1> _tasksCollection;

        public GetTasksQueryHandler(MongoDbContext context, IMongoCollection<Task1> tasksCollection)
        {
            _context = context;
            _tasksCollection = tasksCollection;
        }

        public async Task<List<TaskDto>> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            // If BoardId is provided, filter by BoardId
            var filter = request.BoardId > 0 ? Builders<Task1>.Filter.Eq(t => t.BoardId, request.BoardId) : FilterDefinition<Task1>.Empty;

            var tasks = await _tasksCollection
                .Find(filter) // Apply filter based on BoardId
                .ToListAsync(cancellationToken);

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                BoardId = t.BoardId,
                Status = t.Status,
            }).ToList();
        }
    }
}