using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MiniCSharpUtils
{
    public partial class FrmMain : Form
    {
        private int count = 0;
        Random rand;
        private char[] specialChars = new[] {'!', '%', '*', ')', '(', '?', '#', '$', '^', '$', '~'};
        private Dictionary<string, double> metrica; 

        public FrmMain()
        {
            InitializeComponent();
            rand = new Random();
            metrica = new Dictionary<string, double>();
            metrica.Add("mm", 1);
            metrica.Add("cm", 10);
            metrica.Add("dm", 100);
            metrica.Add("m", 1000);
            metrica.Add("km", 1000000);
            metrica.Add("mile", 1609344);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            LoadNotepad();
            clbPassword.SetItemChecked(0, true);
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Программа \"Мои утилиты\", \nсодержит ряд небольших программ, \nкоторые могут пригодится в жизни. \nА главное - научить меня основам программирования на С#. \nАвтор: Бурмистров Д.В.", "О программе");
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            count++;
            lblCount.Text = count.ToString();
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            count--;
            lblCount.Text = count.ToString();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            count = 0;
            lblCount.Text = Convert.ToString(count); // в качестве учебных целей
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            int n;
            n = rand.Next(Convert.ToInt32(nudFrom.Value), Convert.ToInt32(nudTo.Value) + 1);
            lblRandom.Text = n.ToString();
            if (chkbRandom.Checked)
            {
                int i = 0;
                // автоперебор без повторений
                while (tbRandom.Text.IndexOf(n.ToString()) != -1)
                {
                    n = rand.Next(Convert.ToInt32(nudFrom.Value), Convert.ToInt32(nudTo.Value) + 1);
                    i++;
                    // если попыток больше 100 то прерываем перебор
                    if (i > 100) break;
                }
                if (i <= 100) tbRandom.AppendText(n + "\n");
            }
            else tbRandom.AppendText(n + "\n");
            
        }

        private void btnRandomClear_Click(object sender, EventArgs e)
        {
            tbRandom.Clear();
        }

        private void btnRandomCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbRandom.Text);
        }

        private void tsmiInsertDate_Click(object sender, EventArgs e)
        {
            rtbNotepad.AppendText(DateTime.Now.ToShortDateString() + "\n");
        }

        private void tsmiInsertTime_Click(object sender, EventArgs e)
        {
            rtbNotepad.AppendText(DateTime.Now.ToShortTimeString() + "\n");
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            try
            {
                rtbNotepad.SaveFile("notepad.rtf", RichTextBoxStreamType.RichText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении");
            }
        }

        private void LoadNotepad()
        {
            try
            {
                rtbNotepad.LoadFile("notepad.rtf", RichTextBoxStreamType.RichText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при загрузке");
            }
        }

        private void tsmiLoad_Click(object sender, EventArgs e)
        {
            LoadNotepad();
        }

        private void btnCreatePassword_Click(object sender, EventArgs e)
        {
            if (clbPassword.CheckedItems.Count == 0) return;
            string password = "";
            for (int i = 0; i < nudPathLength.Value; i++)
            {
                int n = rand.Next(0, clbPassword.CheckedItems.Count);
                string s = clbPassword.CheckedItems[n].ToString();
                switch (s)
                {
                    case "Цифры":
                        password += rand.Next(10); // генерит числа от 0 до 9
                        break;
                    case "Прописные буквы":
                        // Согласно таблице ASCII кодов с 65 по 90 находятся прописные буквы
                        password += Convert.ToChar(rand.Next(65, 91)); // 91, так как последний предел исключается
                        break;
                    case "Строчные буквы":
                        // Согласно таблице ASCII кодов с 97 по 122 находятся строчные буквы
                        password += Convert.ToChar(rand.Next(97, 123)); // 123, так как последний предел исключается
                        break;
                    default: // спец.символы
                        password += specialChars[rand.Next(specialChars.Length)]; // specialChars.Length, так как последний предел исключается
                        break;
                }
            }
            tbPassword.Text = password;
            Clipboard.SetText(password);
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            Converter();
        }

        private void Converter()
        {
            double m1 = metrica[cbFrom.Text];
            double m2 = metrica[cbTo.Text];
            double n = Convert.ToDouble(tbFrom.Text) * m1 / m2;
            tbTo.Text = n > 0.001 ? n.ToString() : n.ToString("0.0000E00");
        }

        private void btnSwap_Click(object sender, EventArgs e)
        {
            string t = cbFrom.Text;
            cbFrom.Text = cbTo.Text;
            cbTo.Text = t;
            Converter();
        }

        private void cbMetric_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbMetric.Text)
            {
                case "длинна":
                    metrica.Clear();
                    metrica.Add("mm", 1);
                    metrica.Add("cm", 10);
                    metrica.Add("dm", 100);
                    metrica.Add("m", 1000);
                    metrica.Add("km", 1000000);
                    metrica.Add("mile", 1609344);
                    cbFrom.Items.Clear();
                    cbFrom.Items.Add("mm");
                    cbFrom.Items.Add("cm");
                    cbFrom.Items.Add("dm");
                    cbFrom.Items.Add("m");
                    cbFrom.Items.Add("km");
                    cbFrom.Items.Add("mile");
                    cbFrom.Text = "mm";
                    cbTo.Items.Clear();
                    cbTo.Items.Add("mm");
                    cbTo.Items.Add("cm");
                    cbTo.Items.Add("dm");
                    cbTo.Items.Add("m");
                    cbTo.Items.Add("km");
                    cbTo.Items.Add("mile");
                    cbTo.Text = "mm";
                    break;
                default:
                    metrica.Clear();
                    metrica.Add("g", 1);
                    metrica.Add("kg", 1000);
                    metrica.Add("t", 1000000);
                    metrica.Add("lb", 453.59); // английский фунт
                    metrica.Add("oz", 28.35); // унция авердюпуа (флакончики/тюбики)
                    cbFrom.Items.Clear();
                    cbFrom.Items.Add("g");
                    cbFrom.Items.Add("kg");
                    cbFrom.Items.Add("t");
                    cbFrom.Items.Add("lb");
                    cbFrom.Items.Add("oz");
                    cbFrom.Text = "g";
                    cbTo.Items.Clear();
                    cbTo.Items.Add("g");
                    cbTo.Items.Add("kg");
                    cbTo.Items.Add("t");
                    cbTo.Items.Add("lb");
                    cbTo.Items.Add("oz");
                    cbTo.Text = "g";
                    break;
            }
            tbTo.Text = "";
        }
    }
}
