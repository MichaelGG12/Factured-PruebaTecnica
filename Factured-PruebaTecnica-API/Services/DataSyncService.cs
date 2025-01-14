using Factured_PruebaTecnica_API.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Factured_PruebaTecnica_API.Services
{
    public class DataSyncService
    {
        private readonly PostgresDbContext _postgresDbContext;
        private readonly MongoDbContext _mongoDbContext;

        public DataSyncService(PostgresDbContext postgresDbContext, MongoDbContext mongoDbContext)
        {
            _postgresDbContext = postgresDbContext;
            _mongoDbContext = mongoDbContext;
        }

        public async Task SyncDataAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Fetch data from the source database (Postgres)
                var postgresBoards = await _postgresDbContext.Boards.ToListAsync(cancellationToken);
                var postgresTasks = await _postgresDbContext.Tasks.ToListAsync(cancellationToken);

                // Step 2: Fetch data from the target database (MongoDB)
                var mongoBoards = _mongoDbContext.Boards.AsQueryable().ToList();
                var mongoTasks = _mongoDbContext.Tasks.AsQueryable().ToList();

                // Step 3: Identify changes for boards (new, updated, deleted)
                var newBoards = postgresBoards.Where(p => !mongoBoards.Any(m => m.Id == p.Id)).ToList();
                var updatedBoards = postgresBoards.Where(p => mongoBoards.Any(m => m.Id == p.Id)).ToList();
                var deletedBoards = mongoBoards.Where(m => !postgresBoards.Any(p => p.Id == m.Id)).ToList();

                // Step 4: Apply changes to MongoDB (Boards)
                foreach (var newBoard in newBoards)
                {
                    await _mongoDbContext.Boards.InsertOneAsync(new Board
                    {
                        Id = newBoard.Id,
                        Name = newBoard.Name,
                        Description = newBoard.Description,
                        CreatedAt = newBoard.CreatedAt
                    });
                }

                foreach (var updatedBoard in updatedBoards)
                {
                    await _mongoDbContext.Boards.ReplaceOneAsync(
                        Builders<Board>.Filter.Eq(b => b.Id, updatedBoard.Id),
                        new Board
                        {
                            Id = updatedBoard.Id,
                            Name = updatedBoard.Name,
                            Description = updatedBoard.Description,
                            CreatedAt = updatedBoard.CreatedAt
                        });
                }

                foreach (var deletedBoard in deletedBoards)
                {
                    await _mongoDbContext.Boards.DeleteOneAsync(b => b.Id == deletedBoard.Id);
                }

                // Step 5: Identify changes for tasks (new, updated, deleted)
                var newTasks = postgresTasks.Where(p => !mongoTasks.Any(m => m.Id == p.Id)).ToList();
                var updatedTasks = postgresTasks.Where(p => mongoTasks.Any(m => m.Id == p.Id)).ToList();
                var deletedTasks = mongoTasks.Where(m => !postgresTasks.Any(p => p.Id == m.Id)).ToList();

                // Step 6: Apply changes to MongoDB (Tasks)
                foreach (var newTask in newTasks)
                {
                    await _mongoDbContext.Tasks.InsertOneAsync(new Task1
                    {
                        Id = newTask.Id,
                        Title = newTask.Title,
                        Description = newTask.Description,
                        Status = newTask.Status,
                        BoardId = newTask.BoardId,
                        CreatedAt = newTask.CreatedAt
                    });
                }

                foreach (var updatedTask in updatedTasks)
                {
                    await _mongoDbContext.Tasks.ReplaceOneAsync(
                        Builders<Task1>.Filter.Eq(t => t.Id, updatedTask.Id),
                        new Task1
                        {
                            Id = updatedTask.Id,
                            Title = updatedTask.Title,
                            Description = updatedTask.Description,
                            Status = updatedTask.Status,
                            BoardId = updatedTask.BoardId,
                            CreatedAt = updatedTask.CreatedAt
                        });
                }

                foreach (var deletedTask in deletedTasks)
                {
                    await _mongoDbContext.Tasks.DeleteOneAsync(t => t.Id == deletedTask.Id);
                }

                Console.WriteLine("Data synchronization completed successfully.");
            }
            catch (Exception ex)
            {
                // Step 7: Error handling
                Console.WriteLine($"An error occurred during data synchronization: {ex.Message}");
                throw;
            }
        }
    }
}