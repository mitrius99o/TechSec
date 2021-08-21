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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox1.Text = Form1.updateAbiturientFIO;
            textBox2.Text = Form1.updateAbiturientSpec;
            if (Form1.updateAbiturientConsent)
                checkBox1.Checked = true;
            else
                checkBox1.Checked = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("UPDATE [Table] SET [FIO]=@FIO, [Specialization]=@Specialization, [Problems]=@Problems, [Consent]=@Consent WHERE [FIO]=@SelectedFIO", Form1.sqlConnection);//добавление нового элемента в таблицу

            command.Parameters.AddWithValue("SelectedFIO", Form1.updateAbiturientFIO);
            command.Parameters.AddWithValue("FIO", textBox1.Text);
            command.Parameters.AddWithValue("Specialization", textBox2.Text);
            command.Parameters.AddWithValue("Problems", (0).ToString());
            if (checkBox1.Checked)
                command.Parameters.AddWithValue("Consent", true);
            else
                command.Parameters.AddWithValue("Consent", false);

            await command.ExecuteNonQueryAsync();//читаем запрос и добавляем в таблицу
        }
    }
}
