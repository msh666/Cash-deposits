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
using System.IO;


namespace Денежные_вклады
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            изменитьToolStripMenuItem.Visible = false;
            удалитьToolStripMenuItem.Visible = false;
            добавитьToolStripMenuItem.Visible = false;
            графикToolStripMenuItem.Visible = false;
            maskedTextBox1.Text = DateTime.Now.ToString();
        }

        bool data_menu_visible;
        string CurrWork = null;


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)  //Изменяет состояние формы, в зависимости от выбранного поля в тривью
        {

            if (e.Node.Name == "Таблицы" || e.Node.Name == "Справочники")
            {
                data_menu_visible = true;
                dataGridView1.DataSource = null;
                button1.Enabled = false;
            }
            else
            {
                data_menu_visible = false;
                dataGridView1.DataSource = null;
            }
            menu_visible();
            if (e.Node.Name == "Вклады")
            {
                CurrWork = "Вклад";
                LoadDeposits();
                button1.Enabled = true;
                добавитьToolStripMenuItem.Visible = true;
                графикToolStripMenuItem.Visible = true;
                Rub();
                TotalSumm();
            }
            if (e.Node.Name == "Вкладчики")
            {
                CurrWork = "Вкладчик";
                LoadInvestors();
                button1.Enabled = false;
                добавитьToolStripMenuItem.Visible = true;
                графикToolStripMenuItem.Visible = false;
            }
            if (e.Node.Name == "Банки")
            {
                CurrWork = "Банк";
                LoadBanks();
                button1.Enabled = false;
                добавитьToolStripMenuItem.Visible = true;
                графикToolStripMenuItem.Visible = false;
            }
            if (e.Node.Name == "Срок")
            {
                CurrWork = "Срок";
                LoadDirectory("Срок","Ответственность");
                button1.Enabled = false;
                добавитьToolStripMenuItem.Visible = true;
                графикToolStripMenuItem.Visible = false;
            }
            if (e.Node.Name == "Процент")
            {
                CurrWork = "Процент";
                LoadDirectory("Процент", "Срок");
                button1.Enabled = false;
                добавитьToolStripMenuItem.Visible = false;
                графикToolStripMenuItem.Visible = false;
            }
            if (e.Node.Name == "Налог")
            {
                CurrWork = "Налог";
                LoadDirectory("Налог", "Ответственность");
                button1.Enabled = false;
                добавитьToolStripMenuItem.Visible = false;
                графикToolStripMenuItem.Visible = false;
            }
            if (e.Node.Name == "Валюта")
            {
                CurrWork = "Валюта";
                LoadCur();
                button1.Enabled = false;
                добавитьToolStripMenuItem.Visible = true;
                графикToolStripMenuItem.Visible = false;

            } 
        }
        public void menu_visible() //Делает выидимыми или невидимыми меню
        {
            if (data_menu_visible == true)
            {
                добавитьToolStripMenuItem.Visible = false;
                изменитьToolStripMenuItem.Visible = false;
                удалитьToolStripMenuItem.Visible = false;
                графикToolStripMenuItem.Visible = false;
            }
            else
            {
                изменитьToolStripMenuItem.Visible = true;
                добавитьToolStripMenuItem.Visible = true;

            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((CurrWork == "Валюта") && (dataGridView1.CurrentRow.Index == 0))
            {
                изменитьToolStripMenuItem.Visible = false;
                удалитьToolStripMenuItem.Visible = false;
            }
            else
            {
                if (CurrWork == "Налог")
                {
                    изменитьToolStripMenuItem.Visible = true;
                    удалитьToolStripMenuItem.Visible = false;
                }
                else
                {
                    изменитьToolStripMenuItem.Visible = true;
                    удалитьToolStripMenuItem.Visible = true;
                }
            }
        }
        double totalsumm;
        private void TotalSumm()
        {
            totalsumm = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                totalsumm += Convert.ToDouble(dataGridView1[4, i].Value);
            }
            label5.Text = totalsumm.ToString();
            label5.Visible = true;
        }
        private void LoadDeposits()  //Выводит таблицу Вкладов
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT ID_Вклад AS '№ Вклада', Вклад.Наименование, Сумма, Валюта.Валюта, Сумма_в_рублях, Срок.Срок, Процент.Процент, Налог.Налог, Начисленно, Дата_создания, Банк.Название, Вкладчик.ФИО, Вклад.ID_Срок, Вклад.ID_Валюта, Вклад.ID_Банк, Вклад.ID_Вкладчик, Вклад.ID_Налог FROM Банк INNER JOIN Вклад ON Банк.ID_Банк = Вклад.ID_Банк INNER JOIN Вкладчик ON Вклад.ID_Вкладчик = Вкладчик.ID_Вкладчик INNER JOIN Срок ON Срок.ID_Срок = Вклад.ID_Срок INNER JOIN Процент ON Процент.ID_Процент = Вклад.ID_Процент INNER JOIN Валюта ON Валюта.ID_Валюта = Вклад.ID_Валюта INNER JOIN Налог ON Вклад.ID_Налог = Налог.ID_Налог;";
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            sqlConnection1.Open();
            try
            {
                adapter.Fill(table);
            }
            finally
            {
                sqlConnection1.Close();
            }
            dataGridView1.DataSource = table;
            изменитьToolStripMenuItem.Visible = false;
            удалитьToolStripMenuItem.Visible = false;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].Visible = false;
            dataGridView1.Columns[15].Visible = false;
            dataGridView1.Columns[16].Visible = false;
            label2.Text = dataGridView1.RowCount.ToString();
            label2.Visible = true;
        }

        private void LoadInvestors()  //Выводит таблицу вкладчиков
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT ID_Вкладчик, ФИО, Дата_рождения, Ответственность, Вкладчик.ID_Ответственность FROM Вкладчик INNER JOIN Ответственность ON [Вкладчик].ID_Ответственность = [Ответственность].ID_Ответственность";
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            sqlConnection1.Open();
            try
            {
                adapter.Fill(table);
            }
            finally
            {
                sqlConnection1.Close();
            }
            dataGridView1.DataSource = table;
            изменитьToolStripMenuItem.Visible = false;
            удалитьToolStripMenuItem.Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void LoadBanks()  //Выводит таблицу банков
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT ID_Банк, Название, Адрес FROM Банк";
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            sqlConnection1.Open();
            try
            {
                adapter.Fill(table);
            }
            finally
            {
                sqlConnection1.Close();
            }
            dataGridView1.DataSource = table;
            изменитьToolStripMenuItem.Visible = false;
            удалитьToolStripMenuItem.Visible = false;
        }

        private void LoadDirectory(string table1, string table2)
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT ID_" + table1 + ", " + table2 + ", " + table1 + " FROM " + table1 + " INNER JOIN " + table2 + " ON " + table2 + ".ID_" + table2 + " = " + table1 + ".ID_" + table2 + "";
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            sqlConnection1.Open();
            try
            {
                adapter.Fill(table);
            }
            finally
            {
                sqlConnection1.Close();
            }
            dataGridView1.DataSource = table;
            изменитьToolStripMenuItem.Visible = false;
            удалитьToolStripMenuItem.Visible = false;
        }

        private void LoadCur()
        {

            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT ID_Валюта, Валюта, Курс FROM Валюта";
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            sqlConnection1.Open();
            try
            {
                adapter.Fill(table);
            }
            finally
            {
                sqlConnection1.Close();
            }
            dataGridView1.DataSource = table;
            изменитьToolStripMenuItem.Visible = false;
            удалитьToolStripMenuItem.Visible = false;
        }

        private void GetInfoToChangeBank()  //Считывает информацию необходимую для изменения
        {
            int id = (int)dataGridView1[0, dataGridView1.CurrentRow.Index].Value;
            string name = (string)dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            string adres = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            Банки UpdBank = new Банки(id, name, adres);
            UpdBank.ShowDialog();
        }

        private void GetInfoToChangeInvestors() //Считывает информацию необходимую для изменения
        {
            int id = (int)dataGridView1[0, dataGridView1.CurrentRow.Index].Value;
            string fio = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            string date = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            Вкладчики UpdInv = new Вкладчики(id, fio, date);
            UpdInv.ShowDialog();
        }

        private void GetInfoToChangeDeposits() //Считывает информацию необходимую для изменения
        {
            int id = (int)dataGridView1[0, dataGridView1.CurrentRow.Index].Value;
            string name = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            string summ = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            string time = dataGridView1[12, dataGridView1.CurrentRow.Index].Value.ToString();
            string date = dataGridView1[9, dataGridView1.CurrentRow.Index].Value.ToString();
            string curr = dataGridView1[13, dataGridView1.CurrentRow.Index].Value.ToString();
            string bank = dataGridView1[14, dataGridView1.CurrentRow.Index].Value.ToString();
            string inv = dataGridView1[15, dataGridView1.CurrentRow.Index].Value.ToString();

            Вклады UpdInv = new Вклады(id, name, summ, time, curr, bank, inv, date);
            UpdInv.ShowDialog();
        }
        private void GetInfoToChangeDirectory(string pole1name, string pole2name)  //Считывает информацию необходимую для изменения
        {
            int id = (int)dataGridView1[0, dataGridView1.CurrentRow.Index].Value;
            string table2id = (string)dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            string pole = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            Справочник UpdDir = new Справочник(id, table2id, pole, pole1name, pole2name);
            UpdDir.ShowDialog();
        }

        private void GetInfoToChangeCurr()  //Считывает информацию необходимую для изменения
        {
            int id = (int)dataGridView1[0, dataGridView1.CurrentRow.Index].Value;
            string name = (string)dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
            string val = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            Валюта UpdCurr = new Валюта(id, name, val);
            UpdCurr.ShowDialog();
        }

        public void DeleteRecord(string column, SqlDbType type) //Удаляет выбранную строку
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.Parameters.Clear();
            int del = (int)dataGridView1[0, dataGridView1.CurrentRow.Index].Value;
            com.CommandText = "DELETE FROM " + CurrWork + " WHERE " + column + " = @Code";
            com.Parameters.Add("@Code", type);
            com.Parameters["@Code"].Value = del;
            sqlConnection1.Open();
            try
            {
                com.ExecuteNonQuery();
            }
            finally
            {
                sqlConnection1.Close();
                изменитьToolStripMenuItem.Visible = false;
                удалитьToolStripMenuItem.Visible = false;
            }
        }
        int time1;
        public void TakeTimeID(int time)
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT MAX(ID_Срок) FROM Срок";
            sqlConnection1.Open();
            time1 = (int)com.ExecuteScalar();
            sqlConnection1.Close();
        }
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (CurrWork)
            {
                case "Банк":
                    Банки bk = new Банки();
                    bk.ShowDialog();
                    LoadBanks();
                    break;
                case "Вкладчик":
                    Вкладчики inv = new Вкладчики();
                    inv.ShowDialog();
                    LoadInvestors();
                    break;
                case "Вклад":
                    Вклады dep = new Вклады();
                    dep.ShowDialog();
                    LoadDeposits();
                    Rub();
                    LoadDeposits();
                    TotalSumm();
                    break;
                case "Срок":
                    Справочник dir = new Справочник("Срок", "Срок", "Ответственность");
                    dir.ShowDialog();
                    LoadDirectory("Срок", "Ответственность");
                    TakeTimeID(time1);
                    Справочник dir1 = new Справочник(time1);
                    dir1.ShowDialog();
                    break;
                case "Налог":
                    Справочник dir2 = new Справочник("Налог", "Налог", "Ответственность");
                    dir2.ShowDialog();
                    LoadDirectory("Налог", "Ответственность");
                    break;
                case "Валюта":
                    Валюта curr = new Валюта();
                    curr.ShowDialog();
                    LoadCur();
                    break;
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (CurrWork)
            {
                case "Банк":
                    GetInfoToChangeBank();
                    LoadBanks();
                    break;
                case "Вкладчик":
                    GetInfoToChangeInvestors();
                    LoadInvestors();
                    break;
                case "Вклад":
                    GetInfoToChangeDeposits();
                    LoadDeposits();
                    Rub();
                    LoadDeposits();
                    TotalSumm();
                    break;
                case "Срок":
                    GetInfoToChangeDirectory("Срок", "Ответственность");
                    LoadDirectory("Срок", "Ответственность");
                    break;
                case "Процент":
                    GetInfoToChangeDirectory("Процент", "Срок");
                    LoadDirectory("Процент", "Срок");
                    break;
                case "Налог":
                    GetInfoToChangeDirectory("Налог", "Ответственность");
                    LoadDirectory("Налог", "Ответственность");
                    break;
                case "Валюта":
                    GetInfoToChangeCurr();
                    LoadCur();
                    break;
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (CurrWork)
            {
                case "Банк":
                    DeleteRecord("ID_Банк", SqlDbType.Int);
                    LoadBanks();
                    break;
                case "Вкладчик":
                    DeleteRecord("ID_Вкладчик", SqlDbType.Int);
                    LoadInvestors();
                    break;
                case "Вклад":
                    DeleteRecord("ID_Вклад", SqlDbType.Int);
                    LoadDeposits();
                    TotalSumm();
                    break;
                case "Срок":
                    DeleteRecord("ID_Срок", SqlDbType.Int);
                    LoadDirectory("Срок", "Ответственность");
                    break;
                case "Процент":
                    DeleteRecord("ID_Процент", SqlDbType.Int);
                    LoadDirectory("Процент", "Срок");
                    break;
                case "Налог":
                    DeleteRecord("ID_Налог", SqlDbType.Int);
                    LoadDirectory("Налог", "Ответственность");
                    break;
                case "Валюта":
                    DeleteRecord("ID_Валюта", SqlDbType.Int);
                    LoadCur();
                    break;
            }
        }
        double curr;
        private void Currency(int Idcurr) //Вычисляет процент из записи
        {
            SqlCommand com = sqlConnection1.CreateCommand();
            com.CommandText = "SELECT Курс FROM Валюта WHERE Валюта.ID_Валюта = '" + Idcurr + "'";
            sqlConnection1.Open();
            curr = Convert.ToDouble(com.ExecuteScalar());
            sqlConnection1.Close();
        }
        private void Rub()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                int id = (int)dataGridView1[0, i].Value;
                int cur = (int)dataGridView1[13, i].Value;
                string summ = dataGridView1[2, i].Value.ToString();
                double rub = 0;
                Currency(cur);   //В зависимости от того, в какой валюте хранятся средсва, так и будут расчитываться вклады в рублях    
                double summ1 = Convert.ToDouble(summ);
                rub = summ1 * curr;
                SqlCommand com = sqlConnection1.CreateCommand();
                com.Parameters.Clear();
                com.CommandText = "UPDATE Вклад SET Сумма_в_рублях = @Parametr1 WHERE ID_Вклад = @id";
                com.Parameters.Add("@id", SqlDbType.Int);
                com.Parameters.Add("@Parametr1", SqlDbType.Money);
                com.Parameters["@id"].Value = id;
                double nach = summ1;
                com.Parameters["@Parametr1"].Value = rub;
                sqlConnection1.Open();
                try
                {
                    com.ExecuteNonQuery();
                }
                finally
                {
                    sqlConnection1.Close();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)  //Расчитывает изменения по курсу и времени
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                int id = (int)dataGridView1[0, i].Value;
                string summ = dataGridView1[2, i].Value.ToString();
                string date = dataGridView1[9, i].Value.ToString();
                string proc = dataGridView1[6, i].Value.ToString();
                string nalog = dataGridView1[7, i].Value.ToString();
                int cur = (int)dataGridView1[13, i].Value;
                double nalog1 = 1 - (Convert.ToDouble(nalog)) / 100;
                double proc1 = (Convert.ToDouble(proc)) / 1200 + 1;
                DateTime convDate = Convert.ToDateTime(date);
                string date1 = maskedTextBox1.Text;
                DateTime convDate1 = Convert.ToDateTime(date1);
                System.TimeSpan fin = convDate1 - convDate;
                System.TimeSpan ts = new TimeSpan(0, 0, 0, 0);
                if (fin < ts)
                {
                    fin = ts;
                }
                string strfin = fin.ToString("dd");
                int strfin1 = Convert.ToInt32(strfin) / 30;
                double summ1 = Convert.ToDouble(summ);             
                SqlCommand com = sqlConnection1.CreateCommand();
                com.Parameters.Clear();
                com.CommandText = "UPDATE Вклад SET Начисленно = @Parametr WHERE ID_Вклад = @id";
                com.Parameters.Add("@id", SqlDbType.Int);
                com.Parameters.Add("@Parametr", SqlDbType.Money);
                com.Parameters["@id"].Value = id;
                double nach = summ1;
                for (int j = 1; j <= strfin1; j++)
                {
                    nach = nach * proc1;
                }
                com.Parameters["@Parametr"].Value = (nach-summ1)*nalog1;  //Расчеты по выплатам в зависимости от даты исходя, что месяц это 30 дней.
                sqlConnection1.Open();
                try
                {
                    com.ExecuteNonQuery();
                }
                                finally
                {
                    sqlConnection1.Close();
                }
                LoadDeposits();
            }
        }
        int month;
        int[] mas = null;
        public void TakeMonth()
        {
            mas = new int[12];
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                string date = dataGridView1[9, i].Value.ToString();
                SqlCommand com = sqlConnection1.CreateCommand();
                com.CommandText = "SELECT MONTH('" + date + "')";
                sqlConnection1.Open();
                month = Convert.ToInt32(com.ExecuteScalar());
                sqlConnection1.Close();
                mas[month-1]++;
            }
        }

        private void CorrectNumber(object sender, KeyPressEventArgs e) //Отвечает за правильный ввод данных
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
                return;
            if (Char.IsControl(e.KeyChar))
                return;
            if (e.KeyChar == ',')
                return;
            e.Handled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            CorrectNumber(sender,e);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            CorrectNumber(sender, e);
        }
        private void графикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TakeMonth();
            Graphic gr = new Graphic(this.mas);
            gr.ShowDialog();
        }
    }
}
