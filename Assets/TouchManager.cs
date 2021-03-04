using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;
    //private Touch touch;

    private Vector2 touchPos;

    private float rotationY, rotationX;
    private Camera Camera;

    public Transform toRotate;
    public Transform toZoom;
    public float rotationSpeedModifier = 0.5f;
    public float zoomSpeed = 3;

    public static bool isInGame = false;
    public static bool isUsintMovement = false;
    public static bool isUsingTexture = false;

    public RawImage texture;

    RaycastHit2D[] HitsBuffer = new RaycastHit2D[1];

    void Start()
    {
        Instance = this;
        Camera = Camera.main;
        isInGame = false;
        isUsintMovement = false;
        isUsingTexture = false;
        texture.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isInGame)
        {
            if(Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 screenPos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));

                    int hitCount = Physics2D.RaycastNonAlloc(screenPos, Vector2.zero, HitsBuffer, Mathf.Infinity);

                    for (int i = 0; i < hitCount; i++)
                    {
                        if (HitsBuffer[i].transform.CompareTag("TexturePrefab"))
                        {
                            TextureHolderScript THS = HitsBuffer[i].transform.GetComponent<TextureHolderScript>();

                            texture.gameObject.SetActive(true);
                            texture.texture = THS.heldTexture;

                            PainterManager.Instacne.painter.Texture = THS.heldTexture;
                            isUsingTexture = true;
                            Debug.Log("IN TEX");
                        }
                    }
                }

                if (touch.phase == TouchPhase.Moved && isUsingTexture)
                {
                    Vector3 screenPos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));
                    screenPos.z = 0;

                    texture.transform.position = screenPos;
                    Debug.Log(texture.transform.position);
                }

                if(touch.phase == TouchPhase.Ended)
                {
                    isUsingTexture = false;
                    texture.gameObject.SetActive(false);
                }
            }

            if (Input.touchCount >= 2)
            {
                isUsintMovement = true;
                PainterManager.Instacne.hitScreenData.enabled = false;
                Touch touchOne = Input.GetTouch(0);
                Touch touchTwo = Input.GetTouch(1);

                if (touchOne.phase == TouchPhase.Moved)
                {
                    float deltaX = touchOne.deltaPosition.x;
                    float deltaY = touchOne.deltaPosition.y;
                    rotationX -= deltaX * Time.deltaTime * rotationSpeedModifier;
                    rotationY += deltaY * Time.deltaTime * rotationSpeedModifier;
                    toRotate.transform.eulerAngles = new Vector3(0 , -rotationX, rotationY);
                }

                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;

                float prevMagnitude = (touchOnePrevPos - touchTwoPrevPos).magnitude;
                float currentMagnitude = (touchOne.position - touchTwo.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.01f);
            }
            else
            {
                if (!ColorPickerSimple.Instacne.Selected)
                {
                    isUsintMovement = false;
                    PainterManager.Instacne.hitScreenData.enabled = true;
                }
            }
        }
    }

    private void zoom(float v)
    {
        toZoom.transform.position += toZoom.transform.forward * v * zoomSpeed;
    }
}
