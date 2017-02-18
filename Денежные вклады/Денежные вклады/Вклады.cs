using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Денежные_вклады
{
    public partial class Вклады : Form
    {
        int mode = 0;
        int cod;
        public Вклады() //Открытие формы для добавления, без аргументов, заполнение полей информацией.
        {
            InitializeComponent();
            FillComboBox(comboBox4, "Банк", "ID_Банк", "Название");
            FillComboBox(comboBox5, "Вкладчик", "ID_Вкладчик", "ФИО");
            FillComboBoxTime(comboBox1);
            FillComboBox(comboBox2, "Валюта", "ID_Валюта", "Валюта");
            PercentShow();
            maskedTextBox1.Text = DateTime.Now.ToString();
            mode = 1;
        }

        public Вклады(int id, string name, string summ, string time, string curr, string bank, string inv, string date) //Открыйтие формы ддя изменения с аргументами текущей записи
        {
            InitializeComponent();
            cod = id;
            textBox1.Text = name;
            textBox2.Text = summ;
            FillComboBox(comboBox2, "Валюта", "ID_Валюта", "Валюта");
            comboBox2.SelectedValue = curr;
            FillComboBox(comboBox4, "Банк", "ID_Банк", "Название");
            comboBox4.SelectedValue = bank;
            FillComboBox(comboBox5, "Вкладчик", "ID_Вкладчик", "ФИО");
            comboBox5.SelectedValue = inv;
            FillComboBoxTime(comboBox1);
            comboBox1.SelectedValue = time;
            maskedTextBox1.Text = date;
            PercentShow();
        }

        private void FillComboBox(ComboBox cb, string table, string poleID, string pole) //Заполняет комбобоксы
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT "+poleID+", "+pole+" FROM "+table+"";
            DataTable data_table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            sqlConnection1.Open();
            try
            {
                adapter.Fill(data_table);
                cb.DataSource = data_table;
                cb.DisplayMember = pole;
                cb.ValueMember = poleID;
            }
            finally
            {
                sqlConnection1.Close();
            }
        }

        private void FillComboBoxTime(ComboBox cb) //Заполняет комбобокс сроками
        {
            Otv();
            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT Срок, ID_Срок FROM Срок WHERE Срок.ID_Ответственность = '" + otv + "'";
            DataTable data_table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            sqlConnection1.Open();
            try
            {
                adapter.Fill(data_table);
                cb.DataSource = data_table;
                cb.DisplayMember = "Срок";
                cb.ValueMember = "ID_Срок";
            }
            finally
            {
                sqlConnection1.Close();
            }
        }
        int percent;
        private void Percent() //Вычисляет процент из записи
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            string a = comboBox1.SelectedValue.ToString();
            com.CommandText = "SELECT ID_Процент FROM Процент WHERE Процент.ID_Срок = '" + a + "'";
            sqlConnection1.Open();
            percent = (int)com.ExecuteScalar();
            sqlConnection1.Close();
        }
        int nalog;
        private void Nalog() //Вычисляет процент из записи
        {
            Otv();
            SqlCommand com = sqlConnection1.CreateCommand();
            string a = comboBox1.SelectedValue.ToString();
            com.CommandText = "SELECT ID_Налог FROM Налог WHERE Налог.ID_Ответственность = '" + otv + "'";
            sqlConnection1.Open();
            nalog = (int)com.ExecuteScalar();
            sqlConnection1.Close();
        }
        string otv;
        private void Otv() // Вычисляет вид ответственности из записи
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            string b = comboBox5.SelectedValue.ToString();
            com.CommandText = "SELECT ID_Ответственность FROM Вкладчик WHERE ID_Вкладчик = '" + b + "'";
            sqlConnection1.Open();
            otv = (string)com.ExecuteScalar();
            sqlConnection1.Close();
        }

        string percentShow;
        private void PercentShow() //Отображает процент, соответсвующий сроку
        {
            Percent();
            SqlCommand com = sqlConnection1.CreateCommand();
            string b = comboBox5.SelectedValue.ToString();
            com.CommandText = "SELECT Процент FROM Процент WHERE ID_Процент = '" + percent + "'";
            sqlConnection1.Open();
            percentShow = (string)com.ExecuteScalar();
            sqlConnection1.Close();
            label4.Text = percentShow+'%';
        }

        private void AddRecord() //Производит добавление в базу
        {
            Percent();
            Nalog();
            SqlCommand com = sqlConnection1.CreateCommand();
            string values = "@Наименование,@Сумма,@Срок,@Процент,@Валюта,@Банк,@Вкладчик,@Дата,@Налог";
            com.Parameters.Clear();
            com.CommandText = "INSERT INTO Вклад (Наименование, Сумма, ID_Срок, ID_Процент, ID_Валюта, ID_Банк, ID_Вкладчик, Дата_создания, ID_Налог) values (" + values + ")";
            com.Parameters.Add("Наименование", SqlDbType.NVarChar).Value = textBox1.Text;
            com.Parameters.Add("Сумма", SqlDbType.Money).Value = textBox2.Text;
            com.Parameters.Add("Срок", SqlDbType.NVarChar).Value = comboBox1.SelectedValue;
            com.Parameters.Add("Процент", SqlDbType.NVarChar).Value = percent;
            com.Parameters.Add("Валюта", SqlDbType.NVarChar).Value = comboBox2.SelectedValue;
            com.Parameters.Add("Банк", SqlDbType.NVarChar).Value = comboBox4.SelectedValue;
            com.Parameters.Add("Вкладчик", SqlDbType.NVarChar).Value = comboBox5.SelectedValue;
            com.Parameters.Add("Дата", SqlDbType.DateTime).Value = maskedTextBox1.Text;
            com.Parameters.Add("Налог", SqlDbType.NVarChar).Value = nalog;
            sqlConnection1.Open();
            try
            {
                DateTime dt = Convert.ToDateTime(maskedTextBox1.Text);
                if (dt > DateTime.Now)
                {
                    throw new DataException("Неверная дата!");
                }
                com.ExecuteNonQuery();
                MessageBox.Show("Добавлено!");
            }
            catch (FormatException)
            {
                MessageBox.Show("Заполните все поля");
            }
            catch (DataException)
            {
                MessageBox.Show("Неверная дата!");
            }
            finally
            {
                sqlConnection1.Close();
                this.Close();
            }
        }

        private void UpdateRecord() //Производит изменения в базе
        {
            Percent();
            Nalog();
            SqlCommand com = sqlConnection1.CreateCommand();
            com.Parameters.Clear();
            com.CommandText = "UPDATE Вклад SET Наименование = @Parametr, Сумма = @Parametr1, ID_Срок = @Parametr2, ID_Процент = @Parametr3, ID_Валюта = @Parametr4, ID_Банк = @Parametr6, ID_Вкладчик = @Parametr7, Дата_создания = @Parametr8, ID_Налог = @Parametr9  WHERE ID_Вклад = @cod";
            com.Parameters.Add("@cod", SqlDbType.Int);
            com.Parameters.Add("@Parametr", SqlDbType.NVarChar);
            com.Parameters.Add("@Parametr1", SqlDbType.Money);
            com.Parameters.Add("@Parametr2", SqlDbType.NVarChar);
            com.Parameters.Add("@Parametr3", SqlDbType.NVarChar);
            com.Parameters.Add("@Parametr4", SqlDbType.NVarChar);
            com.Parameters.Add("@Parametr6", SqlDbType.NVarChar);
            com.Parameters.Add("@Parametr7", SqlDbType.NVarChar);
            com.Parameters.Add("@Parametr8", SqlDbType.DateTime);
            com.Parameters.Add("@Parametr9", SqlDbType.NVarChar);

            com.Parameters["@cod"].Value = cod;
            if (MessageBox.Show("Изменить?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                com.Parameters["@Parametr"].Value = textBox1.Text;
                com.Parameters["@Parametr1"].Value = textBox2.Text;
                com.Parameters["@Parametr2"].Value = comboBox1.SelectedValue;
                com.Parameters["@Parametr3"].Value = percent;
                com.Parameters["@Parametr4"].Value = comboBox2.SelectedValue;
                com.Parameters["@Parametr6"].Value = comboBox4.SelectedValue;
                com.Parameters["@Parametr7"].Value = comboBox5.SelectedValue;
                com.Parameters["@Parametr8"].Value = maskedTextBox1.Text;
                com.Parameters["@Parametr9"].Value = nalog;
                sqlConnection1.Open();
                try
                {
                    DateTime dt = Convert.ToDateTime(maskedTextBox1.Text);
                    if (dt > DateTime.Now)
                    {
                        throw new DataException("Неверная дата!");
                    }
                    com.ExecuteNonQuery();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Заполните все поля");
                }
                catch (DataException)
                {
                    MessageBox.Show("Неверная дата!");
                }
                finally
                {
                    sqlConnection1.Close();
                    this.Close();
                }
            }
            else
                sqlConnection1.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mode == 1)
            {
                AddRecord();
            }
            else
                UpdateRecord();
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FillComboBoxTime(comboBox1);
            PercentShow();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PercentShow();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) //Защита от неправильного ввода
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
                return;
            if (Char.IsControl(e.KeyChar))
                return;
            if (e.KeyChar >= 'A' && e.KeyChar <= 'я')
                return;
            if (e.KeyChar == ' ')
                return;
            e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
                return;
            if (Char.IsControl(e.KeyChar))
                return;
            if (e.KeyChar == ',')
                return;
            e.Handled = true;
        }
    }
}
