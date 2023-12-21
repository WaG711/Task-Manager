using System.Windows.Input;
using System.Windows;
using Task_Manager.DataBase;

namespace Task_Manager.ViewModels
{
    public class CreateViewModel : ViewModelBase
    {
        private readonly IRepository _taskRepository;
        private string _taskText;

        public CreateViewModel(IRepository repository)
        {
            _taskRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            CreateTaskCommand = new RelayCommand(CreateTask);
        }

        public ICommand CreateTaskCommand { get; }

        public string TaskText
        {
            get => _taskText;
            set
            {
                if (_taskText != value)
                {
                    _taskText = value;
                    OnPropertyChanged(nameof(TaskText));
                }
            }
        }

        private void CreateTask(object parameter)
        {
            if (parameter is Window window)
            {
                if (!string.IsNullOrEmpty(TaskText))
                {
                    var newTask = new Task { Text = TaskText, CreationDate = DateTime.Now, IsCompleted = false };
                    _taskRepository.Add(newTask);
                    window.Close();
                }
                else
                {
                    MessageBox.Show("Введите текст задачи");
                }
            }
        }
    }
}
