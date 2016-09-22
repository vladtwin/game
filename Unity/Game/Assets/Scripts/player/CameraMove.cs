using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraMove : MonoBehaviour {
    public Player targetPlayer;
    private Vector3 camMove;
    private int camMoveRange = 20;
    private float camMoveSpeed = 30;
    private Vector3 PlayerCoordinate;
    public int camStaticSpeed = 20;
    public int maxTop = 0;
    public int maxBottom = 0;
    public int maxLeft = 0;
    public int maxRight = 0;
    private bgUI ui;
    public bool _isLocalPlayer;
    void Start ()
    {
        //if (!isLocalPlayer) Destroy(gameObject);
        //player = targetPlayer.GetComponent<Player>();
        if (targetPlayer != null)
        {
            PlayerCoordinate = new Vector3(Mathf.Round(targetPlayer.transform.position.x), Mathf.Round(targetPlayer.transform.position.y + 25), Mathf.Round(targetPlayer.transform.position.z - 10));
            transform.position = PlayerCoordinate;
        }
        camMove = new Vector3();
        ui = FindObjectOfType<bgUI>();
    }

    private bool fo;
    void OnApplicationFocus(bool focus)
    {
        fo = focus;
    }

    void Update () {
    //    _isLocalPlayer = base.isLocalPlayer;
     //   if (!base.isLocalPlayer) return;
       
        //if (fo)
        {
            if (Input.GetKeyDown(KeyCode.F1) && targetPlayer != null)
            {
                PlayerCoordinate.Set(Mathf.Round(targetPlayer.transform.position.x), Mathf.Round(targetPlayer.transform.position.y + 25), Mathf.Round(targetPlayer.transform.position.z - 10));
                transform.position = PlayerCoordinate;
                targetPlayer.isSelected = true;
               // ui.selectedPlayer = targetPlayer;
            }

            camMove.Set(0, 0, 0);
            if (Input.mousePosition.x >= Screen.width - camMoveRange && transform.position.x<=maxRight)
            {
                camMove += Vector3.right;
                camMoveSpeed = Mathf.Abs(Screen.width - Input.mousePosition.x - camMoveRange);
            }
            if (Input.mousePosition.x <= camMoveRange && transform.position.x >= maxLeft)
            {
                camMove += Vector3.left;
                camMoveSpeed = Mathf.Abs(Input.mousePosition.x - camMoveRange);
            }
            if (Input.mousePosition.y >= Screen.height - camMoveRange && transform.position.z <= maxTop)
            {
                camMove += Vector3.up;
                camMoveSpeed = Mathf.Abs(Screen.height - Input.mousePosition.y - camMoveRange);
            }
            if (Input.mousePosition.y <= camMoveRange && transform.position.z >= maxBottom)
            {
                camMove += Vector3.down;
                camMoveSpeed = Mathf.Abs(Input.mousePosition.y - camMoveRange);
            }
            transform.Translate(camMove * Time.deltaTime * (camMoveSpeed < camMoveRange ? camMoveSpeed+camStaticSpeed : camMoveRange + camStaticSpeed));
        }
    }
}
