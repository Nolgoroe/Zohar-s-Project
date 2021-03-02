using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;

public class PainterManager : MonoBehaviour
{
    public P3dPaintDecal painter;

    public Texture shapeTex;
    public Texture textureTex;
    void Start()
    {
        
    }

    void Update()
    {
        painter.Shape = shapeTex;
        painter.Texture = textureTex;
    }

    public void BrushSizeChange(bool UpSize)
    {
        if (UpSize)
        {
            painter.Radius += 0.5f;
        }
        else
        {
            painter.Radius -= 0.5f;
        }

        painter.Radius = Mathf.Clamp(painter.Radius, 0.1f, 5f);
    }
}
