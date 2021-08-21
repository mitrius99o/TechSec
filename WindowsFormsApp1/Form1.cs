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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public static SqlConnection sqlConnection;
        public static DbSync dbSync;
        public static IQueryable<Abiturient> abiturients;
        public static string updateAbiturientFIO;
        public static string updateAbiturientSpec;
        public static bool updateAbiturientConsent;

        public FormStat stat;
        public Form2 update;
        public FormAbout about;
        public Form1()
        {
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = @"";//строка подключения,расположение бд
            sqlConnection = new SqlConnection(connectionString);//инициализация подключения
            await sqlConnection.OpenAsync();//открываем соединение с бд

            SqlDataReader sqlReader = null;//можем получить бд в табличном представлении

            DbSync.SetDbContext(@"");
            dbSync = new DbSync();

            abiturients = DbSync.db.GetTable<Abiturient>();
            DbSync.selectedGroup = DbSync.db.GetTable<Abiturient>();
            if(listBox1.Items.Count==0)
            {
                foreach(Abiturient user in abiturients)
                {
                    listBox1.Items.Add($"№{listBox1.Items.Count+1}. {user.FIO}  {user.Specialization}  {user.Problems}");
                }
            }
        }

        //кнопка добавления
        private async void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Table] (FIO, Specialization, Problems, Consent)VALUES(@FIO, @Specialization, @Problems, @Consent)", sqlConnection);//добавление нового элемента в таблицу
                command.Parameters.AddWithValue("FIO", textBox1.Text);
                command.Parameters.AddWithValue("Specialization", textBox2.Text);
                command.Parameters.AddWithValue("Problems", (0).ToString());
                if (checkBox1.Checked)
                    command.Parameters.AddWithValue("Consent", true);
                else
                    command.Parameters.AddWithValue("Consent", false);

                await command.ExecuteNonQueryAsync();//читаем запрос и добавляем в таблицу
            }
            else
            {
                MessageBox.Show("Поля ФИО и Группа длжны быть заполнены");
            }
            //обновление вывода на экран
            abiturients = null;
            abiturients = DbSync.db.GetTable<Abiturient>();
            DbSync.selectedGroup = DbSync.db.GetTable<Abiturient>();

            listBox1.Items.Clear();
            foreach (Abiturient user in abiturients)
            {
                listBox1.Items.Add($"№{listBox1.Items.Count + 1}. {user.FIO}  {user.Specialization}  {user.Problems}");
            }
        }

        //кнопка поиска
        private void button5_Click(object sender, EventArgs e)
        {
            DbSync.selectedGroup =
                abiturients.Where(x =>
                    x.FIO.StartsWith(textBox3.Text) ||
                    x.FIO.Contains(textBox3.Text) ||
                    x.Specialization==textBox3.Text);
            if(checkBox2.Checked)
            {
                DbSync.selectedGroup = DbSync.selectedGroup.Where(x => x.Consent == true);
            }
            listBox1.Items.Clear();
            foreach(Abiturient a in DbSync.selectedGroup)
            {
                listBox1.Items.Add($"№{listBox1.Items.Count + 1}. {a.FIO}  {a.Specialization}  {a.Problems}");
            }
        }

        //изменить статус дела
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        //показать статистику
        private void button2_Click(object sender, EventArgs e)
        {
            stat = new FormStat();
            stat.Show();
        }

        //удалить дело
        private async void button3_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem!=null)
            {
                SqlCommand command = new SqlCommand("DELETE FROM [Table] WHERE [FIO]=@FIO", sqlConnection);
                command.Parameters.AddWithValue("FIO", DbSync.selectedGroup.ToList()[listBox1.SelectedIndex].FIO);

                await command.ExecuteNonQueryAsync();
                MessageBox.Show(null, $"Удаление прошло успешно", "Cообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Выделите объект в списке для удаления");
            }
            abiturients = null;
            abiturients = DbSync.db.GetTable<Abiturient>();

            listBox1.Items.Clear();
            foreach (Abiturient user in abiturients)
            {
                listBox1.Items.Add($"№{listBox1.Items.Count + 1}. {user.FIO}  {user.Specialization}  {user.Problems}");
            }
            
        }
        //кнопка обновления
        private void button6_Click(object sender, EventArgs e)
        {
            DbSync.selectedGroup = DbSync.db.GetTable<Abiturient>();
            listBox1.Items.Clear();
            foreach (Abiturient user in abiturients)
            {
                listBox1.Items.Add($"№{listBox1.Items.Count + 1}. {user.FIO}  {user.Specialization}  {user.Problems}");
            }
        }
        //поиск в реальном времени изменения textBoxa
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            DbSync.selectedGroup = 
                abiturients.Where(
                    x => x.FIO.StartsWith(textBox3.Text) ||
                    x.FIO.Contains(textBox3.Text) ||
                    x.Specialization.Contains(textBox3.Text) ||
                    x.Specialization.StartsWith(textBox3.Text));

            if (checkBox2.Checked)
                DbSync.selectedGroup = DbSync.selectedGroup.Where(x => x.Consent == true);

            listBox1.Items.Clear();
            foreach (Abiturient a in DbSync.selectedGroup)
            {
                listBox1.Items.Add($"№{listBox1.Items.Count + 1}. {a.FIO}  {a.Specialization}  {a.Problems}");
            }
        }
        //изменение дела
        private async void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem!=null)
            {
                updateAbiturientFIO = DbSync.selectedGroup.ToList()[listBox1.SelectedIndex].FIO;
                updateAbiturientSpec = DbSync.selectedGroup.ToList()[listBox1.SelectedIndex].Specialization;
                updateAbiturientConsent = DbSync.selectedGroup.ToList()[listBox1.SelectedIndex].Consent;
                update = new Form2();
                update.Show();
            }
            else
            {
                if (listBox1.SelectedItem == null)
                    MessageBox.Show("Выделите нужную запись в списке для изменения");
            }
            abiturients = null;
            DbSync.selectedGroup = abiturients = DbSync.db.GetTable<Abiturient>();
            listBox1.Items.Clear();
            foreach (Abiturient user in abiturients)
            {
                listBox1.Items.Add($"№{listBox1.Items.Count + 1}. {user.FIO}  {user.Specialization}  {user.Problems}");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            DbSync.selectedGroup =
                abiturients.Where(
                    x => x.FIO.StartsWith(textBox3.Text) ||
                    x.FIO.Contains(textBox3.Text) ||
                    x.Specialization.Contains(textBox3.Text) ||
                    x.Specialization.StartsWith(textBox3.Text));

            if (checkBox2.Checked)
                DbSync.selectedGroup = DbSync.selectedGroup.Where(x => x.Consent == true);

            listBox1.Items.Clear();
            foreach (Abiturient a in DbSync.selectedGroup)
            {
                listBox1.Items.Add($"№{listBox1.Items.Count + 1}. {a.FIO}  {a.Specialization}  {a.Problems}");
            }
        }
        //кнопка о создателе
        private void button1_Click_1(object sender, EventArgs e)
        {
            about = new FormAbout();
            about.Show();
        }
    }
}
