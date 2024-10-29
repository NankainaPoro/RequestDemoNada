using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ОООТехносервис.Classes;

namespace ОООТехносервис.View
{
    public partial class ListRequests : Form
    {
        public ListRequests()
        {
            InitializeComponent();
        }

        private void ListRequests_Load(object sender, EventArgs e)
        {
            
            CenterToScreen();
            
        }


        //Создание списка
        List<Model.Request> requests = new List<Model.Request>();


        /// <summary>
        /// Кнопка "назад"
        ///; </summary>
        /// <param name="sender"></pa;ram>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

       

        private void ShowRequest()
        {

            //DVG настройки на отображение
            dataGridListReq.Rows.Clear();
            int i = 0;
            foreach (Model.Request request in requests)
            {
                dataGridListReq.Rows.Add();
                dataGridListReq.Rows[i].Cells[0].Value = request.RequestID.ToString();
                dataGridListReq.Rows[i].Cells[1].Value = request.RequestDate.ToShortDateString();
                dataGridListReq.Rows[i].Cells[2].Value = request.Equipment.EquipmentName.ToString();
                dataGridListReq.Rows[i].Cells[3].Value = request.User.UserFullName.ToString();
                dataGridListReq.Rows[i].Cells[4].Value = request.Status.StatusName.ToString();
                dataGridListReq.Rows[i].Cells[5].Value = request.User1.UserFullName.ToString();
                dataGridListReq.Rows[i].Cells[6].Value = request.Stage.StageName.ToString();
                i++;
            }

            requests = Helper.DB.Request.ToList();

            if (textBoxSerNum.Text.Length > 0 ) 
            {
                //Точное совпадение
                //requests = requests.Where(r => r.RequestID.ToString() == textBoxSerNum.Text).ToList();
                //Частичное совпадение
                requests = requests.Where(r => r.RequestID.ToString().Contains(textBoxSerNum.Text)).ToList();
            }

            //Фильтрация по статусу
            if ((int)comboBoxFilter.SelectedIndex != 0)
            {
                requests = requests.Where(r => r.RequestStatusID == (int)comboBoxFilter.SelectedIndex).ToList();
            }
            
            //Фильтрация по роли
            if (Helper.user.UserRoleID == 1)
            {
                requests = requests.Where(r => r.RequestUserID == Helper.user.UserID).ToList();
            }
            if (Helper.user.UserRoleID == 2)
            {
                requests = requests.Where(r => r.RequestMasterID == Helper.user.UserID).ToList();
            }


            btnNewReq.Visible = btnEditReq.Visible = btnReportReq.Visible = false;

            switch (Helper.user.UserRoleID)
            {
                case 1:
                    break;
                case 2:
                    btnEditReq.Visible = btnNewReq.Visible = true;
                    break;
                case 3:
                    btnEditReq.Visible = true;
                    break;
                case 4:
                    btnReportReq.Visible = true;
                    break;
            }

            //Настройка списка статусов
            List<Model.Status> statuses = Helper.DB.Status.ToList();
            Model.Status status = new Model.Status();
            status.StatusID = 0;
            status.StatusName = "Все статусы";
            statuses.Insert(0, status);
            comboBoxFilter.DataSource = statuses;
            comboBoxFilter.DisplayMember = "StatusName";
            comboBoxFilter.ValueMember = "StatusID";
           

        }

        private void ListRequests_Shown(object sender, EventArgs e)
        {
            
            ShowRequest();
        }

        /// <summary>
        /// Ввод номера для поиска
        /// </summary>
        /// <param name="sender"></pa;ram>
        /// <param name="e"></param>

        private void textBoxSerNum_TextChanged(object sender, EventArgs e)
        {
            ShowRequest();
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowRequest();
        }

        /// <summary>
        /// Новая заявка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewReq_Click(object sender, EventArgs e)
        {
            View.WorkRequest workRequest = new View.WorkRequest(0, 0); // передаем 0 для ID новой заявки
            Hide();
            workRequest.ShowDialog();
            Show();
        }


        /// <summary>
        /// Редактирование заявки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditReq_Click(object sender, EventArgs e)
        {
            int selectedRow = Convert.ToInt32(dataGridListReq.CurrentRow.Cells[0].Value);
            MessageBox.Show("Номер выбранной заявки: " + selectedRow);

            View.WorkRequest workRequest = new View.WorkRequest(Helper.user.Role.RoleID, selectedRow); // Передаем режим и ID заявки
            Hide();
            workRequest.ShowDialog();
            Show();
        }


        private void btnReportReq_Click(object sender, EventArgs e)
        {
            View.ReportRequest reportRequest = new View.ReportRequest();
            Hide();
            reportRequest.ShowDialog();
            Show();
        }
    }

}
