using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ОООТехносервис.Classes
{
    public class Helper
    {
        //Объект для связи с БД
        public static Model.DBVakulenkoRequests413Entities DB { get; set; }
        //Объект-пользователь, вошедший в систему
        public static Model.User user;
    }

    
        public static class CustomMessageBox
        {
            public static void Show(string message, string title = "Информация")
            {
                Form messageBoxForm = new Form()
                {
                    Width = 400,
                    Height = 200,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = title,
                    StartPosition = FormStartPosition.CenterScreen,
                    Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath) // Иконка приложения
                };

                Label labelMessage = new Label()
                {
                    Left = 50,
                    Top = 30,
                    Width = 300,
                    Height = 50,
                    Text = message,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Button buttonOk = new Button()
                {
                    Text = "OK",
                    Left = 150,
                    Width = 100,
                    Top = 100,
                    DialogResult = DialogResult.OK,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular)
                };
                buttonOk.Click += (sender, e) => { messageBoxForm.Close(); };

                messageBoxForm.Controls.Add(labelMessage);
                messageBoxForm.Controls.Add(buttonOk);
                messageBoxForm.AcceptButton = buttonOk;

                messageBoxForm.ShowDialog();
            }
        }
    }


