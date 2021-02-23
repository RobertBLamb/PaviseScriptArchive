using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {

    public Texture2D cursor;
    /*
	private Vector2 hotSpot = Vector2.zero;

	void Start() {
		Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
	}
    */

    Vector2 mouse;
    int w = 50;
    int h = 50;
     
    void Start()
    {
        Cursor.visible = false;
        h = Screen.height/12;
        w = h;
    }

    void Update()
    {
        mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(mouse.x - (w / 2), mouse.y - (h / 2), w, h), cursor);
    }
}