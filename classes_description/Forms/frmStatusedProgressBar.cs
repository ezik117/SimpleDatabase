using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{
    public partial class frmStatusedProgressBar : Form
    {
        /// <summary>
        /// Указывает была ли нажата кнопка "Отмена"
        /// </summary>
        public bool abortRequested = false;

        /// <summary>
        /// Устанавливается в true в пользовательской задаче, когда она закончила работу и готова к закрытию формы
        /// </summary>
        public bool abortConfirmed = false;

        /// <summary>
        /// Действие (задача) выполняемое в рамках показа формы
        /// </summary>
        public Action<object> userTask;

        /// <summary>
        /// Токен отмены действия в выполняемой задаче "defultAction"
        /// </summary>
        public CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        /// Параметры, передаваемые в пользовательскую функцию. 
        /// Первый параметр всегда ссылка на форму frmStatusedProgressBar
        /// </summary>
        public List<object> userTaskParams = new List<object>();

        /// <summary>
        /// Результирующее сообщение которое будет показано пользователю после выполнения задачи
        /// </summary>
        public string resultMessage = "";

        /// <summary>
        /// Конструктор
        /// </summary>
        public frmStatusedProgressBar()
        {
            InitializeComponent();
            userTaskParams.Add(this);
        }

        /// <summary>
        /// Перерисовать форму
        /// </summary>
        public void UpdateControl()
        {
            this.Invalidate();
            Application.DoEvents();
        }

        /// <summary>
        /// Кнопка отмены действия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbort_Click(object sender, EventArgs e)
        {
            abortRequested = true;
            cts.Cancel();

            while (!abortConfirmed)
            {
                Application.DoEvents();
                Thread.Sleep(5);
            }

            this.Close();
        }

        /// <summary>
        /// Запуск пользовательской связанной задачи для формы и закрытие формы после ее выполнения
        /// </summary>
        private void frmStatusedProgressBar_Shown(object sender, EventArgs e)
        {
            abortConfirmed = false;
            Task.Factory.StartNew(userTask, userTaskParams, cts.Token).
                ContinueWith((task) => {
                    string rm = this.resultMessage;
                    if (rm != "Выполнено") MessageBox.Show(rm);
                    try
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Close();
                        });
                    }
                    catch { }
                });
        }

        /// <summary>
        /// Предотвращает закрытие формы по крестику. Только через кнопку Cancel.
        /// </summary>
        private void frmStatusedProgressBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !abortConfirmed;
        }

        /*
         ---- ШАБЛОН ПОЛЬЗОВАТЕЛЬСКОЙ ЗАДАЧИ -----
         
            public void SaveFileToGoogleDrive(object taskParams)
            {
                List<object> param = (List<object>)taskParams;
                frmStatusedProgressBar frm =  (frmStatusedProgressBar)param[0];
                frm.Invoke((MethodInvoker)delegate { frm.Text = "Сохранение в облаке"; });

                while (!frm.cts.IsCancellationRequested)
                {
                    if (frm.InvokeRequired)
                    {
                        frm.Invoke(
                            (MethodInvoker)delegate
                            {
                                frm.pb0.Value++;
                                Thread.Sleep(100);
                                if (frm.pb0.Value >= frm.pb0.Maximum) frm.pb0.Value = 0;
                            });
                    }
                    Thread.Sleep(10);
                }

                frm.resultMessage = "Выполнено";
                frm.abortConfirmed = true;
            }
        */
    }
}
