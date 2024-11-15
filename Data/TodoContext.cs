using Microsoft.EntityFrameworkCore;  
using TodoList.Models;

namespace TodoList.Data
{
    public class TodoContext :DbContext{
        public TodoContext(DbContextOptions<TodoContext> options) : base(options){}
        public DbSet<TodoItem> TodoItems {get; set;} 
        public DbSet<TodoList.Models.Movies> Movies { get; set; } = default!;
    }
}