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
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Window
    {
        private UserViewModel userViewModel;
        public UserView(UserModel u)
        {
            InitializeComponent();
            this.userViewModel = new UserViewModel(u);
            this.DataContext = userViewModel;
        }

        /// <summary>
        /// This method is called when the user press the button of view board.
        /// </summary>
        /// <param name="sender">Built-in parameters</param>
        /// <param name="e">Built-in parameters</param>
        /// <returns>void</returns>*/
        private void Select_Board_Button(object sender, RoutedEventArgs e)
        {
            
            BoardModel b = userViewModel.SelectedBoard;
            if (b != null)
            {
                BoardView boardView = new BoardView(b,userViewModel.user);
                boardView.Show();
                this.Close();
            }            
        }

        /// <summary>
        /// This method is called when the user press the button of logout.
        /// </summary>
        /// <param name="sender">Built-in parameters</param>
        /// <param name="e">Built-in parameters</param>
        /// <returns>void</returns>*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            userViewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        /*
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BoardModel board = userViewModel.GetBoards();
            if (board != null)
            {
                BoardModel boardView = new BoardView(board);
                boardView.Show();
                this.Close();
            }
        }
        */
    }
}
