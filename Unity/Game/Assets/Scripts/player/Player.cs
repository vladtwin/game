using UnityEngine;
using System;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {


    private Rigidbody phisiks;
    private NavMeshAgent agent;
    public Vector3 target;  
    public GameObject clik;
    // Parametres
    public float speedMove = 10f;    // Скорость передвижения
    public float speedRotation = 360f;    // Скорость поворота

    // Маркеры персонажа
    public bool onPlace = true; 



    private CapsuleCollider capsule;
    private Rigidbody rigid;
    // parametrs
    [SyncVar]
    public int HP = 100;
    public int weight = 100;
    public int speed = 100;
    //score
    public int score;
    public int kills;
    public int damage;

    public int perkPoint;
    public int money;

    // markers
    public bool isGround = true;
    public bool isTeleported = false;
    public bool isSelected = false;

    public Texture2D selectPic;
    public CameraMove camera;

   
    public SkilllUsage skills;
    public int selectedSkillId;
    private int playerId;
    [SyncVar]
    public float SkillUseProcentage;
   // public iSkillProperties s;

    private void Start ()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speedMove;
        agent.angularSpeed = speedRotation;
        target = transform.position;


        capsule = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();
        camera = GameObject.Find("CameraMove").GetComponent<CameraMove>();
        camera.targetPlayer = this;
        skills = new SkilllUsage();
        skills.Register(SkillManager.singleton.getSkill(1));
       
    }

    void Update()
    {
        if(isServer)
        {
            skills.Tick(Time.deltaTime);
            SkillUseProcentage = skills.castProcentage;
        }          
        if (isLocalPlayer)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray targetRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(targetRay, out hit, Mathf.Infinity))
                {
                    if (selectedSkillId != 0)
                    {
                        Vector3Serializer target = new Vector3Serializer();
                        target.Fill(hit.point);
                        CmdSkillUse(selectedSkillId, target);
                        selectedSkillId = 0;
                    }
                    else
                    {
                        target = hit.point;
                        GameObject tap = (Instantiate(clik, target, Quaternion.Euler(-90, 0, 0)) as GameObject);
                        Destroy(tap, 1f);
                        CmdMovePlayer(target);
                    }
                }
            }
        }
    }
    [Command]
    void CmdSkillUse(int id,Vector3Serializer v3)
    {
        skills.SkillUse(id, v3.V3,transform.position,playerId);   
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

        if (!onPlace)
        {
            agent.SetDestination(target);//move only in server
            skills.AbortCast();
        }
    }


}
