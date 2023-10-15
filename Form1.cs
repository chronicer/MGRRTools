using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using static MGRRDat.Form1;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static MGRRDat.EffectStruct;

namespace MGRRDat
{

    public partial class Form1 : Form
    {
  



        int RedPos = 0;
        int GreenPos = 0;
        int BluePos = 0;


        Move_s[] move;
        Emif_s[] emif;
        EffectHeader header;
        TypeGroups[] typeGroup;

        uint[] offsets;


        string NewEmSetRoot = "//NewEmSetRoot";
        string pathToEnemyGroup = "CorpsRoot/MemberList/GroupList[contains(@name, 'GROUP_00')]/diffInfo/EnemyList";

        string[] IDs = new string[100];
        string[] Transes = new string[100];
        string[] SetTypes = new string[100];
        string[] Types = new string[100];

        bool multipleDatUnpack = false;
        bool multipleDatRepack = false;

        string pathToBxmXml = "";
        string treeViewPath = "";

        int firstIndex = -1;
        int secondIndex = -1;
        int thirdIndex = -1;


        XmlDocument XmlDoc = new XmlDocument();
        XmlNode primary;
        XmlNode idNodes;
        XmlNodeList enemies;

        System.Windows.Forms.TextBox[] textBoxes;

        string langugage = "eng";
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxes = new System.Windows.Forms.TextBox[] {
            IDTextBox, RoomTextBox, BaseRotTextBox, BaseRotLTextBox, TransTextBox, TransLTextBox,
            RotationTextBox, SetTypeTextBox, TypeTextBox, SetRtnTextBox, SetFlagTextBox, PathNoTextBox,
            WaypointNoTextBox, SetWaitTextBox, HpTextBox, ParamTextBox, BezierNoTextBox, ParentIdTextBox,
            PartsNoTextBox, HashNoTextBox, ItemIdTextBox, GroupPosTextBox, InitialRtnTextBox, InitialPosTextBox,
            InitialPosDirYTextBox, InitialTimeTextBox, ItemAliasTextBox, Free0TextBox, DropItemNormalTextBox,
            DropItemStealthTextBox, VisceraTableNoTextBox, ReflexViewAngYTextBox, ReflexViewAngXTextBox,
            ReflexViewDistTextBox, ScoutViewAngYTextBox, ScoutViewAngXTextBox, ScoutViewDistTextBox
            };
        }





        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BxmXml("","bxmToXml");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BxmXml("", "xmlToBxm");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            unpackDat("", multipleDatUnpack);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            repackDat(textBox2.Text,multipleDatRepack);
        }

        public void BxmXml(string pathToFile, string type)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            if (pathToFile == "")
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    if (type=="bxmToXml") openFileDialog.Filter = "bxm files (*.bxm)|*.bxm|All files (*.*)|*.*";
                    if (type=="xmlToBxm") openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                   
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
                            FileName = type+".exe",
                            WorkingDirectory = "BXM-XML/",
                            Arguments = @" "+@""""+filePath+@""""
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
                            FileName = type+".exe",
                            WorkingDirectory = "BXM-XML/",
                            Arguments = @" "+@""""+pathToFile+@""""
                        }
                }.Start();
            }
        }

        public void repackMcd(string pathToOldMcd, string pathToNewMcd, string pathToTextFile)
        {

            if (pathToOldMcd == "")
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "mcd files (*.mcd)|*.mcd|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        string oldMcd = openFileDialog.FileName;

                        /******************** for unpacked file ***************************/
                        using (OpenFileDialog openFileDialog2 = new OpenFileDialog())
                        {

                            openFileDialog2.Filter = "All files (*.*)|*.*";
                            openFileDialog2.FilterIndex = 1;
                            openFileDialog2.RestoreDirectory = true;

                            if (openFileDialog2.ShowDialog() == DialogResult.OK)
                            {
                                string textFile = openFileDialog2.FileName;
                                MessageBox.Show(@"repack " + @"""" + oldMcd + @"""" + @" "+@""""+Path.Combine(Path.GetDirectoryName(oldMcd), Path.GetFileNameWithoutExtension(oldMcd)) + @"_new.mcd"+ @"""");
                                var p = new Process
                                {
                                    StartInfo =
                                    {
                                     FileName = "mcdtool.exe",
                                     WorkingDirectory = "DATTools/",
                                     Arguments = @"repack " + @"""" + oldMcd + @"""" + @" "+@""""+Path.Combine(Path.GetDirectoryName(oldMcd), Path.GetFileNameWithoutExtension(oldMcd)) + @"_new.mcd"+ @""""+@" "+textFile
                                    }
                                }.Start();
                            }
                        }

                    }
                }
            }
            if (pathToOldMcd != "")
            {
                MessageBox.Show(@"repack " + @"""" + pathToOldMcd + @"""" + @" " +@""""+ pathToNewMcd + @"""" + @" " + pathToTextFile);
                var p = new Process
                {
                    StartInfo =
                        {
                            FileName = "mcdtool.exe",
                            WorkingDirectory = "DATTools/",
                            Arguments = @"repack "+@""""+pathToOldMcd+@""""+@" "+@""""+pathToNewMcd+@""""+@" "+pathToTextFile
                        }
                }.Start();
            }
        }



        public void unpackMcd(string pathToFile, string pathToNewFile)
        {

            if (pathToFile == "")
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "mcd files (*.mcd)|*.mcd|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        string filePath = openFileDialog.FileName;
                        var p = new Process
                        {
                            StartInfo =
                        {
                            FileName = "mcdtool.exe",
                            WorkingDirectory = "DATTools/",
                            Arguments = @"unpack "+@""""+filePath+@""""+@" "+Path.Combine(Path.GetDirectoryName(filePath),Path.GetFileNameWithoutExtension(filePath))+@".txt"
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
                            FileName = "mcdtool.exe",
                            WorkingDirectory = "DATTools/",
                            Arguments = @"unpack "+@""""+pathToFile+@""""+@" "+pathToNewFile
                        }
                }.Start();
            }
        }

        public void repackDat(string folderPath, bool multiple)
        {
            if (folderPath != "")
            {
                if (multiple)
                {

                    // Получаем список папок
                    DirectoryInfo directory = new DirectoryInfo(folderPath);
                    DirectoryInfo[] subDirectories = directory.GetDirectories();

                    foreach (DirectoryInfo subDirectory in subDirectories)
                    {
                        var p = new Process
                        {
                            StartInfo =
                        {
                            FileName = "dat.exe",
                            WorkingDirectory = "DATRepacker/",
                            Arguments = @" "+@""""+folderPath+@"\"+subDirectory.ToString()+@""""
                        }
                        }.Start();
                    }

                }

                if (!multiple)
                {
                    var p = new Process
                    {
                        StartInfo =
                        {
                            FileName = "dat.exe",
                            WorkingDirectory = "DATRepacker/",
                            Arguments = @" "+@""""+folderPath+@""""
                        }
                    }.Start();
                }
            }
        }

        public void unpackDat(string pathToFile, bool multiple)
        {
            var filePath = string.Empty;


            if (multiple)
            {
                string folderPath = Path.GetDirectoryName(pathToFile+@"\"); // pathToFolder
                string[] datFiles = Directory.GetFiles(folderPath); // array of .dat files

                foreach (string file in datFiles)
                {
                    if (Path.GetExtension(folderPath + file) == ".dat" || Path.GetExtension(folderPath + file) == ".dtt")
                    {

                        var process = new Process();
                        process.StartInfo.FileName = @"dattool.exe";
                        process.StartInfo.WorkingDirectory = @"DATTools\";
                        process.StartInfo.Arguments = @"unpack " + @"""" + file + @"""" + " " + @"""" + folderPath + @"\" + Path.GetFileNameWithoutExtension(file) + @"""";
                        process.Start();

                     /*   var p = new Process
                        {
                            StartInfo =
                            {
                                FileName = "dattool.exe",
                                WorkingDirectory = "DATTools/",
                                Arguments = @"unpack "+@""""+file+@""""+" "+@""""+folderPath+@"\"+Path.GetFileNameWithoutExtension(file)+@""""
                            }
                        }.Start();*/
                    }
                }

            }



            if (!multiple)
            {
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
                            string path = Path.GetDirectoryName(Application.ExecutablePath);


                            var process = new Process();
                            process.StartInfo.FileName = @"dattool.exe";
                            process.StartInfo.WorkingDirectory = @"DATTools\";
                            process.StartInfo.Arguments = @"unpack " + @"""" + filePath + @"""" + " " + @"""" + Path.GetDirectoryName(filePath) + @"\" + Path.GetFileNameWithoutExtension(filePath) + @"""";
                            process.Start();

                        /*    var p = new Process
                            {
                                StartInfo =
                          {
                              FileName = "dattool.exe",
                              WorkingDirectory = "DATTools/",
                              Arguments = @"unpack "+@""""+filePath+@""""+" "+@""""+Path.GetDirectoryName(filePath)+@"\"+Path.GetFileNameWithoutExtension(filePath)+@""""
                          }
                            }.Start();*/
                        }
                    }

                if (pathToFile != "")
                {
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
        }


        public void BnkTool(string pathToFile, string type)
        {
            string cmd = "";
            if (pathToFile == "")
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "bnk files (*.bnk)|*.bnk|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = false;

                    if (openFileDialog.ShowDialog() == DialogResult.OK && pathToFile == "")
                    {
                        //Get the path of specified file
                        pathToFile = openFileDialog.FileName;

                        var p = new Process
                        {
                            StartInfo =
                          {
                              FileName = "nier_BNK_Util.exe",
                              WorkingDirectory = "BNKTool/",
                              Arguments = @""""+pathToFile+@""""+" "+@"-"+type+@" "+@""""+Path.GetDirectoryName(pathToFile)+@"\"+Path.GetFileNameWithoutExtension(pathToFile)+@""""
                          }
                        }.Start();
                    }
                }

            if (pathToFile != "")
            {
                
                if (type == "e") cmd = @"""" + pathToFile + @"""" + " " + @"-" + type + @" " + @"""" + Path.GetDirectoryName(pathToFile) + @"\" + Path.GetFileNameWithoutExtension(pathToFile) + @"""";
                if (type == "r") cmd = @"""" + pathToFile +".bnk"+ @"""" + " " + @"-" + type + @" " + @"""" + Path.GetDirectoryName(pathToFile) + @"\" + Path.GetFileNameWithoutExtension(pathToFile) + @"""";
                var p = new Process
                {
                    StartInfo =
                    {
                        FileName = "nier_BNK_Util.exe",
                        WorkingDirectory = "BNKTool/",
                        Arguments = cmd
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
        private int GetIndex(TreeNode node)
        {
            // Always make a way to exit the recursion.
            if (node.Parent == null)
                return node.Index;

            return node.Index + GetIndex(node.Parent);
        }

        public void ReadDataFromDat(string pathToDat)
        {
            int secToSleep = 500;
            unpackDat(pathToDat,false);

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

            BxmXml(pathToBxmXml, "bxmToXml");

            Thread.Sleep(secToSleep);
            XmlDoc.LoadXml(File.ReadAllText(pathToBxmXml.Replace(".bxm",".xml")));


            primary = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[0];
            idNodes = primary.SelectSingleNode(pathToEnemyGroup);



            treeView2.BeginUpdate();
            treeView2.Nodes.Clear();
           // enemies = idNodes.SelectNodes("SetInfo");


            for (int j = 0; j < XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes.Count; j++)
            {
               // treeView2.Nodes.Add("COPRS_0" + j.ToString());
                if (j<10) treeView2.Nodes.Add("COPRS_0" + j.ToString());
                if (j>=10) treeView2.Nodes.Add("COPRS_" + j.ToString());
                
                for (int k=0;k<XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[j].SelectSingleNode("CorpsRoot/MemberList").ChildNodes.Count;k++)
                {
                    treeView2.Nodes[j].Nodes.Add(XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[j].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[k].Attributes["name"].Value.ToString());
                    // if (k == 0 && j == 0) MessageBox.Show(XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[j].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[k].InnerXml);





                    // if (k == 0 && j == 0) MessageBox.Show(XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[j].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[k].SelectSingleNode("diffInfo/EnemyList").ChildNodes[0].Attributes["Id"].Value.ToString());

                    if (XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[j].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[k].SelectSingleNode("diffInfo") != null)
                    {
                        for (int l = 0; l < XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[j].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[k].SelectSingleNode("diffInfo/EnemyList").ChildNodes.Count; l++)
                        {

                            treeView2.Nodes[j].Nodes[k].Nodes.Add(XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[j].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[k].SelectSingleNode("diffInfo/EnemyList").ChildNodes[l].Attributes["Id"].Value.ToString());
                        }
                    }
                }
            }
            treeView2.EndUpdate();
        }

        public void enemyReplace (string pathToDat)
        {
            int secToSleep = 500;
            string EnemyType = listBox1.GetItemText(listBox1.SelectedItem);


            string fileName = Path.GetFileNameWithoutExtension(pathToDat);
            string fileDirectory = Path.GetDirectoryName(pathToDat);


            XmlDoc.Save(pathToBxmXml.Replace(".bxm", ".xml"));



            Thread.Sleep(secToSleep);
            BxmXml(pathToBxmXml.Replace(".bxm", ".xml"), "xmlToBxm");

            Thread.Sleep(secToSleep);
            File.Delete(pathToBxmXml.Replace(".bxm", ".xml"));

            repackDat(fileDirectory + @"\" + fileName,false);
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
            IDTextBox.Text = listBox1.GetItemText(listBox1.SelectedItem).Substring(0,listBox1.GetItemText(listBox1.SelectedItem).IndexOf(' '));

            // string EnemyType = listBox1.GetItemText(listBox1.SelectedItem);
            // listBox2.Items.Clear();
            // for (int i = 0; i < JSONSettings[EnemyType].Count(); i++)
            //   listBox2.Items.Add(JSONSettings[EnemyType][i]["id"].ToString() + " " + JSONSettings[EnemyType][i][langugage].ToString());
            //listBox2.SelectedIndex = 0;



        }

        private void button6_Click(object sender, EventArgs e)
        {
            BxmXml(textBox4.Text, "bxmToXml");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            BxmXml(textBox5.Text, "xmlToBxm");
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
            unpackDat(textBox6.Text, multipleDatUnpack);
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
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ReadDataFromDat(textBox1.Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (firstIndex != -1)
            {
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Id"].Value = IDTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Room"].Value = RoomTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["BaseRot"].Value = BaseRotTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["BaseRotL"].Value = BaseRotLTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Trans"].Value = TransTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["TransL"].Value = TransLTextBox.Text;



                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Rotation"].Value = RotationTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["SetType"].Value = SetTypeTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Type"].Value = TypeTextBox.Text;

                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["SetRtn"].Value = SetRtnTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["SetFlag"].Value = SetFlagTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["PathNo"].Value = PathNoTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["WaypointNo"].Value = WaypointNoTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["SetWait"].Value = SetWaitTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Hp"].Value = HpTextBox.Text;



                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Param"].Value = ParamTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["BezierNo"].Value = BezierNoTextBox.Text;

                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ParentId"].Value = ParentIdTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["PartsNo"].Value = PartsNoTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["HashNo"].Value = HashNoTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ItemId"].Value = ItemIdTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["GroupPos"].Value = GroupPosTextBox.Text;


                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["InitialRtn"].Value = InitialRtnTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["InitialPos"].Value = InitialPosTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["InitialPosDirY"].Value = InitialPosDirYTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["InitialTime"].Value = InitialTimeTextBox.Text;

                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ItemAlias"].Value = label_55.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Free0"].Value = Free0TextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["DropItemNormal"].Value = DropItemNormalTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["DropItemStealth"].Value = DropItemStealthTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["VisceraTableNo"].Value = VisceraTableNoTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ReflexViewAngY"].Value = ReflexViewAngYTextBox.Text;

                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ReflexViewAngX"].Value = ReflexViewAngXTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ReflexViewDist"].Value = ReflexViewDistTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ScoutViewAngY"].Value = ScoutViewAngYTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ScoutViewAngX"].Value = ScoutViewAngXTextBox.Text;
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ScoutViewDist"].Value = ScoutViewDistTextBox.Text;

                treeView2.SelectedNode.Text = IDTextBox.Text;
            }
            else MessageBox.Show("Sam says: enemy not selected!");

        }

        private void button12_Click(object sender, EventArgs e)
        {
           
            if (firstIndex!=-1)
            {

                
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").AppendChild(
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Clone());
                int count = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes.Count;

               // MessageBox.Show(count.ToString() + " " + XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[count - 1].FirstChild.InnerXml);
                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[count - 1].FirstChild.InnerXml = (count - 1).ToString();
                treeView2.Nodes[firstIndex].Nodes[secondIndex].Nodes.Clear();
            
                //Update treeView after add enemy
                for (int i = 0; i< XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes.Count;i++)
                {
                    treeView2.Nodes[firstIndex].Nodes[secondIndex].Nodes.Add(XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[i].Attributes["Id"].Value);
                }
            }
            else
            {
                MessageBox.Show("Sam says: enemy not selected!");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {


            if (treeView2.SelectedNode.Level == 2)
            {
                firstIndex = treeView2.SelectedNode.Parent.Parent.Index;
                secondIndex = treeView2.SelectedNode.Parent.Index;
                thirdIndex = treeView2.SelectedNode.Index;



                var selectedNode = treeView2.SelectedNode;
                var memberList = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList");

                var enemyNode = memberList.ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes;

              //  for (int i = 0; i < textBoxes.Length; i++)
              //  {
                  //  textBoxes[i].Text = enemyNode[i].Value;
               // }

                IDTextBox.Text = enemyNode["Id"].Value;
                RoomTextBox.Text = enemyNode["Room"].Value;
                BaseRotTextBox.Text = enemyNode["BaseRot"].Value;
                BaseRotLTextBox.Text = enemyNode["BaseRotL"].Value;
                TransTextBox.Text = enemyNode["Trans"].Value;
                TransLTextBox.Text = enemyNode["TransL"].Value;
                RotationTextBox.Text = enemyNode["Rotation"].Value;
                SetTypeTextBox.Text = enemyNode["SetType"].Value;
                TypeTextBox.Text = enemyNode["Type"].Value;
                SetRtnTextBox.Text = enemyNode["SetRtn"].Value;
                SetFlagTextBox.Text = enemyNode["SetFlag"].Value;
                PathNoTextBox.Text = enemyNode["PathNo"].Value;
                WaypointNoTextBox.Text = enemyNode["WaypointNo"].Value;
                SetWaitTextBox.Text = enemyNode["SetWait"].Value;
                HpTextBox.Text = enemyNode["Hp"].Value;
                ParamTextBox.Text = enemyNode["Param"].Value;
                BezierNoTextBox.Text = enemyNode["BezierNo"].Value;
                ParentIdTextBox.Text = enemyNode["ParentId"].Value;
                PartsNoTextBox.Text = enemyNode["PartsNo"].Value;
                HashNoTextBox.Text = enemyNode["HashNo"].Value;
                ItemIdTextBox.Text = enemyNode["ItemId"].Value;
                GroupPosTextBox.Text = enemyNode["GroupPos"].Value;
                InitialRtnTextBox.Text = enemyNode["InitialRtn"].Value;
                InitialPosTextBox.Text = enemyNode["InitialPos"].Value;
                InitialPosDirYTextBox.Text = enemyNode["InitialPosDirY"].Value;
                InitialTimeTextBox.Text = enemyNode["InitialTime"].Value;
                label_55.Text = enemyNode["ItemAlias"].Value;
                Free0TextBox.Text = enemyNode["Free0"].Value;
                DropItemNormalTextBox.Text = enemyNode["DropItemNormal"].Value;
                DropItemStealthTextBox.Text = enemyNode["DropItemStealth"].Value;
                VisceraTableNoTextBox.Text = enemyNode["VisceraTableNo"].Value;
                ReflexViewAngYTextBox.Text = enemyNode["ReflexViewAngY"].Value;
                ReflexViewAngXTextBox.Text = enemyNode["ReflexViewAngX"].Value;
                ReflexViewDistTextBox.Text = enemyNode["ReflexViewDist"].Value;
                ScoutViewAngYTextBox.Text = enemyNode["ScoutViewAngY"].Value;
                ScoutViewAngXTextBox.Text = enemyNode["ScoutViewAngX"].Value;
                ScoutViewDistTextBox.Text = enemyNode["ScoutViewDist"].Value;



                //  IDTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Id"].Value.ToString();
                //  TransTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Trans"].Value.ToString();
                // SetTypeTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["SetType"].Value.ToString();
                /// TypeTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Type"].Value.ToString();

            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            unpackMcd(textBox3.Text,textBox7.Text);
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            unpackMcd("", "");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            repackMcd("", "", "");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            repackMcd(textBox8.Text,textBox9.Text,textBox10.Text);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            BnkTool(textBox11.Text,"e");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            BnkTool(textBox12.Text, "r");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            BnkTool("", "e");
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            multipleDatUnpack = !multipleDatUnpack;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            multipleDatRepack = !multipleDatRepack;
        }

        private void button19_Click(object sender, EventArgs e)
        {


              string output = "";
              int startCode = 16399; // начальное значение кода символа "A"
              if (checkBox3.Checked) startCode = 16490; 
              foreach (char c in textBox13.Text)
              {
                  int code = (int)c - 65 + startCode; // вычисляем код символа
                  output += $"[c:{code}:0/]"; // добавляем закодированный символ в выходную строку
              }

              textBox14.Text = output;

           // textBox14.Text = EncodeText(textBox13.Text);


        }

        private void button21_Click(object sender, EventArgs e)
        {
                string output = "";
                int startCode = 16490;

                Regex regex = new Regex(@"\[c:(\d+):0/\]"); // регулярное выражение для поиска закодированных символов

                foreach (Match match in regex.Matches(textBox15.Text))
                {
                    int code = int.Parse(match.Groups[1].Value);
                    char c = (char)(code - startCode + 65); // вычисляем исходный символ по его коду
                    output += c;
                }
                textBox16.Text = output;
        }

        static string EncodeText(string text, bool isBold)
        {
            string encodedText = "";
            int begin = 16425;
            if (isBold) begin = 16334;
            foreach (char c in text)
            {
                int unicodeValue = (int)c; // Получение числового значения Unicode для символа
                encodedText += $"[c:{unicodeValue + 16425}:0/]"; // Добавление закодированного символа к строке 16490-65
            }

            return encodedText;
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {
        }

        public static void WriteStruct<T>(BinaryWriter writer, T structure)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(structure, handle.AddrOfPinnedObject(), false);
            handle.Free();

            writer.Write(buffer);
        }

        public static T ReadStruct<T>(BinaryReader reader)
        {
            int structSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = reader.ReadBytes(structSize);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return structure;
        }

        private void button22_Click(object sender, EventArgs e)
        {
  
        }

        private void button23_Click(object sender, EventArgs e)
        {


            string filePath = textBox17.Text;

            using (var moveWriter = new BinaryWriter(File.Open(filePath, FileMode.Open)))
            {
                // Запись остальных значений в moveWriter
                // и так далее...

                // Получение текущей позиции для red
                int currentPosition = (int)moveWriter.BaseStream.Position;

                // Перемещение в нужную позицию
                moveWriter.BaseStream.Position = Convert.ToInt32(textBox13.Text);

                // Запись нового значения для red
                float newRedValue = (float)Convert.ToDouble(textBox14.Text);
                moveWriter.Write(newRedValue);
                moveWriter.Close();
            }
        }

    void EffectReader(string filePath)
        {
            header = new EffectHeader();
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                // Считываем заголовок
                header.id = new string (reader.ReadChars(4));
                header.recordCount = reader.ReadUInt32();
                header.recordOffsetsOffset = reader.ReadUInt32();
                header.typeOffset = reader.ReadUInt32();
                header.typeEndOffset = reader.ReadUInt32();
                header.typeSize = reader.ReadUInt32();
                header.typeNumber = reader.ReadUInt32();

                typeGroup = new TypeGroups[header.recordCount];
                reader.BaseStream.Seek(header.recordOffsetsOffset, SeekOrigin.Begin);
                offsets = new uint[header.recordCount];

                for (int i = 0; i<header.recordCount; i++)
                {
                    offsets[i] = reader.ReadUInt32();
                }


            for (int i = 0; i < header.recordCount; i++)
            {
                typeGroup[i] = new TypeGroups();
                typeGroup[i].types = new TypeGroup[23];

                reader.BaseStream.Seek(header.typeOffset, SeekOrigin.Begin);
                for (int j = 0; j < 23; j++)
                {
                    typeGroup[i].types[j] = new TypeGroup();
                    typeGroup[i].types[j].u_a = reader.ReadUInt32();
                    typeGroup[i].types[j].id = new string(reader.ReadChars(4));
                    typeGroup[i].types[j].size = reader.ReadUInt32();
                    typeGroup[i].types[j].offset = reader.ReadUInt32();
                }

            }               

            move = new Move_s[header.recordCount];
            emif = new Emif_s[header.recordCount];

            comboBox1.Items.Clear();
            // Считываем структуры
            for (int i = 0; i < header.recordCount; i++)
            {
                for (int j = 0; j < 23; j++)
                {
                    if (typeGroup[i].types[j].size > 0)
                    {

                        // Переходим на позицию текущего типа
                        reader.BaseStream.Seek(offsets[i] + typeGroup[i].types[j].offset, SeekOrigin.Begin);

                        if (typeGroup[i].types[j].id == "PART")
                        {
                            Part_s part;
                            using (var memStream = new MemoryStream(reader.ReadBytes((int)typeGroup[i].types[j].size)))
                            using (var partReader = new BinaryReader(memStream))
                            {
                            //part = ReadStruct<Part_s>(partReader);
                            }
                        }

                        if (typeGroup[i].types[j].id == "MOVE")
                        {
                            comboBox1.Items.Add((i + 1).ToString());

                            //Move_s move;
                            using (var memStream = new MemoryStream(reader.ReadBytes((int)typeGroup[i].types[j].size)))
                            using (var moveReader = new BinaryReader(memStream))
                            move[i] = ReadStruct<Move_s>(moveReader);
                        }

                        if (typeGroup[i].types[j].id == "EMIF")
                        {
                            using (var memStream = new MemoryStream(reader.ReadBytes((int)typeGroup[i].types[j].size)))
                            using (var emifReader = new BinaryReader(memStream))
                            emif[i] = ReadStruct<Emif_s>(emifReader);
                        }
                    }
                }
            }
            comboBox1.SelectedIndex = 0;
        }
    }

        private void label63_Click(object sender, EventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (textBox27.Text != "")
                EffectReader(textBox27.Text);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            textBox28.Text = (move[comboBox1.SelectedIndex].alpha).ToString();
            textBox29.Text = (move[comboBox1.SelectedIndex].blue).ToString();
            textBox30.Text = (move[comboBox1.SelectedIndex].green).ToString();
            textBox31.Text = (move[comboBox1.SelectedIndex].red).ToString();
            textBox32.Text = move[comboBox1.SelectedIndex].scale.ToString();

            textBox33.Text = (emif[comboBox1.SelectedIndex].count).ToString();
            textBox34.Text = (emif[comboBox1.SelectedIndex].play_delay).ToString();
            textBox35.Text = (emif[comboBox1.SelectedIndex].repeating).ToString();
           
            textBox36.Text = (move[comboBox1.SelectedIndex].angle).ToString();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            move[comboBox1.SelectedIndex].alpha = float.Parse(textBox28.Text);
            move[comboBox1.SelectedIndex].blue = float.Parse(textBox29.Text);
            move[comboBox1.SelectedIndex].green = float.Parse(textBox30.Text);
            move[comboBox1.SelectedIndex].red = float.Parse(textBox31.Text);
            move[comboBox1.SelectedIndex].scale = float.Parse(textBox32.Text);

            move[comboBox1.SelectedIndex].angle = float.Parse(textBox36.Text);


            emif[comboBox1.SelectedIndex].count = Convert.ToInt16(textBox33.Text);
            emif[comboBox1.SelectedIndex].play_delay = Convert.ToInt16(textBox34.Text);
            emif[comboBox1.SelectedIndex].repeating = Convert.ToInt16(textBox35.Text);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string filePath = textBox27.Text;
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Open)))
            {
                // Записываем структуры move
                for (int i = 0; i < header.recordCount; i++)
                {
                    for (int j = 0; j < 23; j++)
                    {
                        writer.BaseStream.Seek(offsets[i] + typeGroup[i].types[j].offset, SeekOrigin.Begin);

                        if (typeGroup[i].types[j].id == "MOVE")
                        {

                            long position = offsets[i] + typeGroup[i].types[j].offset;
                            long originalPosition = writer.BaseStream.Position;
                            writer.BaseStream.Seek(position, SeekOrigin.Begin);

                            using (var memStream = new MemoryStream())
                            {
                                using (var moveWriter = new BinaryWriter(memStream))
                                {
                                    WriteStruct(moveWriter, move[i]);
                                }
                                byte[] data = memStream.ToArray();
                                writer.Write(data);
                            }
                            writer.BaseStream.Seek(originalPosition, SeekOrigin.Begin);
                        }

                        if (typeGroup[i].types[j].id == "EMIF")
                        {

                            long position = offsets[i] + typeGroup[i].types[j].offset;
                            long originalPosition = writer.BaseStream.Position;
                            writer.BaseStream.Seek(position, SeekOrigin.Begin);

                            using (var memStream = new MemoryStream())
                            {
                                using (var emifWriter = new BinaryWriter(memStream))
                                {
                                    WriteStruct(emifWriter, emif[i]);
                                }
                                byte[] data = memStream.ToArray();
                                writer.Write(data);
                            }
                            writer.BaseStream.Seek(originalPosition, SeekOrigin.Begin);
                        }
                    }
                }
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "est files (*.est)|*.est|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    EffectReader(openFileDialog.FileName);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Получение полных путей к файлам
            foreach (string file in files)
            {
                unpackDat(file, multipleDatUnpack);
            }
        }

        private void panel3_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Получение полных путей к файлам
            foreach (string file in files)
            {
                 BxmXml(file, "bxmToXml");
            }

            
        }

        private void panel3_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void panel4_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Получение полных путей к файлам
            foreach (string file in files)
            {
                BxmXml(file, "xmlToBxm");
            }
        }

        private void panel4_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Получение полных путей к файлам
            foreach (string file in files)
            {
                repackDat(file, false);
            }
            
        }

        private void panel5_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void panel5_DragDrop(object sender, DragEventArgs e)
        {
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBox27.Text = file[0];
            EffectReader(file[0]);
        }
    }
}
