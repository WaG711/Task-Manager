using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Task_Manager.DataBase;

namespace Task_Manager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRepository _taskRepository;
        private Task _selectedTask;
        private ObservableCollection<Task> _tasks;
        private ObservableCollection<Task> _completedTasks;
        private readonly ObservableCollection<Task> _lastTwoDeletedTasks = new ObservableCollection<Task>();

        public MainViewModel(IRepository repository)
        {
            _taskRepository = repository;
            Tasks = new ObservableCollection<Task>(repository.GetAll());
            CompletedTasks = new ObservableCollection<Task>();

            OpenCreateTaskWindowCommand = new RelayCommand(OpenCreateTaskWindow);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
        }

        public ICommand DeleteTaskCommand { get; }
        public ICommand OpenCreateTaskWindowCommand { get; }
        public ObservableCollection<Task> CompletedTasks
        {
            get => _completedTasks;
            set
            {
                _completedTasks = value;
                OnPropertyChanged(nameof(CompletedTasks));
            }
        }
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }
        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (_selectedTask != value)
                {
                    _selectedTask = value;
                    OnPropertyChanged(nameof(SelectedTask));
                }
            }
        }

        private void DeleteTask(object parameter)
        {
            if (parameter is not Task task)
            {
                MessageBox.Show("Выберите невыполненную задачу");
                return;
            }

            if (Tasks.Contains(task))
            {
                Tasks.Remove(task);
                task.IsCompleted = true;
                _lastTwoDeletedTasks.Add(task);

                if (_lastTwoDeletedTasks.Count > 2)
                {
                    CompletedTasks.Remove(_lastTwoDeletedTasks.First());
                    _lastTwoDeletedTasks.RemoveAt(0);
                }

                CompletedTasks.Add(task);
                _taskRepository.Update(task);
            }
            else
            {
                CompletedTasks.Remove(task);
            }

            _taskRepository.Delete(task.Id);
        }

        private void OpenCreateTaskWindow(object parameter) { }
    }
}
