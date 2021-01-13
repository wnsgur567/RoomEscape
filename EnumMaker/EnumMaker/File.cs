using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnumMaker
{
    public static class String_Extension
    {
        public static string[] mySplit(this string str,char seperator)
        {
            int length = str.Length;
            int seperator_pos = -1;
            for (int i = 0; i < length; i++)
            {
                if(str[i] == seperator)
                {
                    seperator_pos = i;
                    break;
                }
            }

            if (seperator_pos == -1)
                return null;

            string[] ret = new string[2];
            ret[0] = str.Substring(0, seperator_pos);
            ret[1] = str.Substring(seperator_pos + 1, length - seperator_pos - 1);

            return ret;
        }
    }


    public static class File_Extentsion
    {
        // 파일 경로 내의 모든 파일 이름을 가져옴
        public static List<string> GetFileList(string _folderPath,string _filename_extension)
        {
            List<string> retList = new List<string>();
           
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(_folderPath);
            foreach (System.IO.FileInfo File in di.GetFiles())
            {
                if (File.Extension.ToLower().CompareTo(_filename_extension) == 0)
                {
                    String FullFileName = File.Name;

                    retList.Add(FullFileName);
                }
            }

            retList.Sort();
            return retList;
        }

        public static void SaveStr(string fileName, string output_foldername,string saveData)
        {   //저장
            string currentPath = Environment.CurrentDirectory;
            string save_path = System.IO.Path.Combine(currentPath, output_foldername , fileName);

            System.IO.FileInfo f_imfo = new System.IO.FileInfo(save_path);
            if (f_imfo.Exists)
            {
                if (DialogResult.No == MessageBox.Show("파일을 덮어 쓰시겠습니까?", "확인", MessageBoxButtons.YesNo))
                {
                    MessageBox.Show("저장 취소");
                    return;
                }
            }

            System.IO.File.WriteAllText(save_path, saveData);
            MessageBox.Show("Save Complete");
        }

        public static void OpenFile(string fileName, string foldername)
        {
            string currentPath = Environment.CurrentDirectory;
            string load_path = System.IO.Path.Combine(currentPath, foldername, fileName);
            System.Diagnostics.Process.Start(load_path);
        }
    }
}
