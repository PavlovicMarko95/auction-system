using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using Auction.Model;

namespace Auction.UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using(SqlConnection conn = new SqlConnection())
            {

                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Person WHERE first_name=@FirstName and last_name=@LastName", conn);

                command.Parameters.AddWithValue("@FirstName", FirstName.Text);

                command.Parameters.AddWithValue("LastName", LastName.Text);

                command.CommandType = System.Data.CommandType.Text;

                int sr = Convert.ToInt32(command.ExecuteScalar());

                if (sr == 1)
                {

                    MainWindow mainWindow = new MainWindow(FirstName.Text);
                    
                    mainWindow.ShowDialog();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Check input");
                }
            }
        }

        private void BezPrijave(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow("");
            mainWindow.ShowDialog();
            this.Close();
        }
    }
}
