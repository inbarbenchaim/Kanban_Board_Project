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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel viewModel;

        public BoardView(BoardModel b, UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(b,u);
            this.DataContext = viewModel;
        }

        /// <summary>
        /// This method is called when the user press the button to return the board list.
        /// </summary>
        /// <param name="sender">Built-in parameters</param>
        /// <param name="e">Built-in parameters</param>
        /// <returns>void</returns>*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.user;
            UserView userView = new UserView(u);
            userView.Show();
            this.Close();
        }
    }
}
