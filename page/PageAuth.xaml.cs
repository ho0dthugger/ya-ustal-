using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Тест.file;
using System.Data.Entity;

namespace Тест.page
{
    public partial class PageAuth : Page
    {
        private TestEntities dbContext;
        public PageAuth()
        {
            InitializeComponent();
            dbContext = new TestEntities();
        }

        private void Войти_Click(object sender, RoutedEventArgs e)
        {
            string username = txtLogin.Text;
            string password = txtPassword.Password;

            if (IsValidLogin(username, password, out string userRole))
            {
                var authWindow = Window.GetWindow(this) as AuthWindow;
                Window mainWindow;
                var user = dbContext.Пользователи.FirstOrDefault(u => u.Логин == username);
                if (userRole == "Администратор")
                {
                    mainWindow = new AdminWindow();
                }
                else if (userRole == "Пользователь" && user.ПоследняяУспПопыткаВхода == null) 
                {
                    mainWindow = new NewPasswordWindow();
                    user.ПоследняяУспПопыткаВхода = DateTime.Now;
                    dbContext.SaveChanges();
                }
                else
                {
                    mainWindow = new MainWindow();
                }

                mainWindow.Show();
                authWindow.Close();
                txtStatus.Text = "Успешная авторизация";
            }
            else
            {
                var user = dbContext.Пользователи.FirstOrDefault(u => u.Логин == username);
                if (user != null)
                {
                    if(user.Блокировка.GetValueOrDefault())
                    {
                        txtStatus.Text = "Вы заблокированы";
                        return;
                    }
                    if (user.ПоследняяУспПопыткаВхода.HasValue)
                    {
                        TimeSpan inactivityPeriod = DateTime.Now - user.ПоследняяУспПопыткаВхода.Value;
                        if (inactivityPeriod.TotalDays > 30)
                        {
                            user.Блокировка = true;
                            dbContext.SaveChanges();
                        }
                    }
                    if (user.ПоследняяПопыткаВхода.HasValue && DateTime.Now < user.ПоследняяПопыткаВхода.Value.AddMinutes(30))
                    {
                        user.ПопыткаВхода = (user.ПопыткаВхода ?? 0) + 1;
                    }
                    else
                    {
                        user.ПопыткаВхода = 1;
                        user.ПоследняяПопыткаВхода = DateTime.Now;
                    }
                    if (user.ПопыткаВхода >= 3)
                    {
                        user.Блокировка = true;
                        txtStatus.Text = "Аккаунт заблокирован!";
                    }
                    else
                    {
                        txtStatus.Text = $"Неверный логин или пароль! Осталось попыток: {3 - user.ПопыткаВхода}";
                    }
                    dbContext.SaveChanges();
                }
                else
                {
                    txtStatus.Text = "Нет такого пользователя";
                }
            }
        }

        private bool IsValidLogin(string username, string password, out string userRole)
        {
            var user = dbContext.Пользователи
                .Include(u => u.Роли)
                .FirstOrDefault(u => u.Логин == username);

            if (user != null && !(bool)user.Блокировка && password == user.Пароль)
            {
                user.ПопыткаВхода = 1;
                dbContext.SaveChanges();

                FrameApp.ТекущийПользователь = user;
                userRole = user.Роли?.НазваниеРоли;
                return true;
            }
            userRole = null;
            return false;
        }
    }
}
