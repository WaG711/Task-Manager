using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Task_Manager.DataBase;
using Task_Manager.ViewModels;

namespace Task_Manager
{
    /// <summary>
    /// Логика взаимодействия для CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        public CreateWindow(IRepository repository)
        {
            InitializeComponent();
            DataContext = new CreateViewModel(repository);
        }
    }
}
