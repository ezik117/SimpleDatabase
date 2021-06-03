using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace classes_description
{
    using SqlRows = List<Dictionary<string, object>>;

    public partial class Form1 : Form
    {
        public PropertyDescription propDescr;
        public PropertyDescription classDescr;
        public Database db;

        public Form1()
        {
            InitializeComponent();

            propDescr = new PropertyDescription(btnDescSave, tbDescEdit);
            classDescr = new PropertyDescription(btnClassDescSave, tbClassDescEdit);
            db = new Database();
            db.OpenOrCreate("classes");

            ClassItem.Load(this);
            
        }

        // Добавить класс
        private void btnClassAdd_Click(object sender, EventArgs e)
        {
            ClassItem.Create(this);
        }

        // Сохранить описание свойства
        private void btnDescSave_Click(object sender, EventArgs e)
        {
            PropertyItem.UpdateDescription(this);
        }

        // Сохранить описание класса
        private void btnClassDescSave_Click(object sender, EventArgs e)
        {
            ClassItem.Update(this);
        }

        // Выбран другой класса. Считать связанные данные.
        private void tvClasses_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClassItem.NodeChanged(this);
        }

        // Редактировать название класса.
        private void btnClassEdit_Click(object sender, EventArgs e)
        {
            ClassItem.Edit(this);
        }

        // Удалить класс
        private void btnClassDel_Click(object sender, EventArgs e)
        {
            ClassItem.Delete(this);
        }

        // Добавить свойство класса
        private void btnPropAdd_Click(object sender, EventArgs e)
        {
            PropertyItem.Create(this);
        }

        // Удалить свойство и подсвойства
        private void btnPropDel_Click(object sender, EventArgs e)
        {

            PropertyItem.Delete(this);
        }

        // Редактировать свойство
        private void btnPropEdit_Click(object sender, EventArgs e)
        {
            PropertyItem.Update(this);
        }

        // Свернуть / развернуть список
        private void btnPropCollapseExpand_Click(object sender, EventArgs e)
        {
            if (btnPropCollapseExpand.ImageIndex == (long)PropTypes.CollapseAll)
            {
                btnPropCollapseExpand.ImageIndex = (int)PropTypes.ExpandAll;
                tvProps.CollapseAll();
            }
            else
            {
                btnPropCollapseExpand.ImageIndex = (int)PropTypes.CollapseAll;
                tvProps.ExpandAll();
            }
        }

        // Выбрано другое свойство. Считать связанные данные.
        private void tvProps_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PropertyItem.PropertyChanged(this);
        }

        // Нажата клавиша в окне описания свойства. Определить действия
        private void tbDescEdit_KeyDown(object sender, KeyEventArgs e)
        {
            // CTRL-S. Сохраним данные.
            if (e.Control && e.KeyCode == Keys.S)
            {
                PropertyItem.UpdateDescription(this);
                e.Handled = true;
                return;
            }

            // CTRL-C. Скопируем в буфер обмена.
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject dto = new DataObject();
                dto.SetText(tbDescEdit.SelectedRtf, TextDataFormat.Rtf);
                dto.SetText(tbDescEdit.SelectedText, TextDataFormat.UnicodeText);
                Clipboard.Clear();
                Clipboard.SetDataObject(dto);
                e.Handled = true;
                return;
            }
        }

        // Нажата клавиша в окне описания класса. Определить действия
        private void tbClassDescEdit_KeyDown(object sender, KeyEventArgs e)
        {
            // CTRL-S. Сохраним данные.
            if (e.Control && e.KeyCode == Keys.S)
            {
                ClassItem.Update(this);
                e.Handled = true;
                return;
            }

            // CTRL-C. Скопируем в буфер обмена.
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject dto = new DataObject();
                dto.SetText(tbClassDescEdit.SelectedRtf, TextDataFormat.Rtf);
                dto.SetText(tbClassDescEdit.SelectedText, TextDataFormat.UnicodeText);
                Clipboard.Clear();
                Clipboard.SetDataObject(dto);
                e.Handled = true;
                return;
            }
        }

        // Считать повторно описание класса.
        private void btnClassDescReload_Click(object sender, EventArgs e)
        {
            ClassItem.DescriptionReload(this);
        }

        // Считать повторно описание свойства.
        private void btnDescReload_Click(object sender, EventArgs e)
        {
            PropertyItem.DescriptionReload(this);
        }

        // Проверка несохраненных данных при закрытии формы.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = ClassItem.CheckForUnsavedDesc(this);
            if (e.Cancel) return;
            e.Cancel = PropertyItem.CheckForUnsavedDesc(this);
        }

        // Меняется выбор свойства. Проверить на сохранение свойства.
        private void tvProps_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = PropertyItem.CheckForUnsavedDesc(this);
        }

        // Меняется выбор класса. Проверить на сохранение описаний класса и свойства.
        private void tvClasses_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = ClassItem.CheckForUnsavedDesc(this);
            if (e.Cancel) return;
            e.Cancel = PropertyItem.CheckForUnsavedDesc(this);
        }

        // Тест описания свойства изменен.
        private void tbDescEdit_TextChanged(object sender, EventArgs e)
        {
            propDescr.TextChanging();
        }

        // Текст описания класса изменен.
        private void tbClassDescEdit_TextChanged(object sender, EventArgs e)
        {
            classDescr.TextChanging();
        }

        // Добавим пункты в системное меню
        private void Form1_Load(object sender, EventArgs e)
        {
            SystemMenu.AddItem(this);
        }
    }
}
