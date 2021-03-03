using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    //private Touch touch;

    private Vector2 touchPos;

    private float rotationY, rotationX;

    public Transform toRotate;
    public Transform toZoom;
    public float rotationSpeedModifier = 0.5f;
    public float zoomSpeed = 3;

    public static bool isInGame = false;
    public static bool isUsintMovement = false;
    void Start()
    {
        isInGame = false;
        isUsintMovement = false;
    }

    void Update()
    {
        if (isInGame)
        {
            //if (Input.touchCount == 1)
            //{
            //    touch = Input.GetTouch(0);

            //    if (touch.phase == TouchPhase.Moved)
            //    {
            //        float deltaX = touch.deltaPosition.x;
            //        float deltaY = touch.deltaPosition.y;
            //        rotationX -= deltaX * Time.deltaTime * rotationSpeedModifier;
            //        rotationY += deltaY * Time.deltaTime * rotationSpeedModifier;
            //        toRotate.transform.eulerAngles = new Vector3(-rotationY, -rotationX, 0f);

            //    }
            //}

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
                    toRotate.transform.eulerAngles = new Vector3(-rotationY, -rotationX, 0f);
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
                isUsintMovement = false;
                PainterManager.Instacne.hitScreenData.enabled = true;
            }
        }
    }

    private void zoom(float v)
    {
        toZoom.transform.position += toZoom.transform.forward * v * zoomSpeed;
    }
}
