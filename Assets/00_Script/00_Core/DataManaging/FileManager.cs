using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public static class FileManager
{
    public static List<string> File_Load(string _filePath, char _seperator = ',')
    {
        string currentPath = Environment.CurrentDirectory;
        //Debug.Log(currentPath.ToString());
        string filePath = Path.Combine(currentPath, _filePath);
        StreamReader sr = null;
        try
        {
            sr = new StreamReader(new FileStream(filePath, FileMode.Open));
        }
        catch (FileLoadException e)
        {
            Debug.LogError(e.ToString());
        }

        List<string> loaded_data = new List<string>();

        while (sr.EndOfStream == false)
        {
            string s = sr.ReadLine();
            string[] line = s.Split(_seperator);

            foreach (var item in line)
            {
                loaded_data.Add(item);
            }
        }

        if (sr != null)
            sr.Close();

        return loaded_data;
    }
}

