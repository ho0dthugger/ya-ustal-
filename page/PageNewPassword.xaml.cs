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
using Тест.file;

namespace Тест.page
{
    /// <summary>
    /// Логика взаимодействия для PageNewPassword.xaml
    /// </summary>
    public partial class PageNewPassword : Page
    {
        private TestEntities dbContext;
        private Пользователи currentUser;
        public PageNewPassword()
        {
            InitializeComponent();
            dbContext = new TestEntities();
            currentUser = FrameApp.ТекущийПользователь;
            if (currentUser != null)
            {
                currentUser = dbContext.Пользователи.Find(currentUser.Код);
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            if(string.IsNullOrWhiteSpace(newPassword))
            {
                txtErrorMessage.Text = "Введите новый пароль!";
                return;
            }
            if (newPassword.Length < 6)
            {
                txtErrorMessage.Text = "Пароль должен содержать минимум 6 символов!";
                return;
            }
            if (newPassword != confirmPassword)
            {
                txtErrorMessage.Text = "Пароли не совпадают";
                return;
            }
            try
            {
                currentUser.Пароль = newPassword;
                currentUser.ПоследняяУспПопыткаВхода = DateTime.Now;
                currentUser.ПопыткаВхода = 0;

                dbContext.SaveChanges();
                MessageBox.Show("Вы успешно сменили пароль");

                Window mainWindow = new MainWindow();
                mainWindow.Show();

                Window.GetWindow(this).Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при смене пароля {ex.Message}");
            }
        }
    }
}
