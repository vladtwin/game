using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    public float ZoomSpeed = 100;
    public float ZoomMin = 20;
    public float ZoomMax = 60;
    private Transform targ;

    void Awake()
    {
     //   targ = gameObject.GetComponentInParent<CameraMove>().targetPlayer.transform;
    }
    void Update ()
    {
       /* float sc1 = Input.GetAxis("Mouse ScrollWheel");
        if (sc1 > 0 && transform.position.y > targ.position.y+ZoomMin)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * ZoomSpeed);
        }
        if (sc1 < 0 && transform.position.y < targ.position.y + ZoomMax)
        {
            transform.Translate(Vector3.back * Time.deltaTime * ZoomSpeed);
        }*/
    }
}
