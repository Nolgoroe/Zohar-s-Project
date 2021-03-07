using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public enum DocumentType
{
    Equipment,
    HollowObject
}

[System.Serializable]
public class csvFileInfo
{
    public TextAsset csvFiles;
    public DocumentType typeOfCsvDoc;
}
public class CSVParser : MonoBehaviour
{
    public csvFileInfo[] csvFiles;

    StreamReader inputStream;

    string targetPath;

    void Start()
    {
        Debug.Log("Reading CSV");

        if (Application.platform == RuntimePlatform.Android)
        {
            targetPath = Application.persistentDataPath;

            foreach (csvFileInfo FI in csvFiles)
            {
                SaveToPersistentDataPath(FI);
            }
        }
        else
        {
            foreach (csvFileInfo FI in csvFiles)
            {
                readTextFile(FI);
            }
        }

    }

    public void SaveToPersistentDataPath(csvFileInfo FI)
    {
        File.WriteAllText(Application.persistentDataPath +"/" + FI.csvFiles.name + ".csv", FI.csvFiles.text);

        readTextFile(FI);
    }

    void readTextFile(csvFileInfo FI)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
        }
        else
        {
            inputStream = new StreamReader("Assets/Resources/CSV/DATA.csv");
        }

        List<string> lineList = new List<string>();

        while (!inputStream.EndOfStream)
        {
            string inputLine = inputStream.ReadLine();

            lineList.Add(inputLine);
        }

        inputStream.Close();

        parseList(lineList, FI.typeOfCsvDoc);
    }

    void parseList(List<string> stringList, DocumentType DT)
    {
        List<string[]> parsedList = new List<string[]>();
        for (int i = 0; i < stringList.Count; i++)
        {
            string[] temp = stringList[i].Split(',');
            for (int j = 0; j < temp.Length; j++)
            {
                temp[j] = temp[j].Trim();  //removed the blank spaces
            }
            parsedList.Add(temp);
        }
    }
}
