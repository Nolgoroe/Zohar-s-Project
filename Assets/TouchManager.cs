using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaintIn3D;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;
    //private Touch touch;

    private Vector2 touchPos;

    private float /*rotationY,*/ rotationX;
    private Camera Camera;

    public Transform toRotate;
    public Transform toZoom;
    public float rotationSpeedModifier = 0.5f;
    public float zoomSpeed = 3;

    public static bool isInGame = false;
    public static bool isUsintMovement = false;

    public RawImage texture;

    RaycastHit2D[] HitsBuffer = new RaycastHit2D[1];
    public bool clickingTex = false;
    public float timer = 0;
    bool stopCheck = false;
    bool chosenTex = false;
    Vector3 screenPos;
    public LayerMask hitLayer3D;

    public ScrollRect textureScrollRect;
    Transform previouslyDetectedPiece = null;
    void Start()
    {
        Instance = this;
        Camera = Camera.main;
        isInGame = false;
        isUsintMovement = false;
        clickingTex = false;
        chosenTex = false;
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
                    HitsBuffer[0] = new RaycastHit2D();
                    timer = 0;

                    screenPos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));

                    int hitCount = Physics2D.RaycastNonAlloc(screenPos, Vector2.zero, HitsBuffer, Mathf.Infinity);

                    if(HitsBuffer[0].transform)
                    {
                        if (HitsBuffer[0].transform.CompareTag("TexturePrefab"))
                        {
                            clickingTex = true;
                        }
                    }
                }

                if(touch.phase == TouchPhase.Stationary)
                {
                    if (clickingTex && !stopCheck)
                    {
                        stopCheck = false;
                        timer += Time.deltaTime;

                        if (timer > 0.3f)
                        {
                            textureScrollRect.enabled = false;
                            chosenTex = true;
                            stopCheck = true;
                            TextureHolderScript THS = HitsBuffer[0].transform.GetComponent<TextureHolderScript>();

                            texture.gameObject.SetActive(true);
                            texture.texture = THS.heldTexture;

                            //PainterManager.Instacne.painter.Texture = THS.heldTexture;

                            Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));
                            mousePos.z = texture.transform.position.z;
                            texture.transform.position = mousePos;
                            //Debug.Log("IN TEX");
                        }
                    }
                }

                if (touch.phase == TouchPhase.Moved )
                {
                    clickingTex = false;
                    timer = 0;

                    if (chosenTex)
                    {
                        Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));
                        mousePos.z = texture.transform.position.z;
                        texture.transform.position = mousePos;

                        RaycastHit hit;
                        Ray raycastRay;
                        Ray ray = Camera.ScreenPointToRay(touch.position);

                        //Debug.DrawRay(Camera.transform.position, mousePos, Color.red);
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                        {
                            if (hit.transform.CompareTag("ShoePiece"))
                            {
                                Transform newDetected = hit.transform;

                                P3dPaintable paintableObject = hit.transform.GetComponent<P3dPaintable>();

                                if (previouslyDetectedPiece == null)
                                {
                                    Renderer pieceRenderer = newDetected.transform.GetComponent<Renderer>();
                                    pieceRenderer.materials[0].SetTexture("_BaseMap", texture.texture);
                                    previouslyDetectedPiece = newDetected;
                                    RefreshMap(paintableObject);
                                }

                                if (newDetected != previouslyDetectedPiece)
                                {
                                    Renderer previouslyDetectedPieceRenderer = previouslyDetectedPiece.transform.GetComponent<Renderer>();
                                    previouslyDetectedPieceRenderer.materials[0].SetTexture("_BaseMap", null);

                                    Renderer newDetectedPieceRenderer = newDetected.transform.GetComponent<Renderer>();
                                    newDetectedPieceRenderer.materials[0].SetTexture("_BaseMap", texture.texture);

                                    previouslyDetectedPiece = newDetected;
                                    RefreshMap(paintableObject);
                                }
                            }
                        }
                        else
                        {
                            if (previouslyDetectedPiece)
                            {
                                Renderer previouslyDetectedPieceRenderer = previouslyDetectedPiece.transform.GetComponent<Renderer>();
                                previouslyDetectedPieceRenderer.material.SetTexture("_BaseMap", null);
                                previouslyDetectedPiece = null;
                            }
                        }
                    }
                }

                if(touch.phase == TouchPhase.Ended)
                {
                    textureScrollRect.enabled = true;

                    previouslyDetectedPiece = null;
                    stopCheck = false;
                    clickingTex = false;
                    chosenTex = false;
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
                    //rotationY += deltaY * Time.deltaTime * rotationSpeedModifier;
                    toRotate.transform.eulerAngles = new Vector3(0 , -rotationX/*, rotationY*/);
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

    public void RefreshMap(P3dPaintable paintableObject)
    {
        paintableObject.Invoke("Activate", 0.1f);
        paintableObject.Activate();
    }
}
