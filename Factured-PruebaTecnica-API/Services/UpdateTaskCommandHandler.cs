using Factured_PruebaTecnica_API.DTO;
using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;
using MediatR;

namespace Factured_PruebaTecnica_API.Services
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
    {
        private readonly PostgresDbContext _context;

        public UpdateTaskCommandHandler(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FindAsync(new object[] { request.Id }, cancellationToken);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
            }

            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = request.Status;

            await _context.SaveChangesAsync(cancellationToken);

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