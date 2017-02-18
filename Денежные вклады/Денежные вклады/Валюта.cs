using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Денежные_вклады
{
    public partial class Валюта : Form
    {
        int mode = 0;
        int cod;
        public Валюта(int id, string name, string val)  //Открыйтие формы ддя изменения с аргументами текущей записи
        {
            InitializeComponent();
            cod = id;
            textBox1.Text = name;
            textBox2.Text = val;
        }
        public Валюта()    //Открытие формы для добавления, без аргументов.
        {
            InitializeComponent();
            mode = 1;
        }
        private void AddRecord() //Производит добавление в базу
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            string values = "@Валюта, @Курс";
            com.Parameters.Clear();
            com.CommandText = "insert into Валюта (Валюта, Курс) values (" + values + ")";
            com.Parameters.Add("Валюта", SqlDbType.NVarChar).Value = textBox1.Text;
            com.Parameters.Add("Курс", SqlDbType.Money).Value = textBox2.Text;
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
            com.CommandText = "UPDATE Валюта SET Валюта = @Parametr, Курс = @Parametr1 WHERE ID_Валюта = @cod";
            com.Parameters.Add("@cod", SqlDbType.Int);
            com.Parameters.Add("@Parametr", SqlDbType.NVarChar);
            com.Parameters.Add("@Parametr1", SqlDbType.Money);
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
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
