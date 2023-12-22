using Microsoft.EntityFrameworkCore;
using Task = Task_Manager.Model.Task;

namespace Task_Manager.DataBase
{
    public class TaskContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tasktracker.db");
        }
    }
}
