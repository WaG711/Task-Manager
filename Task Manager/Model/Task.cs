namespace Task_Manager.Model
{
    public class Task
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
    }
}
