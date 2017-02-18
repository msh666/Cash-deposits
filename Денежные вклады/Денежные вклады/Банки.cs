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
    public partial class Банки : Form
    {
        int mode = 0;
        int cod;
        public Банки(int id, string name, string adres)  //Открыйтие формы ддя изменения с аргументами текущей записи
        {
            InitializeComponent();
            cod = id;
            textBox1.Text = name;
            textBox2.Text = adres;
        }
        public Банки()    //Открытие формы для добавления, без аргументов.
        {
            InitializeComponent();
            mode = 1;
        }
        private void AddRecord() //Производит добавление в базу
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            string values = "@Название, @Адрес";
            com.Parameters.Clear();
            com.CommandText = "insert into Банк (Название, Адрес) values (" + values + ")";
            com.Parameters.Add("Название", SqlDbType.NVarChar).Value = textBox1.Text;
            com.Parameters.Add("Адрес", SqlDbType.NVarChar).Value = textBox2.Text;
            sqlConnection1.Open();
            try
            {
                com.ExecuteNonQuery();
                MessageBox.Show("Добавлено!");
            }
            catch (Exception)
            {
                MessageBox.Show("Вы заполнили не все поля!");
            }
            finally
            {
                sqlConnection1.Close();
                this.Close();
            }

        }

        private void UpdateRecord()  //Изменяет значения в базе
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.Parameters.Clear();
            com.CommandText = "UPDATE Банк SET Название = @Parametr, Адрес = @Parametr1 WHERE ID_Банк = @cod";
            com.Parameters.Add("@cod", SqlDbType.Int);
            com.Parameters.Add("@Parametr", SqlDbType.NVarChar);
            com.Parameters.Add("@Parametr1", SqlDbType.NVarChar);
            com.Parameters["@cod"].Value = cod;
            if (MessageBox.Show("Изменить?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                com.Parameters["@Parametr"].Value = textBox1.Text;
                com.Parameters["@Parametr1"].Value = textBox2.Text;

                sqlConnection1.Open();
                try
                {
                    com.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    MessageBox.Show("Вы заполнили не все поля!");
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

        private void CorrectText(object sender, KeyPressEventArgs e) //Отвечает за правильный ввод данных
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
                return;
            if (Char.IsControl(e.KeyChar))
                return;
            if (e.KeyChar == ',')
                return;
            if (e.KeyChar >= 'A' && e.KeyChar <= 'я')
                return;
            if (e.KeyChar == ' ')
                return;
            e.Handled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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
            if (e.KeyChar == '.')
                return;
            if (e.KeyChar == '/')
                return;
            if (e.KeyChar >= 'A' && e.KeyChar <= 'я')
                return;
            if (e.KeyChar == ' ')
                return;
            e.Handled = true;
        }
    }
}
