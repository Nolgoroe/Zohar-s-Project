using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Localization;

public class ReadFolderData : MonoBehaviour
{
    public LocalizedTexture localTex;
    public string myPath;

    public string colorCode;
    void Start()
    {
        Debug.Log(Application.dataPath + myPath);
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + myPath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            print(f.Name);
        }


    }
}
