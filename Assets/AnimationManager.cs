using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;


[Serializable]
public class AnimatedObject
{
    public GameObject theObject;
    public Vector3 originalPos;
    public Vector3 originalScale;
    public Vector3 newPos;
    public Vector3 newScale;
    public float timeToAnimate = 1;
    public Image imageToChange;
    public Sprite originalSprite;
    public Sprite targetSprite;
}

[Serializable]
public class FadeAnimatedObjects
{
    public Image theObject;
    public float origingalAlpha = 0;
    public float targetAlpha = 1;
    public float timeToAnimate = 1;
    public bool isStartNoAlpha;
}
public class AnimationManager : MonoBehaviour
{
    public AnimatedObject[] objectsToAnimate;
    public FadeAnimatedObjects[] objectsToFade;

    void Start()
    {
        for (int i = 0; i < objectsToAnimate.Length; i++)
        {
            objectsToAnimate[i].originalPos = objectsToAnimate[i].theObject.transform.localPosition;
            objectsToAnimate[i].originalScale = objectsToAnimate[i].theObject.transform.localScale;

            if (objectsToAnimate[i].targetSprite)
            {
                objectsToAnimate[i].imageToChange = objectsToAnimate[i].theObject.GetComponent<Image>();
                objectsToAnimate[i].originalSprite = objectsToAnimate[i].theObject.GetComponent<Image>().sprite;
            }
        }

        for (int i = 0; i < objectsToFade.Length; i++)
        {
            if (objectsToFade[i].isStartNoAlpha)
            {
                objectsToFade[i].theObject.color = new Color(objectsToFade[i].theObject.color.r, objectsToFade[i].theObject.color.g, objectsToFade[i].theObject.color.b, 0);
            }
            else
            {
                objectsToFade[i].theObject.color = new Color(objectsToFade[i].theObject.color.r, objectsToFade[i].theObject.color.g, objectsToFade[i].theObject.color.b, 1);
            }

            objectsToFade[i].origingalAlpha = objectsToFade[i].theObject.color.a;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AnimateNow();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Rewind();
        }
    }

    public void AnimateNow()
    {
        for (int i = 0; i < objectsToAnimate.Length; i++)
        {
            objectsToAnimate[i].theObject.transform.DOLocalMove(objectsToAnimate[i].newPos, objectsToAnimate[i].timeToAnimate);
            objectsToAnimate[i].theObject.transform.DOScale(objectsToAnimate[i].newScale, objectsToAnimate[i].timeToAnimate);

            if (objectsToAnimate[i].targetSprite)
            {
                objectsToAnimate[i].imageToChange.DOCrossfadeImage(objectsToAnimate[i].targetSprite, objectsToAnimate[i].timeToAnimate);
            }
        }

        for (int i = 0; i < objectsToFade.Length; i++)
        {
            objectsToFade[i].theObject.DOFade(objectsToFade[i].targetAlpha, objectsToFade[i].timeToAnimate);
        }

        UIManager.Instance.AfterAnimation();
    }

    public void Rewind()
    {
        for (int i = 0; i < objectsToAnimate.Length; i++)
        {
            objectsToAnimate[i].theObject.transform.DOLocalMove(objectsToAnimate[i].originalPos, objectsToAnimate[i].timeToAnimate);
            objectsToAnimate[i].theObject.transform.DOScale(objectsToAnimate[i].originalScale, objectsToAnimate[i].timeToAnimate);

            if (objectsToAnimate[i].targetSprite)
            {
                objectsToAnimate[i].imageToChange.DOCrossfadeImage(objectsToAnimate[i].originalSprite, objectsToAnimate[i].timeToAnimate);
            }
        }

        for (int i = 0; i < objectsToFade.Length; i++)
        {
            objectsToFade[i].theObject.DOFade(objectsToFade[i].origingalAlpha, objectsToFade[i].timeToAnimate);
        }

        UIManager.Instance.AfterRewind();
    }
}
