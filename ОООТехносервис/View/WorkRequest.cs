using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ОООТехносервис.Classes;

namespace ОООТехносервис.View
{
    public partial class WorkRequest : Form
    {
        // Переданная заявка
        Model.Request request;
        int requestID;      // Номер переданной заявки
        int mode;           // 0 - новая, 2 - редактирование мастером, 3 - редактирование оператором
        private int v;

        // Конструктор по умолчанию
        public WorkRequest()
        {
            InitializeComponent();
        }

        /// Конструктор с параметрами
        /// <param name="mode">Режим работы окна</param>
        /// <param name="requestID">0 или номер выбранной заявки для редактирования</param>
        public WorkRequest(int mode, int requestID)
        {
            InitializeComponent();
            this.mode = mode;
            this.requestID = requestID;

            // Если mode указывает на редактирование, ищем заявку по requestID
            if (mode != 0)
            {
                request = Helper.DB.Request.FirstOrDefault(r => r.RequestID == requestID);
                if (request == null)
                {
                    MessageBox.Show("Заявка с таким номером не найдена");
                    return;
                }
            }
            else
            {
                request = new Model.Request(); // Создать новую заявку
            }
        }

        public WorkRequest(int v)
        {
            this.v = v;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WorkRequest_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        /// Вспомогательный метод для заполнения списков из таблиц БД
        private void SetComboBox<T>(ComboBox comboBox, string displayMember, string valueMember, List<T> list)
        {
            comboBox.DataSource = list;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
            comboBox.Enabled = false;
        }

        /// При отображении формы подготовить все элементы
        private void WorkRequest_Shown(object sender, EventArgs e)
        {
            // Настройка списков из БД
            SetComboBox<Model.Equipment>(comboBoxEquipment, "EquipmentName", "EquipmentId", Helper.DB.Equipment.ToList());
            SetComboBox<Model.User>(comboBoxClient, "UserFullName", "UserId", Helper.DB.User.Where(u => u.UserRoleID == 1).ToList());
            SetComboBox<Model.User>(comboBoxMaster, "UserFullName", "UserId", Helper.DB.User.Where(u => u.UserRoleID == 2).ToList());
            SetComboBox<Model.Priority>(comboBoxPriority, "PriorityName", "PriorityId", Helper.DB.Priority.ToList());
            SetComboBox<Model.Stage>(comboBoxStage, "StageName", "StageId", Helper.DB.Stage.ToList());
            SetComboBox<Model.Status>(comboBoxStatus, "StatusName", "StatusId", Helper.DB.Status.ToList());
            SetComboBox<Model.Problem>(comboBoxProblem, "ProblemName", "ProblemId", Helper.DB.Problem.ToList());

            if (mode == 0)  // Новая заявка
            {
                // Настройка полей для новой заявки
                textBoxComment.Enabled = textBoxTime.Enabled = false;
                textBoxDate.Text = DateTime.Now.ToShortDateString(); // Заполняем дату
                comboBoxStatus.SelectedValue = 2; // Устанавливаем статус по умолчанию
                comboBoxStage.SelectedValue = 2; // Устанавливаем стадию по умолчанию
                textBoxTime.Text = "0"; // Устанавливаем время по умолчанию
                textBoxComment.Text = string.Empty; // Очищаем комментарий
            }
            else  // Редактирование выбранной заявки
            {
                if (request != null)
                {
                    // Заполняем данные редактируемой заявки
                    textBoxNumber.Text = requestID.ToString();
                    textBoxDate.Text = request.RequestDate.ToShortDateString();
                    comboBoxEquipment.SelectedValue = request.RequestEquipmentID;
                    comboBoxClient.SelectedValue = request.RequestUserID;
                    textBoxDescription.Text = request.RequestDescription;
                    comboBoxMaster.SelectedValue = request.RequestMasterID;
                    textBoxTime.Text = request.RequestTime.ToString();
                    comboBoxPriority.SelectedValue = request.RequestPriorityID;
                    comboBoxStatus.SelectedValue = request.RequestStatusID;
                    comboBoxStage.SelectedValue = request.RequestStageID;
                    textBoxComment.Text = request.RequestComment;
                    comboBoxProblem.SelectedValue = request.RequestProblemID;

                    // В зависимости от режима редактирования, настраиваем доступ к полям
                    if (mode == 3)  // Редактирует Оператор
                    {
                        // Отключаем все поля кроме comboBoxMaster
                        textBoxNumber.Enabled = false;
                        textBoxDate.Enabled = false;
                        comboBoxEquipment.Enabled = false;
                        comboBoxClient.Enabled = false;
                        textBoxDescription.Enabled = false;
                        textBoxTime.Enabled = false;
                        comboBoxPriority.Enabled = false;
                        comboBoxStatus.Enabled = false;
                        comboBoxStage.Enabled = false;
                        textBoxComment.Enabled = false;
                        comboBoxProblem.Enabled = false;
                        comboBoxMaster.Enabled = true; // Разрешаем редактирование только мастера
                    }
                    else  // Редактирует Мастер
                    {
                        textBoxDescription.Enabled = textBoxTime.Enabled = comboBoxStage.Enabled = textBoxComment.Enabled = true;
                    }
                }
            }
        }



        /// Сохранение данных в БД
        private void buttonFixed_Click(object sender, EventArgs e)
        {
            // Заполнение обязательных полей, кроме автоинкрементируемых
            request.RequestComment = textBoxComment.Text;
            request.RequestDate = Convert.ToDateTime(textBoxDate.Text);
            request.RequestDescription = textBoxDescription.Text;
            request.RequestEquipmentID = (int)comboBoxEquipment.SelectedValue;
            request.RequestUserID = (int)comboBoxClient.SelectedValue;
            request.RequestMasterID = (int)comboBoxMaster.SelectedValue;
            request.RequestProblemID = (int)comboBoxProblem.SelectedValue;
            request.RequestPriorityID = (int)comboBoxPriority.SelectedValue;
            request.RequestStageID = (int)comboBoxStage.SelectedValue;
            request.RequestStatusID = (int)comboBoxStatus.SelectedValue;
            request.RequestTime = Convert.ToInt32(textBoxTime.Text);

            if (mode == 0)  // Новая заявка
            {
                // Добавляем новую заявку в список
                Helper.DB.Request.Add(request);
            }

            try
            {
                // Сохранение изменений в БД
                Helper.DB.SaveChanges();
                MessageBox.Show("Данные успешно обновлены");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("При обновлении возникла ошибка: " + ex.Message);
            }
        }
    }
}
