using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button letsStartButton, startDesignButton;

    public static UIManager Instance;


    public GameObject CanvasScreens;

    private void Start()
    {
        Instance = this;

        startDesignButton.interactable = false;
        startDesignButton.GetComponent<Image>().raycastTarget = false;

        letsStartButton.interactable = true;
        letsStartButton.GetComponent<Image>().raycastTarget = true;
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
        CanvasScreens.SetActive(false);
        TouchManager.isInGame = true;
        Timer.Instance.timerIsRunning = true;

        ColorPickerSimple.Instacne.colorPickedFrontImage.color = PainterManager.Instacne.painter.Color;
    }
}
