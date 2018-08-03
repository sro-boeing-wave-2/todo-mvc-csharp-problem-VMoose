using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Todo.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext (DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<Todo.Models.Note> Note { get; set; }
        public DbSet<Todo.Models.Checklist> Checklist { get; set; }
        public DbSet<Todo.Models.Label> Lable { get; set; }

    }
}
