using DMTAgent.Infrastructure;
using System.Windows;

namespace DMTAgentCore.Views
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        #region Constructors

        public MainWindow(IViewModel<MainWindow> viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        #endregion Constructors
    }
}