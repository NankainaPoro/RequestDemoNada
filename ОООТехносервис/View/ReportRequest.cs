using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ОООТехносервис.View
{
    public partial class ReportRequest : Form
    {
        public ReportRequest()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ReportRequest_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            // Подсчет выполненных заявок
            var completedRequests = Classes.Helper.DB.Request.Where(r => r.RequestStatusID == 3);
            this.labelCount.Text = "Количество выполненных заявок = " + completedRequests.Count().ToString();

            if (completedRequests.Any())
            {
                this.labelAvg.Text = "Среднее время выполненных заявок (часов) = " + (completedRequests.Average(r => r.RequestTime) / 60.0).ToString("F2");
            }
            else
            {
                this.labelAvg.Text = "Среднее время выполненных заявок (часов) = 0.00";
            }

            // Заполнение DataGridView
            this.dataGridReport.DataSource = Classes.Helper.DB.GroupProblem.ToList();
            this.dataGridReport.Columns[0].HeaderText = "Тип неисправности";
            this.dataGridReport.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridReport.Columns[1].HeaderText = "Количество неисправности";
            this.dataGridReport.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

    }
}
