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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MGRRDat
{

    public partial class Form1 : Form
    {

   

        string NewEmSetRoot = "//NewEmSetRoot";
        string pathToEnemyGroup = "CorpsRoot/MemberList/GroupList[contains(@name, 'GROUP_00')]/diffInfo/EnemyList";

        string[] IDs = new string[32];
        string[] Transes = new string[32];
        string[] SetTypes = new string[32];
        string[] Types = new string[32];
        string pathToBxmXml = "";

        XmlDocument XmlDoc = new XmlDocument();
        XmlNode primary;
        XmlNode idNodes;
        XmlNodeList enemies;

        string langugage = "eng";
        JObject JSONSettings = JObject.Parse(File.ReadAllText("enemyList.json"));
        public Form1()
        {
            InitializeComponent();

        }

        public void RefreshTreeView()
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("Enemy list");
            enemies = idNodes.SelectNodes("SetInfo");
            for (int j = 0; j < enemies.Count; j++)
            {
                treeView1.Nodes[0].Nodes.Add(enemies[j].Attributes["Id"].Value.ToString());
            }
            treeView1.EndUpdate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            for (int i = 0; i < JSONSettings["Soldiers"].Count(); i++)
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

        public void bxmToXml(string pathToFile)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            if (pathToFile == "")
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "bxm files (*.bxm)|*.bxm|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = false;

                    if (openFileDialog.ShowDialog() == DialogResult.OK && pathToFile == "")
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;
                        if (pathToFile != "") filePath = pathToFile;

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

            if (pathToFile == "")
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = false;

                    if (openFileDialog.ShowDialog() == DialogResult.OK && pathToFile == "")
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

        public int TreeViewSelectedIndex()
        {
            int index = 0;
            int selectedNodeIndex = -1;
            
            foreach (var _item in treeView1.Nodes[0].Nodes)
            {
                if (_item == treeView1.SelectedNode)
                {
                    selectedNodeIndex = index;
                    break;
                }
                index++;
            }
            return selectedNodeIndex;
        }

        public void ReadDataFromDat(string pathToDat)
        {
            int secToSleep = 500;
            unpackDat(pathToDat);

            string fileName = Path.GetFileNameWithoutExtension(pathToDat);
            string fileDirectory = Path.GetDirectoryName(pathToDat);

            Thread.Sleep(secToSleep);


            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(fileDirectory+@"\"+fileName);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            IEnumerable<System.IO.FileInfo> fileQuery =
            from file in fileList
            where file.Extension == ".bxm"
            orderby file.Name
            select file;


            

            foreach (System.IO.FileInfo fi in fileQuery)
            {
                if (fi.Name.Contains("_EnemySet.bxm"))
                {
                    pathToBxmXml = fi.FullName;
                    break;
                }
                
            }

            bxmToXml(pathToBxmXml);

            Thread.Sleep(secToSleep);
            XmlDoc.LoadXml(File.ReadAllText(pathToBxmXml.Replace(".bxm",".xml")));

            primary = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[0];
            idNodes = primary.SelectSingleNode(pathToEnemyGroup);
           

            enemies = idNodes.SelectNodes("SetInfo");
            RefreshTreeView();
        }

        public void enemyReplace (string pathToDat)
        {
            int secToSleep = 500;
            string EnemyType = listBox1.GetItemText(listBox1.SelectedItem);


            string fileName = Path.GetFileNameWithoutExtension(pathToDat);
            string fileDirectory = Path.GetDirectoryName(pathToDat);

            for (int j = 0; j<enemies.Count; j++)
            {
                if (IDs[j]!="" && IDs[j]!=null) 
                    enemies[j].Attributes["Id"].Value = IDs[j];

                if (Transes[j]!="" && Transes[j]!=null) 
                    enemies[j].Attributes["Trans"].Value = Transes[j];
                   
                if (SetTypes[j]!= "" && SetTypes[j]!=null) 
                    enemies[j].Attributes["SetType"].Value = SetTypes[j];
                    
                if (Types[j]!="" && Types[j]!=null) 
                    enemies[j].Attributes["Type"].Value = Types[j];
                
                //SetNo
                enemies[j].FirstChild.InnerXml = j.ToString();
            }


            XmlDoc.Save(pathToBxmXml.Replace(".bxm", ".xml"));



            Thread.Sleep(secToSleep);
            xmlToBxm(pathToBxmXml.Replace(".bxm", ".xml"));

            Thread.Sleep(secToSleep);
            File.Delete(pathToBxmXml.Replace(".bxm", ".xml"));

            repackDat(fileDirectory + @"\" + fileName);
           // string directory = fileDirectory + @"\" + fileName;


             /*if (Directory.Exists(directory))
             {
                 Directory.Delete(directory, true);
             }*/
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

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void Free0Box_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
            if (TreeViewSelectedIndex()!=-1)
            {
                if (IDs[TreeViewSelectedIndex()]!="" && IDs[TreeViewSelectedIndex()]!=null)
                {
                    IDTextBox.Text = IDs[TreeViewSelectedIndex()];
                    treeView1.Nodes[0].Nodes[TreeViewSelectedIndex()].Text = IDs[TreeViewSelectedIndex()];
                }
                else IDTextBox.Text = enemies[TreeViewSelectedIndex()].Attributes["Id"].Value;


                
                
                if (Transes[TreeViewSelectedIndex()] != "" && Transes[TreeViewSelectedIndex()] != null)
                {
                    TransTextBox.Text = Transes[TreeViewSelectedIndex()];
                }
                else TransTextBox.Text = enemies[TreeViewSelectedIndex()].Attributes["Trans"].Value;

                
                
                if (SetTypes[TreeViewSelectedIndex()] != "" && Transes[TreeViewSelectedIndex()] != null)
                {
                    SetTypeTextBox.Text = SetTypes[TreeViewSelectedIndex()];
                }
                else SetTypeTextBox.Text = enemies[TreeViewSelectedIndex()].Attributes["SetType"].Value;

                if (Types[TreeViewSelectedIndex()] != "" && Transes[TreeViewSelectedIndex()] != null)
                {
                    TypeTextBox.Text = Types[TreeViewSelectedIndex()];
                }
                else TypeTextBox.Text = enemies[TreeViewSelectedIndex()].Attributes["Type"].Value;
            }
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ReadDataFromDat(textBox1.Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            IDs[TreeViewSelectedIndex()] = IDTextBox.Text;
            Transes[TreeViewSelectedIndex()] = TransTextBox.Text;
            SetTypes[TreeViewSelectedIndex()] = SetTypeTextBox.Text;
            Types[TreeViewSelectedIndex()] = TypeTextBox.Text;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            idNodes.AppendChild(idNodes.SelectSingleNode("SetInfo").Clone());
            RefreshTreeView();
        }
    }
}
