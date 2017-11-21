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
        string[] metrLength = { "Миллиметр", "Сантиметр", "Дециметр", "Метр", "Километр", "Миля" };
        string[] metrWeight = { "Грамм", "Унция", "Фунт", "Килограмм", "Центнер", "Тонна" };
        
        public FrmMain()
        {
            InitializeComponent();
            rand = new Random();
            metrica = new Dictionary<string, double>();
            switch (cbMetric.Text)
            {
                case "Вес":
                    SetDicWeight();
                    break;
                case "Длинна":
                    SetDicLength();
                    break;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            clbPassword.SetItemChecked(0, true);
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа \"Мои утилиты\", \nсодержит ряд небольших программ, \nкоторые могут пригодится в жизни.", 
                            "О программе", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);
        }

        #region Счётчик

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

        #endregion

        #region Генератор

        private void btnRandom_Click(object sender, EventArgs e)
        {
            int n = rand.Next(Convert.ToInt32(nudFrom.Value), Convert.ToInt32(nudTo.Value) + 1);
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

        #endregion
        
        #region Блокнот

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

        #endregion

        #region Пароли

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
                        password += specialChars[rand.Next(specialChars.Length)];
                            // specialChars.Length, так как последний предел исключается
                        break;
                }
            }
            tbPassword.Text = password;
            Clipboard.SetText(password);
        }

        #endregion

        #region Конвертер

        private void btnConvert_Click(object sender, EventArgs e)
        {
            Converter();
        }

        private void Converter()
        {
            double m1;
            double m2;
            try
            {
                m1 = metrica[cbFrom.Text];
                m2 = metrica[cbTo.Text];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при конвертации");
                return;
            }
            double n = Convert.ToDouble(tbFrom.Text) * m1 / m2;
            tbTo.Text = n > 0.001 ? n.ToString() : n.ToString("0.0000E00");
        }

        private void btnSwap_Click(object sender, EventArgs e)
        {
            string from = cbFrom.Text;
            cbFrom.Text = cbTo.Text;
            cbTo.Text = from;
            Converter();
        }

        private void cbMetric_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbMetric.Text)
            {
                case "Вес":
                    SetDicWeight();
                    break;
                default:
                    SetDicLength();
                    break;
            }
            tbTo.Text = "";
        }

        private void SetDicWeight()
        {
            metrica.Clear();
            metrica.Add("Грамм", 1);
            metrica.Add("Унция", 28.35); // унция авердюпуа (флакончики/тюбики)
            metrica.Add("Фунт", 453.59); // английский фунт
            metrica.Add("Килограмм", 1000);
            metrica.Add("Центнер", 100000); // центнер
            metrica.Add("Тонна", 1000000);
            string[] metrWeight2 = metrWeight.Clone() as string[];
            cbFrom.DataSource = null;
            cbFrom.DataSource = metrWeight;
            cbTo.DataSource = null;
            cbTo.DataSource = metrWeight2;
        }

        private void SetDicLength()
        {
            metrica.Clear();
            metrica.Add("Миллиметр", 1);
            metrica.Add("Сантиметр", 10);
            metrica.Add("Дециметр", 100);
            metrica.Add("Метр", 1000);
            metrica.Add("Километр", 1000000);
            metrica.Add("Миля", 1609344);
            string[] metrLength2 = metrLength.Clone() as string[];
            cbFrom.DataSource = null;
            cbFrom.DataSource = metrLength;
            cbTo.DataSource = null;
            cbTo.DataSource = metrLength2;
        }

        #endregion

    }
}
