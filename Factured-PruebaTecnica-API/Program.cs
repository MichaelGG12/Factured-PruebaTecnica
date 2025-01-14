using Factured_PruebaTecnica_API.DTO;
using Factured_PruebaTecnica_API.Entities;
using Factured_PruebaTecnica_API.Models;
using Factured_PruebaTecnica_API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PostgresDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<IMongoCollection<Task1>>(sp =>
{
    var context = sp.GetRequiredService<MongoDbContext>();
    return context.Tasks; // This will be injected wherever required
});
builder.Services.AddScoped<DataSyncService>(); // Register the DataSyncService

// Register MongoDB client and collection services
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDbSettings>();
    return new MongoClient(mongoSettings.ConnectionString);
});
builder.Services.AddSingleton(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var mongoSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDbSettings>();
    var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);
    return database.GetCollection<Board>("Boards");  // Ensure the collection name is correct
});

// Register GetBoardsQueryHandler with MediatR
builder.Services.AddTransient<IRequestHandler<GetBoardsQuery, List<BoardDto>>, GetBoardsQueryHandler>();


var app = builder.Build();

if (app.Environment.IsDevelopment()) // Configure the HTTP request pipeline.
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();