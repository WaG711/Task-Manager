using Microsoft.EntityFrameworkCore;
using Task = Task_Manager.Model.Task;

namespace Task_Manager.DataBase
{
    public class TaskRepository : IRepository
    {
        private readonly TaskContext _context;

        public TaskRepository(TaskContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.Database.EnsureCreated();
            _context.Tasks.Load();
        }

        public void Add(Task task)
        {
            _context.Tasks.Add(task);
            SaveChanges();
        }

        public void Delete(int taskId)
        {
            var taskToDelete = _context.Tasks.FirstOrDefault(task => task.Id == taskId);

            if (taskToDelete != null)
            {
                _context.Tasks.Remove(taskToDelete);
                SaveChanges();
            }
        }

        public void Update(Task task)
        {
            _context.Tasks.Update(task);
            SaveChanges();
        }

        public IEnumerable<Task> GetAll()
        {
            return _context.Tasks.OrderByDescending(task => task.CreationDate).ToList();
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
