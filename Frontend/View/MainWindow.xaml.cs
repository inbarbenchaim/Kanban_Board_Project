using Frontend.Model;
using Frontend.ViewModel;
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

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.viewModel = (MainViewModel)DataContext;

        }

        /// <summary>
        /// This method is continue the process of login, once the user press the login click, this method is called.
        /// </summary>
        /// <param name="sender">Built-in parameters</param>
        /// <param name="e">Built-in parameters</param>
        /// <returns>void</returns>*/
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Login();
            if (u != null)
            {
                UserView userView = new UserView(u);
                userView.Show();
                this.Close();
            }
        }

        /// <summary>
        /// This method is continue the process of register, once the user press the Register_Click, this method is called.
        /// </summary>
        /// <param name="sender">Built-in parameters</param>
        /// <param name="e">Built-in parameters</param>
        /// <returns>void</returns>*/
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Register();
        }
    }
}
