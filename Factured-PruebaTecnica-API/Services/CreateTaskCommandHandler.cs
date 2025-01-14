using Factured_PruebaTecnica_API.DTO;
using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;
using MediatR;
using System.Threading.Tasks;

namespace Factured_PruebaTecnica_API.Services
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly PostgresDbContext _context;

        public CreateTaskCommandHandler(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            Task1 task = new Task1 // Create entry.
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                BoardId = request.BoardId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken); // Save to DB.

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