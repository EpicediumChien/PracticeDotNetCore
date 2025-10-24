using Microsoft.EntityFrameworkCore;
using PracticeDotNetCore.Models;
using System.Collections.Generic;

namespace PracticeDotNetCore.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
