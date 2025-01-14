using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;

namespace Factured_PruebaTecnica_API.Services
{
    public class DeleteTaskCommandHandler
    {
        private readonly PostgresDbContext _context;

        public DeleteTaskCommandHandler(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FindAsync(new object[] { request.Id }, cancellationToken);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
            return true; // Indicate success
        }
    }
}