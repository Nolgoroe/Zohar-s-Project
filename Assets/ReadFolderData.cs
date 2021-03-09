using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.Localization;
using UnityEngine.Networking;


public class ReadFolderData : MonoBehaviour
{
    public static ReadFolderData Instance;
    //public LocalizedTexture localTex;
    public string colorCode;

    public List<Texture> languageTextures;
    public List<string> languageVideoClipsURL;

    void Start()
    {
        Instance = this;

        languageTextures = new List<Texture>();
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] Images = dir.GetFiles("*.png");

        foreach (FileInfo f in Images)
        {
            if (f.ToString().Contains("Lang"))
            {

                StartCoroutine(LoadImages(f));
            }
        }

        FileInfo[] moveis = dir.GetFiles("*.mp4");

        foreach (FileInfo f in moveis)
        {
            if (f.ToString().Contains("Lang"))
            {
                StartCoroutine(LoadMovies(f));
            }
        }

        FileInfo[] colorCode = dir.GetFiles("*.txt");

        foreach (FileInfo f in colorCode)
        {
            if (f.ToString().Contains("Color"))
            {
                print(f.Name);
                StartCoroutine(LoadColorCode(f));
            }
        }
    }


    IEnumerator LoadMovies(FileInfo GameData)
    {
        //1 ignore meata files
        if (GameData.Name.Contains("meta"))
        {
            yield break;
        }
        else
        {
            string wwwImageFilePath = "file://" + GameData.FullName.ToString();
        WWW www = new WWW(wwwImageFilePath);
        //UnityWebRequest www = new UnityWebRequest(wwwImageFilePath);
        yield return www;

            languageVideoClipsURL.Add(www.url);
        }
    }
    IEnumerator LoadColorCode(FileInfo GameData)
    {
        //1 ignore meata files
        if (GameData.Name.Contains("meta"))
        {
            yield break;
        }
        else
        {
            string wwwTextFilePath = "file://" + GameData.FullName.ToString();
            WWW www = new WWW(wwwTextFilePath);
            yield return www;

            StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/ColorData.txt");

            colorCode = reader.ReadToEnd();

            Color color = new Color();
            ColorUtility.TryParseHtmlString("#"+colorCode, out color);
            UIManager.Instance.coloredBar.color = color;

            reader.Close();
        }

    }
    IEnumerator LoadImages(FileInfo GameData)
    {
        //1 ignore meata files
        if (GameData.Name.Contains("meta"))
        {
            yield break;
        }
        else
        {
            string wwwVideoFilePath = "file://" + GameData.FullName.ToString();
            WWW www = new WWW(wwwVideoFilePath);
            yield return www;

            languageTextures.Add(www.texture);
        }

    }
}
