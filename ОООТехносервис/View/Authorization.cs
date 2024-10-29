using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ОООТехносервис.Classes;

namespace ОООТехносервис
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();

            // Установка связи с БД
            try
            {
                Helper.DB = new Model.DBVakulenkoRequests413Entities();
                CustomMessageBox.Show("Связь с БД установлена", "Успех");
            }
            catch
            {
                CustomMessageBox.Show("Связь с БД не установлена", "Ошибка");
            }
        }

        /// <summary>
        /// Кастомный MessageBox для отображения сообщений
        /// </summary>
        //private void ShowCustomMessageBox(string message, string title)
        //{
        //    Form messageBoxForm = new Form()
        //    {
        //        Width = 400,
        //        Height = 200,
        //        FormBorderStyle = FormBorderStyle.FixedDialog,
        //        Text = title,
        //        StartPosition = FormStartPosition.CenterScreen,
        //        Icon = this.Icon  // Использует иконку формы Authorization
        //    };

        //    Label labelMessage = new Label()
        //    {
        //        Left = 50,
        //        Top = 30,
        //        Width = 300,
        //        Height = 50,
        //        Text = message,
        //        Font = new Font("Segoe UI", 10, FontStyle.Regular),
        //        TextAlign = ContentAlignment.MiddleCenter
        //    };

        //    Button buttonOk = new Button()
        //    {
        //        Text = "OK",
        //        Left = 150,
        //        Width = 100,
        //        Top = 100,
        //        DialogResult = DialogResult.OK,
        //        Font = new Font("Segoe UI", 9, FontStyle.Regular)
        //    };
        //    buttonOk.Click += (sender, e) => { messageBoxForm.Close(); };

        //    messageBoxForm.Controls.Add(labelMessage);
        //    messageBoxForm.Controls.Add(buttonOk);
        //    messageBoxForm.AcceptButton = buttonOk;
        //    messageBoxForm.ShowDialog();
        //}

        /// <summary>
        /// Завершение приложения
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            // Вход в систему
            Helper.user = Helper.DB.User.FirstOrDefault(u =>
                u.UserLogin == textBoxLogin.Text && u.UserPassword == textBoxPassword.Text);

            if (Helper.user != null)
            {
                CustomMessageBox.Show($"{Helper.user.UserFullName}, вы вошли с ролью {Helper.user.Role.RoleName}", "Успех");

                // Переход к окну заявок
                View.ListRequests listRequests = new View.ListRequests();
                Hide();
                listRequests.ShowDialog();
                Show();
            }
            else
            {
                CustomMessageBox.Show("Пользователь не найден", "Ошибка");
            }
        }

        private void Authorization_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }
    }
}
