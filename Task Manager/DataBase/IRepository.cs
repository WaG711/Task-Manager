using Task = Task_Manager.Model.Task;

namespace Task_Manager.DataBase
{
    public interface IRepository
    {
        IEnumerable<Task> GetAll();
        void Add(Task task);
        void Update(Task task);
        void Delete(int taskId);
    }
}
