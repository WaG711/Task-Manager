using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Task_Manager.DataBase;
using GalaSoft.MvvmLight.Messaging;
using Task = Task_Manager.Model.Task;

namespace Task_Manager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRepository _taskRepository;
        private Task _selectedTask;
        private ObservableCollection<Task> _tasks;
        private ObservableCollection<Task> _completedTasks;
        private readonly ObservableCollection<Task> _lastDeletedTasks = new ObservableCollection<Task>();
        private readonly byte _maxQueue = 3;

        public MainViewModel(IRepository repository)
        {
            _taskRepository = repository;
            Tasks = new ObservableCollection<Task>(repository.GetAll());
            CompletedTasks = new ObservableCollection<Task>();

            OpenCreateWindowCommand = new RelayCommand(OpenCreateWindow);
            DeleteTaskCommand = new RelayCommand(DeleteTask);

            Messenger.Default.Register<Task>(this, HandleNewTaskMessage);
        }

        public ICommand DeleteTaskCommand { get; }
        public ICommand OpenCreateWindowCommand { get; }
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
                _lastDeletedTasks.Add(task);

                if (_lastDeletedTasks.Count > _maxQueue)
                {
                    CompletedTasks.Remove(_lastDeletedTasks.First());
                    _lastDeletedTasks.RemoveAt(0);
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

        private void HandleNewTaskMessage(Task newTask)
        {
            Tasks.Add(newTask);
            _taskRepository.Add(newTask);
        }

        private void OpenCreateWindow(object parameter)
        {
            var createTaskViewModel = new CreateViewModel(_taskRepository);
            var createTaskWindow = new CreateWindow(_taskRepository);
            createTaskWindow.DataContext = createTaskViewModel;
            createTaskWindow.ShowDialog();
        }
    }
}
