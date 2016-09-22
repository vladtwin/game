using UnityEngine;
using System.Collections.Generic;

public class bgUI : MonoBehaviour {
    public GUISkin mainUI;
    public int numDepth = 0;
    public string nameWindow;
    public Texture2D selectPic;

    public Player selectedPlayer;

    void Start () {

	}
	
	void Update () {
        Debug.Log(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Ray targetRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(targetRay, out hit, Mathf.Infinity))
            {
                if (hit.transform.GetComponent<Player>() != null)
                {
                    selectedPlayer = hit.transform.GetComponent<Player>();
                    selectedPlayer.isSelected = true;
                }
                else
                {
                    selectedPlayer.isSelected = false;
                    selectedPlayer = null;
                }
            }
        }

    }

    void OnGUI()
    {
        GUI.depth = numDepth;
        GUI.skin = mainUI;

        //Left box
        GUI.Box(new Rect(0, Screen.height * 0.8f, Screen.height * 0.2f, Screen.height * 0.2f), "", GUI.skin.GetStyle("Border"));
        //Right box
        GUI.DrawTexture(new Rect(Screen.width - Screen.height * 0.19f, Screen.height * 0.81f, Screen.height * 0.18f, Screen.height * 0.18f), selectPic);
        GUI.Box(new Rect(Screen.width - Screen.height *0.2f, Screen.height * 0.8f, Screen.height * 0.2f, Screen.height * 0.2f), "", GUI.skin.GetStyle("Border"));
        //Center  box
        GUI.Box(new Rect(Screen.height * 0.2f, Screen.height * 0.84f, Screen.width - Screen.height * 0.4f, Screen.height * 0.16f), "", GUI.skin.GetStyle("CentralBox"));
        //GUI.Box(new Rect(Screen.height / 5, Screen.height / 5 - Screen.height / 6, Screen.width - Screen.height / 5 * 2, Screen.height / 6), "", GUI.skin.GetStyle("LongBorder"));
        //Lines
        //GUI.Box(new Rect(Screen.width * 0.3f, Screen.height / 5 - Screen.height / 6, 4, Screen.height / 6), "", GUI.skin.GetStyle("Line"));
        //GUI.Box(new Rect(Screen.width * 0.85f, Screen.height / 5 - Screen.height / 6, 4, Screen.height / 6), "", GUI.skin.GetStyle("Line"));

        //In central box

        if (selectedPlayer != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - Screen.height * 0.19f, Screen.height * 0.81f, Screen.height * 0.18f, Screen.height * 0.18f), selectedPlayer.selectPic);

            //Stats
            GUI.Label(new Rect(Screen.width * 0.22f, Screen.height * 0.9f, Screen.width * 0.21f, Screen.height * 0.03f), "HP - " + selectedPlayer.HP);
            GUI.Label(new Rect(Screen.width * 0.22f, Screen.height * 0.935f, Screen.width * 0.21f, Screen.height * 0.03f), "Weight - " + selectedPlayer.weight);
            GUI.Label(new Rect(Screen.width * 0.22f, Screen.height * 0.965f, Screen.width * 0.21f, Screen.height * 0.03f), "Speed - " + selectedPlayer.speed);

            // Buttons
            if (GUI.Button(new Rect(Screen.width * 0.36f, Screen.height * 0.845f, Screen.height * 0.07f, Screen.height * 0.07f), "Q"))
                ;
            GUI.Button(new Rect(Screen.width * 0.405f, Screen.height * 0.845f, Screen.height * 0.07f, Screen.height * 0.07f), "W");
            GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.845f, Screen.height * 0.07f, Screen.height * 0.07f), "E");
            GUI.Button(new Rect(Screen.width * 0.495f, Screen.height * 0.845f, Screen.height * 0.07f, Screen.height * 0.07f), "R");

            GUI.Button(new Rect(Screen.width * 0.37f, Screen.height * 0.92f, Screen.height * 0.07f, Screen.height * 0.07f), "A");
            GUI.Button(new Rect(Screen.width * 0.415f, Screen.height * 0.92f, Screen.height * 0.07f, Screen.height * 0.07f), "S");
            GUI.Button(new Rect(Screen.width * 0.46f, Screen.height * 0.92f, Screen.height * 0.07f, Screen.height * 0.07f), "D");
            GUI.Button(new Rect(Screen.width * 0.505f, Screen.height * 0.92f, Screen.height * 0.07f, Screen.height * 0.07f), "F");
        }


    }
}
