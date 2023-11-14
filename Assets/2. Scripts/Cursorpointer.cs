using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursorpointer : MonoBehaviour
{
    public Texture2D cursorImg;

    void Start()
    {
        Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.ForceSoftware);
    }


}
