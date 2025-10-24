using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed; // Important!
using PracticeDotNetCore.Data;
using PracticeDotNetCore.Models;
using System.Text.Json; // Important!
// ... other usings for your DbContext and Models

namespace PracticeDotNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IDistributedCache _cache; // Inject the cache

        public TodosController(TodoContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            const string cacheKey = "all_todos";
            string serializedTodos;

            var cachedTodos = await _cache.GetStringAsync(cacheKey);

            if (cachedTodos != null)
            {
                // CACHE HIT: Data found in Redis
                serializedTodos = cachedTodos;
            }
            else
            {
                // CACHE MISS: Data not found, get from DB
                var todos = await _context.TodoItems.ToListAsync();
                serializedTodos = JsonSerializer.Serialize(todos);

                // Set the data in Redis cache with an expiration time
                var cacheOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1)); // Cache for 1 minute
                await _cache.SetStringAsync(cacheKey, serializedTodos, cacheOptions);
            }

            // For the demo, we return a special object so the frontend knows the source
            var response = new
            {
                Source = (cachedTodos != null) ? "Cache" : "Database",
                Data = JsonSerializer.Deserialize<List<TodoItem>>(serializedTodos)
            };

            return Ok(response);
        }

        // --- IMPORTANT: CACHE INVALIDATION ---
        // In every method that changes data, you must clear the cache!

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // Invalidate the cache because we added a new item
            await _cache.RemoveAsync("all_todos");

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // Do the same `await _cache.RemoveAsync("all_todos");` for your PUT and DELETE methods!
    }
}