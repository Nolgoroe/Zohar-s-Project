using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class UIManager : MonoBehaviour
{
    public Button letsStartButton, startDesignButton;

    public static UIManager Instance;


    public GameObject firstScreenUI, lastScreenUI, sidePanel;

    public Transform shoeGO;

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

    public void ChangeLocalization(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    public IEnumerator GoLastScreen()
    {
        yield return new WaitForSeconds(1.1f);
        shoeGO.transform.rotation = Quaternion.identity;
        shoeGO.transform.position = Vector3.zero;
        TouchManager.isInGame = false;

        firstScreenUI.SetActive(false);
        lastScreenUI.SetActive(true);
    }
}
