using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
    public Texture2D crosshair;
    public Rect pos;
    static bool OriginalOn = true;
    public bool CursorLock= false;
	void Start () {
        if (gameObject.name != "MainCamera")
        {
            pos = new Rect((Screen.width - crosshair.width) / 2, (Screen.height - crosshair.height) / 2, crosshair.width, crosshair.height);
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CursorLock == true)
            {
                CursorLock = false;
            }
            else if(CursorLock == false)
            {
                CursorLock = true;
            }
        }
    }

	void OnGUI () {
        if (CursorLock == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (CursorLock == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (OriginalOn == true)
        {
            GUI.DrawTexture(pos, crosshair);
        }
	}
}
