using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormStat : Form
    {
        public FormStat()
        {
            InitializeComponent();
        }

        

        private void FormStat_Load(object sender, EventArgs e)
        {
            List<string> specializations = new List<string>();
            int count = 0;
            int countCons = 0;

            foreach (Abiturient a in Form1.abiturients)
                if (!specializations.Contains(a.Specialization))
                    specializations.Add(a.Specialization);

            for (int i = 0; i < specializations.Count; i++)
            {
                foreach (Abiturient a in Form1.abiturients)
                {
                    if (specializations[i] == a.Specialization)
                    {
                        count++;
                        countCons = a.Consent == true ? countCons + 1 : countCons;
                    }
                }
                chart1.Series[0].Points.Add(count);
                chart1.Series[0].Points.Last().LegendText = 
                    specializations[i]+" "+(count*100/Form1.abiturients.Count()).ToString()+"% ---"+count.ToString()+" чел.";
                chart2.Series[1].Points.AddXY(specializations[i]+$" ///{count} всего, {countCons} согл.", count-countCons);
                chart2.Series[0].Points.AddXY(specializations[i] + $" ///{count} всего, {countCons} согл.", countCons);
                count = 0;
                countCons = 0;
            }
            chart2.Series[0].LegendText = "С согласиями";
            chart2.Series[1].LegendText = "Все абитуриенты";
            label2.Text = $"Всего обработано дел {Form1.abiturients.Count()} на момент {DateTime.Now.ToString()}";
        }
    }
}
