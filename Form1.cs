using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace MGRRDat
{

    public partial class Form1 : Form
    {

   

        string NewEmSetRoot = "//NewEmSetRoot";
        string pathToEnemyGroup = "CorpsRoot/MemberList/GroupList[contains(@name, 'GROUP_00')]/diffInfo/EnemyList";

        string[] IDs = new string[100];
        string[] Transes = new string[100];
        string[] SetTypes = new string[100];
        string[] Types = new string[100];



        string pathToBxmXml = "";
        string treeViewPath = "";

        int firstIndex = -1;
        int secondIndex = -1;
        int thirdIndex = -1;


        XmlDocument XmlDoc = new XmlDocument();
        XmlNode primary;
        XmlNode idNodes;
        XmlNodeList enemies;

        string langugage = "eng";
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            unpackDat("");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            repackDat("");
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

        public void repackDat(string folderPath)
        {
            if (folderPath == "") folderPath = textBox2.Text;
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

                XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ItemAlias"].Value = ItemAliasTextBox.Text;
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

                IDTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Id"].Value;
                RoomTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Room"].Value;
                BaseRotTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["BaseRot"].Value;
                BaseRotLTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["BaseRotL"].Value;
                TransTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Trans"].Value;
                TransLTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["TransL"].Value;



                RotationTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Rotation"].Value;
                SetTypeTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["SetType"].Value;
                TypeTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Type"].Value;

                SetRtnTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["SetRtn"].Value;
                SetFlagTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["SetFlag"].Value;
                PathNoTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["PathNo"].Value;
                WaypointNoTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["WaypointNo"].Value;
                SetWaitTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["SetWait"].Value;
                HpTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Hp"].Value;



                ParamTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Param"].Value;
                BezierNoTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["BezierNo"].Value;

                ParentIdTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ParentId"].Value;
                PartsNoTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["PartsNo"].Value;
                HashNoTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["HashNo"].Value;
                ItemIdTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ItemId"].Value;
                GroupPosTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["GroupPos"].Value;


                InitialRtnTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["InitialRtn"].Value;
                InitialPosTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["InitialPos"].Value;
                InitialPosDirYTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["InitialPosDirY"].Value;
                InitialTimeTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["InitialTime"].Value;

                ItemAliasTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ItemAlias"].Value;
                Free0TextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["Free0"].Value;
                DropItemNormalTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["DropItemNormal"].Value;
                DropItemStealthTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["DropItemStealth"].Value;
                VisceraTableNoTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["VisceraTableNo"].Value;
                ReflexViewAngYTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ReflexViewAngY"].Value;

                ReflexViewAngXTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ReflexViewAngX"].Value;
                ReflexViewDistTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ReflexViewDist"].Value;
                ScoutViewAngYTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ScoutViewAngY"].Value;
                ScoutViewAngXTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ScoutViewAngX"].Value;
                ScoutViewDistTextBox.Text = XmlDoc.SelectSingleNode(NewEmSetRoot).ChildNodes[firstIndex].SelectSingleNode("CorpsRoot/MemberList").ChildNodes[secondIndex].SelectSingleNode("diffInfo/EnemyList").ChildNodes[thirdIndex].Attributes["ScoutViewDist"].Value;





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
    }
}
