using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnumMaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Name = "Fool";

            Init();
            Init_Label();
            Init_InputBox();
            Init_FileLoader_Button();
        }

        private System.Windows.Forms.Label m_textLabel_filenameExtension;        
        private System.Windows.Forms.Label m_textLabel_EnumName;        
        private System.Windows.Forms.Label m_textLabel_saveFileName;        
        private System.Windows.Forms.TextBox m_inputBox_filenameExtension;
        private System.Windows.Forms.TextBox m_inputBox_EnumName;        
        private System.Windows.Forms.TextBox m_inputBox_saveFileName;        
        private System.Windows.Forms.Button m_fileloader_button;

        private Size m_windowSize;
        public void Init()
        {
            m_windowSize = new Size(200, 220);
            this.ClientSize = m_windowSize;
        }
        public void Init_Label()
        {
            m_textLabel_filenameExtension = new Label();
            m_textLabel_filenameExtension.Name = "TextLabel1";
            m_textLabel_filenameExtension.Size = new Size(120, 20);
            m_textLabel_filenameExtension.Text = "Filename Extension";
            m_textLabel_filenameExtension.Location = new Point(40, 20);
            this.Controls.Add(m_textLabel_filenameExtension);

            m_textLabel_EnumName = new Label();
            m_textLabel_EnumName.Name = "TextLabel2";
            m_textLabel_EnumName.Size = new Size(120, 20);
            m_textLabel_EnumName.Text = "Enum Type Name";
            m_textLabel_EnumName.Location = new Point(40, 70);
            this.Controls.Add(m_textLabel_EnumName);

            m_textLabel_saveFileName = new Label();
            m_textLabel_saveFileName.Name = "TextLabel3";
            m_textLabel_saveFileName.Size = new Size(120, 20);
            m_textLabel_saveFileName.Text = "Save File Name";
            m_textLabel_saveFileName.Location = new Point(40, 120);
            this.Controls.Add(m_textLabel_saveFileName);
        }
        public void Init_InputBox()
        {
            m_inputBox_filenameExtension = new TextBox();
            m_inputBox_filenameExtension.Name = "InputBox1";
            m_inputBox_filenameExtension.Size = new Size(120, 20);
            m_inputBox_filenameExtension.Text = ".prefab";
            m_inputBox_filenameExtension.Location = new Point(40, 40);
            this.Controls.Add(m_inputBox_filenameExtension);            

            m_inputBox_EnumName = new TextBox();
            m_inputBox_EnumName.Name = "InputBox2";
            m_inputBox_EnumName.Size = new Size(120, 20);
            m_inputBox_EnumName.Text = "E_oooType";
            m_inputBox_EnumName.Location = new Point(40, 90);
            this.Controls.Add(m_inputBox_EnumName);

            m_inputBox_saveFileName = new TextBox();
            m_inputBox_saveFileName.Name = "InputBox3";
            m_inputBox_saveFileName.Size = new Size(120, 20);
            m_inputBox_saveFileName.Text = "Test.txt";
            m_inputBox_saveFileName.Location = new Point(40, 140);
            this.Controls.Add(m_inputBox_saveFileName);
        }
        public void Init_FileLoader_Button()
        {
            m_fileloader_button = new Button();
            m_fileloader_button.Name = "FileLoader_Button";
            m_fileloader_button.Size = new Size(120, 20);
            m_fileloader_button.Text = "Open File Dialog";
            m_fileloader_button.Location = new Point(40, 185);
            this.Controls.Add(m_fileloader_button);

            m_fileloader_button.Click += ButtonClick;
        }



        private string fileNmae_Extension;
        private string folderPath;
        private List<string> fileList;
        private List<string> resultList;
        void ButtonClick(object sender, EventArgs e)
        {
            fileNmae_Extension = m_inputBox_filenameExtension.Text;
            if (fileNmae_Extension[0] != '.')
            {
                MessageBox.Show(".을 포함한 확장자를 입력하세요!");
                return;
            }

            OpenDialog();
            Make_OutPutList();
            Make_OutPutFormat();
            SaveData();
            feedback();
        }

        public void OpenDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (DialogResult.OK == dialog.ShowDialog())
            {
                folderPath = dialog.SelectedPath;                
            }
        }

        public void Make_OutPutList()
        {
            fileList = File_Extentsion.GetFileList(folderPath, fileNmae_Extension);

            int count = 0;
            resultList = new List<string>();
            foreach (var item in fileList)
            {
                // item 이 00_FileName.prefab 파일 일 경우

                // 0 : 00
                // 1 : FileName.prefab
                string[] seperated = item.mySplit('_');
                if (seperated == null)
                {
                    MessageBox.Show("파일 이름이 규정과 다릅니다 (ex : 00_None.prefab)");
                    return;
                }

                // 0 : FileName
                // 1 : prefab
                seperated = seperated[1].mySplit('.');

                // output : FileName = 0
                string output_str = string.Format("{0} = {1}", seperated[0], count.ToString());
                ++count;
                resultList.Add(output_str);
            }
        }

        string saveString;

        public void Make_OutPutFormat()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("public enum ");
            sb.Append(m_inputBox_EnumName.Text);
            sb.Append('\n');
            sb.Append("{\n");

            foreach (var item in resultList)
            {
                sb.Append('\t');
                sb.Append(item);
                sb.Append(",\n");
            }
    
            sb.Append("\n\tMax\n");
            sb.Append("}\n");

            saveString = sb.ToString();
        }

        public void SaveData()
        {
            File_Extentsion.SaveStr(m_inputBox_saveFileName.Text, "Output", saveString);
        }

        public void feedback()
        {
            File_Extentsion.OpenFile(m_inputBox_saveFileName.Text, "Output");
        }
    }
}
