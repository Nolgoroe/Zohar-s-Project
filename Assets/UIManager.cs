using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class UIManager : MonoBehaviour
{
    public Button letsStartButton, startDesignButton;

    public static UIManager Instance;


    public GameObject firstScreenUI, lastScreenUI, sidePanel;

    public Transform shoeGO;

    public RawImage infoTextImage;
    public Image coloredBar;

    public VideoPlayer playerOfVideos;

    public int clickedIndexByInfo;

    private void Start()
    {
        Instance = this;

        startDesignButton.interactable = false;
        startDesignButton.GetComponent<Image>().raycastTarget = false;

        letsStartButton.interactable = true;
        letsStartButton.GetComponent<Image>().raycastTarget = true;

        lastScreenUI.SetActive(false);
        firstScreenUI.SetActive(true);
        sidePanel.SetActive(true);
    }


    public void AfterAnimation()
    {
        startDesignButton.interactable = true;
        startDesignButton.GetComponent<Image>().raycastTarget = true;

        letsStartButton.interactable = false;
        letsStartButton.GetComponent<Image>().raycastTarget = false;
    }

    public void AfterRewind()
    {
        startDesignButton.interactable = false;
        startDesignButton.GetComponent<Image>().raycastTarget = false;

        letsStartButton.interactable = true;
        letsStartButton.GetComponent<Image>().raycastTarget = true;
    }

    public void GoToGame()
    {
        firstScreenUI.SetActive(false);
        TouchManager.isInGame = true;
        Timer.Instance.timerIsRunning = true;

        ColorPickerSimple.Instacne.colorPickedFrontImage.color = PainterManager.Instacne.painter.Color;

        PainterManager.Instacne.hitScreenData.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeLocalizationImages(int index)
    {
        clickedIndexByInfo = index;
        if (ReadFolderData.Instance.languageTextures.Count > index)
        {
            infoTextImage.texture = ReadFolderData.Instance.languageTextures[index];
        }
        else
        {
            Debug.Log("No image Found");
        }
        //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

    }
    public IEnumerator GoLastScreen()
    {
        yield return new WaitForSeconds(1.1f);
        shoeGO.transform.rotation = Quaternion.identity;
        shoeGO.transform.position = Vector3.zero;
        TouchManager.isInGame = false;

        firstScreenUI.SetActive(false);
        lastScreenUI.SetActive(true);

        yield return new WaitForSeconds(2);
        //for (int i = 0; i < 100000; i++)
        //{
        //    if (!System.IO.File.Exists(Application.streamingAssetsPath + "/Screenshot" + i + ".png"))
        //    {
        //        ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/Screenshot" + i + ".png");
        //        break;
        //    }
        //}

        string date = System.DateTime.Now.ToString();
        date = date.Replace("/", "-");
        date = date.Replace(" ", "_");
        date = date.Replace(":", "-");

        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/Screenshot" + date + ".png"))
        {
            ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/Screenshot" + date + ".png");
        }
    }
}
