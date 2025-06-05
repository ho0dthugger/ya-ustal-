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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BCrypt.Net;

namespace Тест.page
{
    /// <summary>
    /// Логика взаимодействия для PageTest.xaml
    /// </summary>
    public partial class PageTest : Page
    {
        public PageTest()
        {
            InitializeComponent();
        }
        private void GenerateHash_Click(object sender, RoutedEventArgs e)
        {
            string password = txtPassword.Password;

            try
            {
                // Генерация хеша с рабочей сложностью 12
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, 12);
                txtHashedPassword.Text = hashedPassword;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating hash: {ex.Message}");
            }
        }
    }
}
