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
using System.Data.Entity;

namespace Тест.page
{
    /// <summary>
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page
    {
        private TestEntities dbContext;
        public PageAdmin()
        {
            InitializeComponent();
            dbContext = new TestEntities();
            LoadData();
        }

        private void LoadData()
        {
            usersGrid.ItemsSource = dbContext.Пользователи.Include(p => p.Роли).ToList();
            cmbRole.ItemsSource = dbContext.Роли.ToList();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newUser = new Пользователи
                {
                    ФИО = txtFullName.Text,
                    Логин = txtLogin.Text,
                    Пароль = txtPassword.Password,
                    Роль = ((Роли)cmbRole.SelectedItem).Код,
                    Блокировка = chkBlocked.IsChecked,
                    ПопыткаВхода = 0,
                    ПоследняяПопыткаВхода = null,
                    ПоследняяУспПопыткаВхода = null
                };
                dbContext.Пользователи.Add(newUser);
                dbContext.SaveChanges();

                usersGrid.ItemsSource = dbContext.Пользователи.ToList();

                txtFullName.Text = "";
                txtLogin.Text = "";
                txtPassword.Password = "";
                chkBlocked.IsChecked = false;

                MessageBox.Show("Пользователь успешно добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя {ex.Message}");
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dbContext.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных {ex.Message}");
            }
        }
    }
}
