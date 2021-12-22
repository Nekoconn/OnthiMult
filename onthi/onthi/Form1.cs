using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace onthi
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        struct QuestionSheet
        {
            public List<int> selectionRan;
            public QuestionT1 question;
            public QuestionSheet(QuestionT1 q)
            {
                question = q;
                selectionRan = new List<int>();
                for (int i = 0; i < q.cauDung.Count + q.cauSai.Count; i++)
                {
                    selectionRan.Add(i);
                }
                selectionRan.Shuffle();
            }
            public string getSelection(int pos)
            {
                if (pos < question.cauDung.Count)
                {
                    return question.cauDung[pos];
                }
                else
                {
                    return question.cauSai[pos - question.cauDung.Count];
                }
            }

            public int getTotal()
            {
                return question.cauDung.Count + question.cauSai.Count;
            }
            public bool check(List<int> l)
            {
                int count = 0;
                foreach (int i in l)
                {
                    if (selectionRan[i] < question.cauDung.Count)
                    {
                        count++;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (count == question.cauDung.Count)
                {
                    return true;
                }
                return false;
            }
        }

        String[,] readToStringArray(StreamReader sr)
        {
            int numlines = 0;
            List<String[]> toRe = new List<String[]>();
            using (TextFieldParser parser = new TextFieldParser(sr))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;
                while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();
                    toRe.Add(fields);
                    numlines++;
                }
            }
            String[,] rere = new String[numlines, toRe[0].Length];
            for (int i = 0; i < numlines; i++)
            {
                for (int o = 0; o < toRe[0].Length; o++) {
                    rere[i, o] = toRe[i][o];
                }
            }

            return rere;
        }

        List<QuestionT1> q1;
        List<QuestionT1> q2;
        List<QuestionT1> q3;
        List<QuestionT1> q4;
        List<QuestionT1> q5;
        List<QuestionT1> q6;
        List<QuestionT1> q7;
        List<QuestionT1> q11;
        List<QuestionSheet> dethi;
        List<List<int>> answerSheet;
        List<bool> rightQ;
        bool locked;
        void readQ1toQ7(List<QuestionT1> l, string s)
        {
            String savePath = "Cauhoi.zip";
            try
            {
                using (FileStream zipToOpen = new FileStream(savePath, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        if (archive.GetEntry(s) != null)
                        {
                            ZipArchiveEntry readmeEntry = archive.GetEntry(s);
                            String[,] strArr = readToStringArray(new StreamReader(readmeEntry.Open()));
                            int i = 0;
                            while (i < strArr.GetLength(0)) {
                                QuestionT1 q = new QuestionT1();
                                q.cauHoi = strArr[i, 3];
                                q.cauDung = new List<String>();
                                q.cauSai = new List<String>();
                                try
                                {
                                    q.soCauDung = Int32.Parse(strArr[i, 6]);
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to parse int");
                                }
                                if (strArr[i, 4] != "")
                                {
                                    q.cauDung.Add(strArr[i, 4]);
                                }
                                if (strArr[i, 5] != "")
                                {
                                    q.cauSai.Add(strArr[i, 5]);
                                }
                                i++;
                                while (i < strArr.GetLength(0) && strArr[i, 0] == "")
                                {
                                    if (strArr[i, 4] != "")
                                    {
                                        q.cauDung.Add(strArr[i, 4]);
                                    }
                                    if (strArr[i, 5] != "")
                                    {
                                        q.cauSai.Add(strArr[i, 5]);
                                    }
                                    
                                    i++;
                                }
                                l.Add(q);
                            }
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }

        void readQ11(List<QuestionT1> l, string s)
        {
            String savePath = "Cauhoi.zip";
            try
            {
                using (FileStream zipToOpen = new FileStream(savePath, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        if (archive.GetEntry(s) != null)
                        {
                            ZipArchiveEntry readmeEntry = archive.GetEntry(s);
                            String[,] strArr = readToStringArray(new StreamReader(readmeEntry.Open()));
                            int i = 0;
                            while (i < strArr.GetLength(0))
                            {
                                QuestionT1 q = new QuestionT1();
                                q.cauHoi = strArr[i, 1];
                                String[] rightlist = strArr[i, 2].Split(',');
                                q.cauDung = new List<String>();
                                q.cauSai = new List<String>();

                                q.soCauDung = 0;
                                if (rightlist.Contains("1"))
                                {
                                    q.cauDung.Add(strArr[i, 3]);
                                }
                                else
                                {
                                    q.cauSai.Add(strArr[i, 3]);
                                }
                                if (rightlist.Contains("2"))
                                {
                                    q.cauDung.Add(strArr[i, 4]);
                                }
                                else
                                {
                                    q.cauSai.Add(strArr[i, 4]);
                                }
                                if (rightlist.Contains("3"))
                                {
                                    q.cauDung.Add(strArr[i, 5]);
                                }
                                else
                                {
                                    q.cauSai.Add(strArr[i, 5]);
                                }
                                if (rightlist.Contains("4"))
                                {
                                    q.cauDung.Add(strArr[i, 6]);
                                }
                                else
                                {
                                    q.cauSai.Add(strArr[i, 6]);
                                }


                                i++;
                                l.Add(q);
                            }
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }
        void readQ()
        {
            q1 = new List<QuestionT1>();
            q2 = new List<QuestionT1>();
            q3 = new List<QuestionT1>();
            q4 = new List<QuestionT1>();
            q5 = new List<QuestionT1>();
            q6 = new List<QuestionT1>();
            q7 = new List<QuestionT1>();
            q11 = new List<QuestionT1>();
            readQ1toQ7(q1, "Q1.csv");
            readQ1toQ7(q2, "Q2.csv");
            readQ1toQ7(q3, "Q3.csv");
            readQ1toQ7(q4, "Q4.csv");
            readQ1toQ7(q5, "Q5.csv");
            readQ1toQ7(q6, "Q6.csv");
            readQ1toQ7(q7, "Q7.csv");
            readQ11(q11, "Q11.csv");
            q1.Shuffle();
            q2.Shuffle();
            q3.Shuffle();
            q4.Shuffle();
            q5.Shuffle();
            q6.Shuffle();
            q7.Shuffle();
            q11.Shuffle();

        }
        private List<QuestionSheet> generateDeThi()
        {
            List<QuestionSheet> toRe = new List<QuestionSheet>();
            for (int i = 0; i < 10 && i < q1.Count; i++)
            {
                toRe.Add(new QuestionSheet(q1[i]));
            }
            for (int i = 0; i < 3 && i < q2.Count; i++)
            {
                toRe.Add(new QuestionSheet(q2[i]));
            }
            for (int i = 0; i < 10 && i < q3.Count; i++)
            {
                toRe.Add(new QuestionSheet(q3[i]));
            }
            for (int i = 0; i < 3 && i < q4.Count; i++)
            {
                toRe.Add(new QuestionSheet(q4[i]));
            }
            for (int i = 0; i < 10 && i < q5.Count; i++)
            {
                toRe.Add(new QuestionSheet(q5[i]));
            }
            for (int i = 0; i < 3 && i < q6.Count; i++)
            {
                toRe.Add(new QuestionSheet(q6[i]));
            }
            for (int i = 0; i < 2 && i < q7.Count; i++)
            {
                toRe.Add(new QuestionSheet(q7[i]));
            }
            for (int i = 0; i < 20 && i < q11.Count; i++)
            {
                toRe.Add(new QuestionSheet(q11[i]));
            }
            toRe.Shuffle();
            return toRe;
        }
        private void displayQuestionSheet(int pos)
        {
            checkedListBox1.Items.Clear();
            QuestionSheet qs = dethi[pos];
            label1.Text = "Câu hỏi: " + (pos+1) + "/" + dethi.Count;
            textBox1.Text = qs.question.cauHoi;

            foreach (int s in qs.selectionRan)
            {
                checkedListBox1.Items.Add(qs.getSelection(s));
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.ScrollBars = ScrollBars.Vertical;
            myCheckedListBox1.PrimaryColor = Color.White;
            myCheckedListBox1.AlternateColor =Color.Red ;
            rightQ = new List<bool>();
            readQ();
            dethi = generateDeThi();
            for (int i = 0; i < dethi.Count; i++)
            {
                rightQ.Add(true);
            }
            for (int i = 0; i < dethi.Count; i++)
            {
                string s = "Câu " + (i + 1);
                myCheckedListBox1.Items.Add(s);
            }
            answerSheet = new List<List<int>>();
            for (int i = 0; i < dethi.Count; i++)
            {
                List<int> l = new List<int>();
                answerSheet.Add(l);
                
            }
            myCheckedListBox1.SelectedIndex = 0;
            displayQuestionSheet(0);
            locked = false;
            label4.Visible = false;
        }
        private void saveSelected(int pos)
        {
            answerSheet[pos].Clear();
            foreach (int i in checkedListBox1.CheckedIndices)
            {
                answerSheet[pos].Add(i);
            }
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveSelected(myCheckedListBox1.SelectedIndex);
            checkedListBox1.ClearSelected();
            
        }

        private void loadSelected(int pos)
        {
            bool cur = locked;
            locked = false;
            List<int> a = answerSheet[pos];
            foreach (int i in a) {
                checkedListBox1.SetItemChecked(i, true);
            }
            locked = cur;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            myCheckedListBox1.PrimaryColor = Color.Green;
            rightQ.Clear();
            rightQ = new List<bool>();
            locked = true;
            label4.Visible = true;
            int countRight = 0;
            for (int i = 0; i < dethi.Count; i++)
            {
                QuestionSheet qs = dethi[i];
                List<int> ans = answerSheet[i];
                bool isRight = qs.check(ans);
                if (isRight)
                {
                    countRight++;
                }
                rightQ.Add(isRight);
            }
            label4.Text = "Điểm: " + countRight + "/" + dethi.Count;
            label4.ForeColor = Color.Green;
            myCheckedListBox1.setQRight(rightQ);
            myCheckedListBox1.Refresh();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (locked)
            {
                e.NewValue = e.CurrentValue;
            }
        }

        private void myCheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayQuestionSheet(myCheckedListBox1.SelectedIndex);
            loadSelected(myCheckedListBox1.SelectedIndex);
        }
    }
    static class MyExtensions
    {
        private static Random rng = new Random();
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
