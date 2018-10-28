using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class FileIO{

#if UNITY_IPHONE
    string fileNameBase = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
    static string fileName = fileNameBase.Substring(0, fileNameBase.LastIndexOf('/')) + "/Documents/" + FILE_NAME;
#elif UNITY_ANDROID
    static string fileName = Application.persistentDataPath + "/";
#else
    static string fileName = Application.dataPath + "/" ;
#endif

    static FileIO io;
    StreamWriter writer;
    string writingPath;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static public List<string> ReadTextFile(string fileNamePath)
    {
        if (!File.Exists( fileName + fileNamePath))
        {
            Debug.Log("FILE DOES NOT EXSIST");
            CreateDirectory("Data");
            //File.Create(fileName + fileNamePath);
            return null;
        }

        List<string> text = new List<string>();      
        StreamReader reader = new StreamReader(fileName + fileNamePath);
        string line = reader.ReadLine();

        while(line != null)
        {
            text.Add(line);
            line = reader.ReadLine();
        }

        reader.Close();

        return text;
    }
    static public bool HasDirectory(string directoryName)
    {
        return Directory.Exists(fileName + directoryName);
    }
    static public bool HasSpecificDirectory(string directoryName)
    {
        return Directory.Exists(directoryName);
    }
    static public void CreateDirectory(string directoryName)
    {
        if (!HasDirectory(directoryName))
        {
            Debug.Log("DIRECTORY DOESN'T EXSIST: " + fileName + directoryName);
            Directory.CreateDirectory(fileName + directoryName);
        }
    }
    static public void CreateSpecificDirectory(string directoryName)
    {
        if (!HasSpecificDirectory(directoryName))
        {
            Debug.Log("DIRECTORY DOESN'T EXSIST: " + directoryName);
            Directory.CreateDirectory(directoryName);
        }
    }
    static public void InitWriteToFile(string filePath, bool overWriteData)
    {
        if (io == null)
        {
            io = new FileIO();
        }

        io.writingPath = fileName + filePath;
        io.writer = new StreamWriter(io.writingPath, !overWriteData);
    }
    static public void WriteLineToFile(string line)
    {
        io.writer.WriteLine(line);
    }

    static public void EndWriteToFile()
    {
        io.writer.Close();
        io.writer = null;
    }
    static public void DeleteFile(string path)
    {
        if (File.Exists(fileName + path))
        {
            File.Delete(fileName + path);
        }
        else
        {
            Debug.Log("FILE DOES NOT EXIST!");
        }
    }
    static public void CopyFolderContentTo(string startPath, string endPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(startPath);
        FileInfo[] filesInFolder = directoryInfo.GetFiles();
        int fileCount = filesInFolder.Length;

        for (int i = 0; i < fileCount; i++)
        {
            filesInFolder[i].CopyTo(endPath + "/" + filesInFolder[i].Name, true);
        }
    }
}
