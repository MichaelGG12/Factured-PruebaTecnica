using Factured_PruebaTecnica_API.DTO;
using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Factured_PruebaTecnica_API.Services
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly MongoDbContext _mongoDbContext;

        public GetTaskByIdQueryHandler(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            // Fetch task from MongoDB
            var task = await _mongoDbContext.Tasks
                .Find(t => t.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
            }

            // Map the MongoDB Task entity to a TaskDto
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                BoardId = task.BoardId
            };
        }
    }
}