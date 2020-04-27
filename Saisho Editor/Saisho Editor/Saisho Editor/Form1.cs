using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;
using AboControls.UserControls;
using System.Text.RegularExpressions;
using System.Collections;

namespace Saisho_Editor
{

    public partial class MainForm : Form
    {
        bool und = false;
        bool red = false;
        public MainForm()
        {
            InitializeComponent();
        }
        
        private void MainForn_Load(object sender, EventArgs e)
        {
            
            newToolStripMenuItem_Click(sender, e);
            getRichTextBox().Select();
            
        }
        int u = 0;
        public void MtxtC(object sender, EventArgs e)
        {
          
            Syntaxhighlight();und = false;
        }
       
       
        //Function that sends the Richtextbox object of the current selected tab
        public RichTextBox getRichTextBox()
        {
            NumberedRTB rtb = null;
            TabPage tp = tabControl1.SelectedTab;
            if (tp != null)
            {
                rtb = tp.Controls[0] as NumberedRTB;
            }
            return rtb.RichTextBox;
        }
        public Stack<string> getUndolist()
        {
            NumberedRTB rtb = null;
            TabPage tp = tabControl1.SelectedTab;
            if (tp != null)
            {
                rtb = tp.Controls[0] as NumberedRTB;
            }
            
            return rtb.undoList;
        }
        public Stack<string> getRedolist()
        {
            NumberedRTB rtb = null;
            TabPage tp = tabControl1.SelectedTab;
            if (tp != null)
            {
                rtb = tp.Controls[0] as NumberedRTB;
            }
            return rtb.redoList;
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tp = new TabPage("Untitled Document");
            NumberedRTB rtb = new NumberedRTB();
            rtb.Dock = DockStyle.Fill;
            rtb.RichTextBox.Multiline = true;
            rtb.RichTextBox.WordWrap = false;
            rtb.RichTextBox.ScrollBars = RichTextBoxScrollBars.Both;
            
            rtb.RichTextBox.ContextMenuStrip = contextMenuStrip1;
            rtb.RichTextBox.TextChanged += new EventHandler(MtxtC);
            tp.Controls.Add(rtb);
            tabControl1.TabPages.Add(tp);
           
           
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getRichTextBox().Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getRichTextBox().Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getRichTextBox().Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getRichTextBox().SelectAll();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(u==0)
            {
                if (getUndolist().Count != 0)
                    getUndolist().Pop();
                u = 1;
            }
            if (getUndolist().Count != 0)
            {
                String s= getUndolist().Pop();
                getRichTextBox().Text = s;
                getRedolist().Push(s);
                getRichTextBox().Select(getRichTextBox().Text.Length, 0);
            }

            else
                getRichTextBox().Text = "";
            und = true;
        }
        
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (getRedolist().Count != 0)
            {
                if (u == 1)
                {
                    getRedolist().Pop();
                    u = 4;
                }

                red = true;
                getRichTextBox().Text = getRedolist().Pop();
               // getRichTextBox().Select(getRichTextBox().Text.Length, 0);
            }
        }
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            getRichTextBox().Copy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            getRichTextBox().Paste();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            getRichTextBox().Cut();
        }

        private void findToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            findToolStripMenuItem_Click(sender, e);
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);und = true;
        }
        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            getRichTextBox().SelectAll();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File (.txt)|*.txt|Java Files (.java)|*.java|C Files (.c)|*.c|C++ Files (.cpp)|*.cpp|C# Files (.cs)|*.cs|Python Files (.py)|*.py";
            ofd.Title = "Select a File";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                TabPage tp = new TabPage(Path.GetFileName(ofd.FileName));
                NumberedRTB rtb = new NumberedRTB();
                rtb.Dock = DockStyle.Fill;
                rtb.RichTextBox.Multiline = true;
                rtb.RichTextBox.WordWrap = false;
                rtb.RichTextBox.ScrollBars = RichTextBoxScrollBars.Both;
                rtb.RichTextBox.ContextMenuStrip = contextMenuStrip1;
                rtb.RichTextBox.TextChanged += new EventHandler(MtxtC);

                System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
                rtb.RichTextBox.Text = sr.ReadToEnd();
                tp.Controls.Add(rtb);
                tabControl1.TabPages.Add(tp);
                sr.Close();
                tabControl1.SelectedTab = tp;
                tabControl1.SelectedTab.Name = ofd.FileName;

                
            }
           

        }
        //SaveAs Function
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File (.txt)|*.txt|Java Files (.java)|*.java|C Files (.c)|*.c|C++ Files (.cpp)|*.cpp|C# Files (.cs)|*.cs|Python Files (.py)|*.py";
            sfd.Title = "Save File";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                tabControl1.SelectedTab.Text = Path.GetFileName(sfd.FileName);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName);
                tabControl1.SelectedTab.Name = sfd.FileName;
                sw.Write(getRichTextBox().Text);
                sw.Close();
                getRichTextBox().Modified = false;
            }
        }
        //Save function
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = tabControl1.SelectedTab.Name;
            try
            {
                if (filename == "")
                {
                    saveAsToolStripMenuItem_Click(sender, e);
                }
                else
                    using (StreamWriter sw = new StreamWriter(filename))
                    {
                        sw.Write(getRichTextBox().Text);
                        sw.Close();
                        getRichTextBox().Modified = false;
                    }
            }
            catch (Exception E)
            {
                MessageBox.Show("Failed to save file.\n" + E.Message);
            }     
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (tabControl1.TabCount!=1)
            {
                if (getRichTextBox().Modified)
                {
                    DialogResult result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        getRedolist().Clear();
                        getUndolist().Clear();
                        saveToolStripMenuItem_Click(sender, e);
                    }
                    else if (result == DialogResult.No)
                    {
                        getRedolist().Clear();
                        getUndolist().Clear();
                        tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex); }
                    else if (result == DialogResult.Cancel)
                    { }
                }
                else
                    tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);


            }
            else
            {
                if (getRichTextBox().Modified)
                {
                    DialogResult result = MessageBox.Show("Do you want to save changes and Exit?", "Confirmation", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        getRedolist().Clear();
                        getUndolist().Clear();
                        saveToolStripMenuItem_Click(sender, e);
                    }
                    else if (result == DialogResult.No)
                    {

                        Application.Exit();
                    }
                    else if (result == DialogResult.Cancel)
                    {
                    }
                }
                else
                    Application.Exit();
            }

        }
        //LineNumber is done here
        //********************************************************************************************************************
       
      
        private void tabControl_tabindexchanged(object sender, EventArgs e)
        {
            wordstart = -1;
            ErrorTextbox.Text = "";
            


        } 
        //********************************************************************************************************************
        //******************************************************************************************************************
        //Find and Replace part
        public FindForm f;
        int start = 0;
        
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f = new FindForm();
            f.Show();
            f.button2.Enabled = false;
            f.button1.Click += new EventHandler(Find_All_Click);
            f.FormClosed += new FormClosedEventHandler (Find_Close);
            f.button3.Click += new EventHandler(Find_Only);
            f.richTextBox1.TextChanged += new EventHandler(Txt_changed);
            f.button2.Click += new EventHandler(Find_nxt);

        }
        public void Txt_changed(object sender,EventArgs e)
        {
           
            getRichTextBox().SelectAll();
            getRichTextBox().SelectionColor = Color.Black;
            getRichTextBox().SelectionBackColor = Color.White;
            getRichTextBox().DeselectAll();
            start = 0;
            f.button2.Enabled = false;
        }
        public void Find_Close(object sender, EventArgs e)
        {

            int initial = tabControl1.SelectedIndex;
            int i = tabControl1.TabCount;
                for (int j = 0; j < i; j++)
                    {
                tabControl1.SelectTab(j);
                getRichTextBox().SelectAll();
                getRichTextBox().SelectionColor = Color.Black;
                getRichTextBox().SelectionBackColor = Color.White;
                getRichTextBox().DeselectAll();
                    }
                    tabControl1.SelectTab(initial);
            start = 0;
            f = null;

        }

        public void Find_nxt(object sender,EventArgs e)
        {
           
                int wordstartindex = getRichTextBox().Find(f.richTextBox1.Text, start, RichTextBoxFinds.None);
                if (wordstartindex != -1)
                {
                    getRichTextBox().SelectionStart = wordstartindex;
                    getRichTextBox().SelectionLength = f.richTextBox1.Text.Length;
                    getRichTextBox().SelectionColor = Color.Yellow;
                    getRichTextBox().SelectionBackColor = Color.Black;
                    f.button2.Enabled = true;
                    start = wordstartindex + f.richTextBox1.Text.Length;
                getRichTextBox().ScrollToCaret();
                }
            
        }
        public void Find_Only(object sender, EventArgs e)
        {
            start = 0;
            getRichTextBox().SelectAll();
            getRichTextBox().SelectionColor = Color.Black;
            getRichTextBox().SelectionBackColor = Color.White;
            getRichTextBox().DeselectAll();

            int wordstartindex = getRichTextBox().Find(f.richTextBox1.Text, start, RichTextBoxFinds.None);
                if (wordstartindex != -1)
                {
                    getRichTextBox().SelectionStart = wordstartindex;
                    getRichTextBox().SelectionLength = f.richTextBox1.Text.Length;
                    getRichTextBox().SelectionColor = Color.Yellow;
                    getRichTextBox().SelectionBackColor = Color.Black;
                    f.button2.Enabled = true;
                    start = wordstartindex + f.richTextBox1.Text.Length;
                getRichTextBox().ScrollToCaret();
            }
            else
                MessageBox.Show("Word not found!");

        }
        public void Find_All_Click(object sender, EventArgs e)
        {

            HighlightWords(f.richTextBox1.Text);   
        }
        public void HighlightWords(String words)
        {
          
           
                int startIndex = 0;
                while(startIndex < getRichTextBox().TextLength)
                {    
                
                    int wordstartindex = getRichTextBox().Find(words, startIndex, RichTextBoxFinds.None);
                    if (wordstartindex != -1)
                    {
                        getRichTextBox().SelectionStart = wordstartindex;
                        getRichTextBox().SelectionLength = words.Length;
                        getRichTextBox().SelectionColor = Color.Yellow;
                        getRichTextBox().SelectionBackColor = Color.Black;
                        getRichTextBox().ScrollToCaret();
                }
                    else
                    {
                        
                         break;
                    }
                    startIndex = wordstartindex + words.Length;
                  
                }
                
            
        }
        //find ends here
        //****************************************************************************************************************

        //Replace Part Starts
        ReplaceForm r;
        int srt;
        int index;
        int wordstart = -1;
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            r = new ReplaceForm();

            r.Show();
            r.findnxt.Enabled = false;
            r.replace.Enabled = false;
            r.replaceAll.Enabled = false;
            r.find.Click += new EventHandler(RFind);
            r.findnxt.Click += new EventHandler(RFindNxt);
            r.richTextBox1.TextChanged += new EventHandler(BoxChange);
            r.FormClosed += new FormClosedEventHandler(RFind_Close);
            r.replace.Click += new EventHandler(Replace);
            r.replaceAll.Click += new EventHandler(ReplaceAll);
        }
        public void RFind_Close(object sender, EventArgs e)
        {

            int initial = tabControl1.SelectedIndex;
            int i = tabControl1.TabCount;
            for (int j = 0; j < i; j++)
            {
                tabControl1.SelectTab(j);
                getRichTextBox().SelectAll();
                getRichTextBox().SelectionColor = Color.Black;
                getRichTextBox().SelectionBackColor = Color.White;
                getRichTextBox().DeselectAll();
            }
            tabControl1.SelectTab(initial);
            srt = 0;
            r = null;

        }
        public void BoxChange(object sender,EventArgs e)
        {

            getRichTextBox().SelectAll();
            getRichTextBox().SelectionColor = Color.Black;
            getRichTextBox().SelectionBackColor = Color.White;
            getRichTextBox().DeselectAll();
            r.findnxt.Enabled = false;
            r.replace.Enabled = false;
            r.replaceAll.Enabled = false;
            
        }
        public void RFind(object sender,EventArgs e)
        {
            getRichTextBox().SelectAll();
            getRichTextBox().SelectionColor = Color.Black;
            getRichTextBox().SelectionBackColor = Color.White;
            getRichTextBox().DeselectAll();
            srt = 0;
            wordstart = getRichTextBox().Find(r.richTextBox1.Text, srt, RichTextBoxFinds.MatchCase);

            if (wordstart != -1)
            {
                index = wordstart;
                getRichTextBox().SelectionStart = wordstart;
                getRichTextBox().SelectionLength = r.richTextBox1.Text.Length;
                getRichTextBox().SelectionColor = Color.Yellow;
                getRichTextBox().SelectionBackColor = Color.Black;
                r.replace.Enabled = true;
                srt = wordstart + r.richTextBox1.Text.Length;
                getRichTextBox().ScrollToCaret();
                r.findnxt.Enabled = true;
                r.replace.Enabled = true;
                r.replaceAll.Enabled = true;
            }
            else
                MessageBox.Show("Word not found!");
        }
        public void RFindNxt(object sender , EventArgs e)
        {
             wordstart = getRichTextBox().Find(r.richTextBox1.Text, srt, RichTextBoxFinds.MatchCase);
            if (wordstart != -1)
            {
                index = wordstart;
                getRichTextBox().SelectionStart = wordstart;
                getRichTextBox().SelectionLength = r.richTextBox1.Text.Length;
                getRichTextBox().SelectionColor = Color.Yellow;
                getRichTextBox().SelectionBackColor = Color.Black;
                srt = wordstart + r.richTextBox1.Text.Length;
                getRichTextBox().ScrollToCaret();
            }
        }
        public void Replace(object sender, EventArgs e)
        {
            if(wordstart!=-1)
            {
                getRichTextBox().Text = getRichTextBox().Text.Remove(index, r.richTextBox1.Text.Length);
                
                if (index == 0)
                {
                    getRichTextBox().SelectAll();
                    getRichTextBox().SelectionColor = Color.Black;
                    getRichTextBox().SelectionBackColor = Color.White;
                    getRichTextBox().DeselectAll();
                }
                getRichTextBox().Text=getRichTextBox().Text.Insert(index, r.richTextBox2.Text);
                wordstart = -1;
            }

        }
        public void ReplaceAll(object sender, EventArgs e)
        {
            int startIdex = 0;
            while (startIdex < getRichTextBox().TextLength)
            {

                int wordstartinde = getRichTextBox().Find(r.richTextBox1.Text, startIdex, RichTextBoxFinds.MatchCase);
                if (wordstartinde != -1)
                {
                    getRichTextBox().Text = getRichTextBox().Text.Remove(wordstartinde, r.richTextBox1.Text.Length);
                    getRichTextBox().Text = getRichTextBox().Text.Insert(wordstartinde, r.richTextBox2.Text);
                    if (wordstartinde == 0)
                    {
                        getRichTextBox().SelectAll();
                        getRichTextBox().SelectionColor = Color.Black;
                        getRichTextBox().SelectionBackColor = Color.White;
                        getRichTextBox().DeselectAll();
                    }
                    getRichTextBox().ScrollToCaret();
                }
                else
                {

                    break;
                }
                startIdex = wordstartinde + r.richTextBox2.Text.Length;
               
            }r.replace.Enabled = false;
            r.replaceAll.Enabled = false;
            r.findnxt.Enabled = false;
        }



        //replace ends here
        //*********************************************************************************************************


        /// <param name="EXE">program which you want to run (javac.exe)</param>
        /// <param name="WorkingDirectory">directory name where .java file is stored</param>
        /// <param name="FileName">.java file name</param>
        /// <returns></returns>
        /// public Process process = new Process();
        /// 

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name != "")
            {
                filecheck(false);
            }
            else
            {
                DialogResult result = MessageBox.Show("File not saved. Do you want to save and continue ?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    saveToolStripMenuItem_Click(sender, e);
                else if (result == DialogResult.No)
                {

                }
            }

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name != "")
            {
                filecheck(true);
            }
            else
            {
                DialogResult result = MessageBox.Show("File not saved. Do you want to save and continue ?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    saveToolStripMenuItem_Click(sender, e);
                else if (result == DialogResult.No)
                {

                }
            }
        }

        public void filecheck(bool output)
        {

            if (Path.GetExtension(tabControl1.SelectedTab.Name) == ".java")
                CompileJava(tabControl1.SelectedTab.Name, System.Configuration.ConfigurationManager.AppSettings["jdkpath"], output);
            else if (Path.GetExtension(tabControl1.SelectedTab.Name) == ".c")
                compileC(tabControl1.SelectedTab.Name, System.Configuration.ConfigurationManager.AppSettings["gccpath"], output);
            else if (Path.GetExtension(tabControl1.SelectedTab.Name) == ".cpp")
                compileCpp(tabControl1.SelectedTab.Name, System.Configuration.ConfigurationManager.AppSettings["gccpath"], output);
            else if (Path.GetExtension(tabControl1.SelectedTab.Name) == ".cs")
                CompileCS(tabControl1.SelectedTab.Name, System.Configuration.ConfigurationManager.AppSettings["CSpath"], output);
            else if (Path.GetExtension(tabControl1.SelectedTab.Name) == ".py")
                CompilePython(tabControl1.SelectedTab.Name, System.Configuration.ConfigurationManager.AppSettings["Pythonpath"], output);
            else if (Path.GetExtension(tabControl1.SelectedTab.Name) == ".txt")
                MessageBox.Show("Text file cannot be compiled or run!");
        }


        public Process process;
        public StreamReader ErrorReader;
        public bool Compile(String EXE, String WorkingDirectory, String FileName,Process process)
        {
            ErrorTextbox.Text = "";
           
            bool processStarted = false;

            if (File.Exists(EXE))
            {
                process.StartInfo.FileName = EXE;
                if (FileName.Contains(".py"))
                process.StartInfo.Arguments = "-m py_compile " +FileName;
                else
                process.StartInfo.Arguments = FileName;
                process.StartInfo.WorkingDirectory = WorkingDirectory;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.ErrorDialog = false;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                processStarted = process.Start();
            }
            else
            {
                MessageBox.Show("Unable to compile file. Check your Compiler Path settings. ");
            }
            return processStarted;
        }
        //function to compile and get output of java
        public void CompileJava(String file, String jdkpath, bool output)
        {
            process = new Process();
            
            if (Compile(jdkpath + "\\bin\\javac.exe", Path.GetDirectoryName(file), Path.GetFileName(file),process))
            {
                ErrorReader = process.StandardError;
                string response = ErrorReader.ReadToEnd();

                if (response != "")
                {
                    ErrorTextbox.Text = response;
                }
                else if (response == "")
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
                else if (response.Contains("uses or overrides a deprecated API."))
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
                else
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select jdk path";
                fbd.ShowNewFolderButton = false;
                fbd.ShowDialog();
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["jdkpath"].Value = fbd.SelectedPath;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            //this is Here is used to get the output
            if (output == true)
            {
                if (ErrorTextbox.Text.Contains("Program Compiled Successfully!"))
                {
                    ProcessStartInfo ProcessInfo;
                    String javapath = jdkpath + "\\bin";

                    String getfilename = file.Substring(file.LastIndexOf("\\") + 1);
                    String fname = "";
                    if (getfilename.Contains(".java"))
                    {
                        fname = getfilename.Remove(getfilename.Length - 5);
                    }
                    
                    ProcessInfo = new ProcessStartInfo("cmd.exe", "/K" + "cd/ &&  cd /d " + Path.GetDirectoryName(file) + "  &&  set path=%path%;" + javapath + "  & java " + fname);
                    ProcessInfo.CreateNoWindow = true;
                    ProcessInfo.UseShellExecute = true;
                    Process.Start(ProcessInfo);
                    process = null;
                    ProcessInfo = null;
                }
            }
            else
            {
                process = null;
            }

        }
        public void compileC(String file, String gccpath, bool output)
        {
            process = new Process();
            
            
            if (Compile(gccpath + "\\bin\\gcc.exe", Path.GetDirectoryName(file), Path.GetFileName(file), process))
            {
                ErrorReader = process.StandardError;
                string response = ErrorReader.ReadToEnd();

                if (response != "")
                {
                    ErrorTextbox.Text = response;
                }
                else if (response == "")
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
               
                else
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select GCC path";
                fbd.ShowNewFolderButton = false;
                fbd.ShowDialog();
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["gccpath"].Value = fbd.SelectedPath;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            //this is Here is used to get the output
            if (output == true)
            {
                if (ErrorTextbox.Text.Contains("Program Compiled Successfully!"))
                {
                    ProcessStartInfo ProcessInfo;
                    ProcessInfo = new ProcessStartInfo("cmd.exe", "/K" + "cd/ &&  cd /d " + Path.GetDirectoryName(file)  +"&& a");
                    ProcessInfo.CreateNoWindow = true;
                    ProcessInfo.UseShellExecute = true;
                    Process.Start(ProcessInfo);
                    process = null;
                    ProcessInfo = null;
                }
            }
            else
            process = null;
        }
        public void compileCpp(String file, String gccpath, bool output)
        {
            process = new Process();


            if (Compile(gccpath + "\\bin\\g++.exe", Path.GetDirectoryName(file), Path.GetFileName(file), process))
            {
                ErrorReader = process.StandardError;
                string response = ErrorReader.ReadToEnd();

                if (response != "")
                {
                    ErrorTextbox.Text = response;
                }
                else if (response == "")
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }

                else
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select GCC-MinGW Installation path";
                fbd.ShowNewFolderButton = false;
                fbd.ShowDialog();
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["gccpath"].Value = fbd.SelectedPath;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            //this is Here is used to get the output
            if (output == true)
            {
                if (ErrorTextbox.Text.Contains("Program Compiled Successfully!"))
                {
                    ProcessStartInfo ProcessInfo;
                    ProcessInfo = new ProcessStartInfo("cmd.exe", "/K" + "cd/ &&  cd /d " + Path.GetDirectoryName(file) + "&& a");
                    ProcessInfo.CreateNoWindow = true;
                    ProcessInfo.UseShellExecute = true;
                    Process.Start(ProcessInfo);
                    process = null;
                    ProcessInfo = null;
                }
            }
            else
                process = null;
        }

        public void CompileCS(String file, String CSpath, bool output)
        {
            process = new Process();

            if (Compile(CSpath + "\\csc.exe", Path.GetDirectoryName(file), Path.GetFileName(file), process))
            {
                ErrorReader = process.StandardError;
                string response = ErrorReader.ReadToEnd();

                if (response != "")
                {
                    ErrorTextbox.Text = response;
                }
                else if (response == "")
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
                else if (response.Contains("uses or overrides a deprecated API."))
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
                else
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select C# Compiler folder in C:\\Windows\\Microsoft.NET\\Framework\\Version path";
                fbd.ShowNewFolderButton = false;
                fbd.ShowDialog();
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["CSpath"].Value = fbd.SelectedPath;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            //this is Here is used to get the output
            if (output == true)
            {
                if (ErrorTextbox.Text.Contains("Program Compiled Successfully!"))
                {
                    ProcessStartInfo ProcessInfo;
                    String getfilename = file.Substring(file.LastIndexOf("\\") + 1);
                    String fname = "";
                    if (getfilename.Contains(".cs"))
                    {
                        fname = getfilename.Remove(getfilename.Length - 3);
                    }
                    ProcessInfo = new ProcessStartInfo("cmd.exe", "/K" + "cd/ &&  cd /d " + Path.GetDirectoryName(file)+"&&"+ fname+".exe");
                    ProcessInfo.CreateNoWindow = true;
                    ProcessInfo.UseShellExecute = true;
                    Process.Start(ProcessInfo);
                    process = null;
                    ProcessInfo = null;
                }
            }
            else
            {
                process = null;
            }

        }

        public void CompilePython(String file, String pythonpath, bool output)
        {
            process = new Process();

            if (Compile(pythonpath + "\\python.exe", Path.GetDirectoryName(file), Path.GetFileName(file), process))
            {
                ErrorReader = process.StandardError;
                string response = ErrorReader.ReadToEnd();

                if (response != "")
                {
                    ErrorTextbox.Text = response;
                }
                else if (response == "")
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
               
                else
                {
                    ErrorTextbox.Text = "Program Compiled Successfully!";
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select Python File Directory eg-C:\\Users\\UserName\\AppData\\Local\\Programs\\Python\\Python37-32 (tip - if not found try after selecting Show Hidden Files in File Explorer)";
                fbd.ShowNewFolderButton = false;
                fbd.ShowDialog();
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Pythonpath"].Value = fbd.SelectedPath;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            //this is Here is used to get the output
            if (output == true)
            {
                if (ErrorTextbox.Text.Contains("Program Compiled Successfully!"))
                {
                    ProcessStartInfo ProcessInfo;
                    
                    String getfilename = file.Substring(file.LastIndexOf("\\") + 1);
                    String fname = "";
                    if (getfilename.Contains(".py"))
                    {
                        fname = getfilename.Remove(getfilename.Length - 3);
                    }

                    String pyver;
                    pyver = pythonpath.Substring(pythonpath.LastIndexOf("n")+1) ;
                    pyver = pyver.Substring(0, 2);
                    
                    ProcessInfo = new ProcessStartInfo("cmd.exe", "/K" + "cd/ &&  cd /d " + Path.GetDirectoryName(file)+ "\\__pycache__  && " + fname+ ".cpython-"+pyver+".pyc");
                    ProcessInfo.CreateNoWindow = true;
                    ProcessInfo.UseShellExecute = true;
                    Process.Start(ProcessInfo);
                    process = null;
                    ProcessInfo = null;
                }
            }
            else
            {
                process = null;
            }
        }


        //************************************************************************************************************************
        //************************************************************************************************************************

        public void Syntaxhighlight()
        {
            
            if (und == false)
            {
                if(red==false)
                getRedolist().Clear();
                getUndolist().Push(getRichTextBox().Text);u = 0;
            }

        }

       
    }
}