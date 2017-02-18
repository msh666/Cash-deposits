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
    public partial class Справочник : Form
    {
        int cod;
        int mode = 0;
        string tableName;
        string pole11;
        string pole22;
        public Справочник(string table, string pole1, string pole2)
        {
            InitializeComponent();
            label1.Text = pole2;
            label2.Text = pole1;
            comboBox1.Enabled = true;
            FillComboBox(comboBox1, pole2, "ID_" + pole2, pole2);
            mode = 1;
            tableName = table;
            pole11 = pole1;
            pole22 = pole2;
        }

        public Справочник(int id, string table2id, string pole, string pole1name, string pole2name)
        {
            InitializeComponent();
            label1.Text = pole2name;
            label2.Text = pole1name;
            FillComboBox(comboBox1, pole2name, "ID_" + pole2name, pole2name);
            comboBox1.SelectedText = table2id;
            comboBox1.Enabled = false;
            cod = id;
            textBox2.Text = pole;
            tableName = pole1name;
        }
        public Справочник (int time)
        {
            InitializeComponent();
            label1.Text = "Срок";
            label2.Text = "Процент";
            FillComboBox(comboBox1, "Срок", "ID_Срок", "Срок");
            comboBox1.SelectedValue = time;
            comboBox1.Enabled = false;
            tableName = "Процент";
            pole11 = "Процент";
            pole22 = "Срок";
            mode = 1;
        }

        private void FillComboBox(ComboBox cb, string table2, string poleID, string pole) //Заполняет комбобоксы
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT " + poleID + ", " + pole + " FROM " + table2 + "";
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

        private void AddRecord(string table, string pole1, string pole2) //Производит добавление в базу
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            string values = "@ID_"+pole2+", @"+pole1+"";
            com.Parameters.Clear();
            com.CommandText = "insert into "+table+" (ID_"+pole2+", "+pole1+") values (" + values + ")";
            com.Parameters.Add("ID_"+pole2+"", SqlDbType.NVarChar).Value = comboBox1.SelectedValue;
            com.Parameters.Add(pole1, SqlDbType.NVarChar).Value = textBox2.Text;
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

        private void UpdateRecord(string table, string pole1)  //Изменяет значения в базе
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.Parameters.Clear();
            com.CommandText = "UPDATE "+table+" SET "+pole1+" = @Parametr WHERE ID_"+pole1+" = @cod";
            com.Parameters.Add("@cod", SqlDbType.Int);
            com.Parameters.Add("@Parametr", SqlDbType.NVarChar);
            com.Parameters["@cod"].Value = cod;
            if (MessageBox.Show("Изменить?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                com.Parameters["@Parametr"].Value = textBox2.Text;

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
                AddRecord(tableName, pole11, pole22);
            }
            else
                UpdateRecord(tableName, tableName);
        }
    }
}
