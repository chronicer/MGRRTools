using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MGRRDat
{

    public partial class Form1 : Form
    {
        string langugage = "eng";
        JObject JSONSettings = JObject.Parse(File.ReadAllText("enemyList.json"));
        JObject JSONXml;
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            for (int i=0; i < JSONSettings["Soldiers"].Count(); i++)
                listBox2.Items.Add(JSONSettings["Soldiers"][i]["id"].ToString() + " " + JSONSettings["Soldiers"][i][langugage].ToString());

            

            foreach (JProperty property in JSONSettings.Properties())
            {
                listBox1.Items.Add(property.Name);
            }
            listBox2.SelectedIndex = 0;
            listBox1.SelectedIndex = 0;
        }





        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bxmToXml("");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            xmlToBxm("");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            unpackDat("");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            repackDat("");
        }

        public void bxmToXml (string pathToFile)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            if (pathToFile == "")
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "bxm files (*.bxm)|*.bxm|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK && pathToFile=="")
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    if (pathToFile!="") filePath = pathToFile;
                    
                    var p = new Process
                    {
                        StartInfo =
                        {
                            FileName = "py",
                            WorkingDirectory = "BXM-XML-Converter/",
                            Arguments = @"bxmToXml.py "+@""""+filePath+@""""
                        }
                    }.Start();
                }
            }

            if (pathToFile != "")
            {
                var p = new Process
                {
                    StartInfo =
                        {
                            FileName = "py",
                            WorkingDirectory = "BXM-XML-Converter/",
                            Arguments = @"bxmToXml.py "+@""""+pathToFile+@""""
                        }
                }.Start();
            }
        }

        public void xmlToBxm(string pathToFile)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            if (pathToFile == "")
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;
                        var p = new Process
                        {
                            StartInfo =
                        {
                            FileName = "py",
                            WorkingDirectory = "BXM-XML-Converter/",
                            Arguments = @"XmlToBxm.py "+@""""+filePath+@""""
                        }
                        }.Start();
                    }
                }
            }
            if (pathToFile != "")
            {
                var p = new Process
                {
                    StartInfo =
                        {
                            FileName = "py",
                            WorkingDirectory = "BXM-XML-Converter/",
                            Arguments = @"XmlToBxm.py "+@""""+pathToFile+@""""
                        }
                }.Start();
            }
        }

        public void repackDat(string folderPath)
        {
            if (folderPath == "") folderPath = textBox2.Text;
            var p = new Process
            {
                StartInfo =
                 {
                     FileName = "py",
                     WorkingDirectory = "DATRepacker/",
                     Arguments = @"dat.py "+@""""+folderPath+@""""
                 }
            }.Start();
        }

        public void unpackDat(string pathToFile)
        {
            var filePath = string.Empty;

            if (pathToFile=="")
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK && pathToFile=="")
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    //string strCmdText;
                    //strCmdText = path+@"\DATTools\dattool.exe unpack "+filePath+" "+Path.GetDirectoryName(filePath) + @"\" + Path.GetFileNameWithoutExtension(filePath);
                    string path = Path.GetDirectoryName(Application.ExecutablePath);

                    var p = new Process
                    {
                        StartInfo =
                          {
                              FileName = "dattool.exe",
                              WorkingDirectory = "DATTools/",
                              Arguments = @"unpack "+@""""+filePath+@""""+" "+@""""+Path.GetDirectoryName(filePath)+@"\"+Path.GetFileNameWithoutExtension(filePath)+@""""
                          }
                    }.Start();
                }
            }

            if (pathToFile != "")
            {
                //string strCmdText;
                //strCmdText = path+@"\DATTools\dattool.exe unpack "+filePath+" "+Path.GetDirectoryName(filePath) + @"\" + Path.GetFileNameWithoutExtension(filePath);
                string path = Path.GetDirectoryName(Application.ExecutablePath);

                var p = new Process
                {
                    StartInfo =
                    {
                        FileName = "dattool.exe",
                        WorkingDirectory = "DATTools/",
                        Arguments = @"unpack "+@""""+pathToFile+@""""+" "+@""""+Path.GetDirectoryName(pathToFile)+@"\"+Path.GetFileNameWithoutExtension(pathToFile)+@""""
                    }
                }.Start();
            }

        }

       


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public void enemyReplace (string pathToDat)
        {
            int secToSleep = 500;
            string EnemyType = listBox1.GetItemText(listBox1.SelectedItem);


            string fileName = Path.GetFileNameWithoutExtension(pathToDat);
            string fileDirectory = Path.GetDirectoryName(pathToDat);
            unpackDat(pathToDat);

            Thread.Sleep(secToSleep);
            bxmToXml(fileDirectory + @"\" + fileName + @"\" + fileName + "_EnemySet.bxm");

            XmlDocument doc = new XmlDocument();
            doc = new XmlDocument();

            Thread.Sleep(secToSleep);
            doc.LoadXml(File.ReadAllText(fileDirectory + @"\" + fileName + @"\" + fileName + "_EnemySet.xml"));
            string json;
            json = JsonConvert.SerializeXmlNode(doc);
            JSONXml = JObject.Parse(json.ToString());
            JToken JSONEnemyList = JSONXml["NewEmSetRoot"]["Corps"][0]["CorpsRoot"]["MemberList"]["GroupList"][0]["diffInfo"]["EnemyList"];

            
            JSONEnemyList["SetInfo"]["@Id"] = JSONSettings[EnemyType][listBox2.SelectedIndex]["id"].ToString();
           
            if (textBox3.Text != "") JSONEnemyList["SetInfo"]["@SetType"] = textBox3.Text;
            if (setRtnBox.Text!= "") JSONEnemyList["SetInfo"]["@SetRtn"] = setRtnBox.Text;
            if (setFlag.Text!= "") JSONEnemyList["SetInfo"]["@SetFlag"] = setFlag.Text;
            if (initialRtn.Text!= "") JSONEnemyList["SetInfo"]["@InitialRtn"] = initialRtn.Text;

            XmlDocument doc2 = (XmlDocument)JsonConvert.DeserializeXmlNode(JSONXml.ToString());
            File.WriteAllText(fileDirectory + @"\" + fileName + @"\" + fileName + "_EnemySet.xml", doc2.OuterXml);


            Thread.Sleep(secToSleep);
            xmlToBxm(fileDirectory + @"\" + fileName + @"\" + fileName + "_EnemySet.xml");

            Thread.Sleep(secToSleep);
            File.Delete(fileDirectory + @"\" + fileName + @"\" + fileName + "_EnemySet.xml");

            repackDat(fileDirectory + @"\" + fileName);

            Thread.Sleep(secToSleep);
            string directory = fileDirectory + @"\" + fileName;


            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            MessageBox.Show("We done here!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            enemyReplace(textBox1.Text);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string EnemyType = listBox1.GetItemText(listBox1.SelectedItem);
            listBox2.Items.Clear();
            for (int i = 0; i < JSONSettings[EnemyType].Count(); i++)
                listBox2.Items.Add(JSONSettings[EnemyType][i]["id"].ToString() + " " + JSONSettings[EnemyType][i][langugage].ToString());
            listBox2.SelectedIndex = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bxmToXml(textBox4.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            xmlToBxm(textBox5.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    enemyReplace(openFileDialog.FileName);
                }

            }


            }

        private void button9_Click(object sender, EventArgs e)
        {
            unpackDat(textBox6.Text);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            langugage = "eng";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            langugage = "rus";
        }
    }
}
