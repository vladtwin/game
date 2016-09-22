using UnityEngine;
using System.Collections;
using UnityEngine.Networking;



public class Move : NetworkBehaviour
{
    private Rigidbody phisiks;
    private NavMeshAgent agent;
    public Vector3 target;
    private Animator anima;
    public GameObject clik;
    // Parametres
    public float speedMove = 10f;    // Скорость передвижения
    public float speedRotation = 360f;    // Скорость поворота

    // Маркеры персонажа
    public bool onPlace = true;
    public bool isGround;
    
    void Start () {
      
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speedMove;
        agent.angularSpeed = speedRotation;
        target = transform.position;

      //  agent.updatePosition = false;   
    }

    void Update ()
    {              
        if (base.isLocalPlayer && Input.GetMouseButtonDown(1))
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray targetRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(targetRay, out hit, Mathf.Infinity))
                {
                    target = hit.point;
                    GameObject tap = (Instantiate(clik, target, Quaternion.Euler(-90, 0, 0)) as GameObject);
                    Destroy(tap, 1f);
                    CmdMovePlayer(target);//send to server target position
                }
            }
        }
    }
    [Command]
    void CmdMovePlayer(Vector3 target)
    {    
        if (Vector3.Distance(transform.position, target) < 1.5f)
        {
            onPlace = true;
        }
        else
        {
            onPlace = false;
        }

        if (!onPlace )
        {
             agent.SetDestination(target);//move only in server
        }
    }


}
