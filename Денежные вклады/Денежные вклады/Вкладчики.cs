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
    public partial class Вкладчики : Form
    {
        int mode = 0;
        int cod;

        public Вкладчики() //Открытие формы для добавления, без аргументов, заполнение полей информацией.
        {
            InitializeComponent();
            comboBox1.Visible = true;
            label3.Visible = true;
            FillComboBox(comboBox1);
            mode = 1;
        }

        public Вкладчики(int id, string fio, string date) //Открыйтие формы ддя изменения с аргументами текущей записи
        {
            InitializeComponent();
            cod = id;
            textBox1.Text = fio;
            maskedTextBox1.Text = date;
            comboBox1.Visible = false;
            label3.Visible = false;
        }

        private void FillComboBox(ComboBox cb) //Заполняет комбобоксы
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT ID_Ответственность, Ответственность FROM Ответственность";
            DataTable data_table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            sqlConnection1.Open();
            try
            {
                adapter.Fill(data_table);
                cb.DataSource = data_table;
                cb.DisplayMember = "Ответственность";
                cb.ValueMember = "ID_Ответственность";
            }
            finally
            {
                sqlConnection1.Close();
            }
        }

        private void AddRecord() //Производит добавление в базу
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            string values = "@ФИО,@Дата_рождения,@ID_Ответственность";
            com.Parameters.Clear();
            com.CommandText = "INSERT INTO Вкладчик (ФИО, Дата_рождения, ID_Ответственность) values (" + values + ")";
            com.Parameters.Add("ФИО", SqlDbType.NVarChar).Value = textBox1.Text;
            com.Parameters.Add("Дата_рождения", SqlDbType.Date).Value = maskedTextBox1.Text;
            com.Parameters.Add("ID_Ответственность", SqlDbType.NVarChar).Value = comboBox1.SelectedValue;
            sqlConnection1.Open();
            DateTime dt = DateTime.Now;
            try
            {
                dt = Convert.ToDateTime(maskedTextBox1.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Не верная дата!");
            }
            try
            {
                if (dt > DateTime.Now)
                {
                    throw new Exception("Неверная дата рождения!"); //Проверка. Если введенная дата больше текущей, то ошибка
                }
                com.ExecuteNonQuery();
                MessageBox.Show("Добавлено!");
            }
            catch (FormatException)
            {
                MessageBox.Show("Заполните все поля");
            }
            catch (Exception)
            {
                MessageBox.Show("Неверная дата рождения!");
            }
            finally
            {
                sqlConnection1.Close();
                this.Close();
            }
        }

        private void UpdateRecord() //Производит изменения в базе
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.Parameters.Clear();
            com.CommandText = "UPDATE Вкладчик SET ФИО = @Parametr, Дата_рождения = @Parametr1 WHERE ID_Вкладчик = @cod";
            com.Parameters.Add("@cod", SqlDbType.Int);
            com.Parameters.Add("@Parametr", SqlDbType.NVarChar);
            com.Parameters.Add("@Parametr1", SqlDbType.DateTime);
            com.Parameters["@cod"].Value = cod;
            if (MessageBox.Show("Изменить?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                com.Parameters["@Parametr"].Value = textBox1.Text;
                com.Parameters["@Parametr1"].Value =  maskedTextBox1.Text;
                sqlConnection1.Open();
                DateTime dt = DateTime.Now;
                try
                {
                   dt = Convert.ToDateTime(maskedTextBox1.Text);
                }
                catch(FormatException)
                {
                    MessageBox.Show("Не верная дата!");
                }
                try
                {
                    if (dt > DateTime.Now)
                    {
                        throw new Exception("Неверная дата рождения!");
                    }
                    com.ExecuteNonQuery();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Заполните все поля");
                }
                catch (Exception)
                {
                    MessageBox.Show("Неверная дата рождения!");
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)  //Защита от неправильного ввода
        {

            if (Char.IsControl(e.KeyChar))
                return;
            if (e.KeyChar >= 'A' && e.KeyChar <= 'я')
                return;
            if (e.KeyChar == ' ')
                return;
            e.Handled = true;
        }     
    }
}
