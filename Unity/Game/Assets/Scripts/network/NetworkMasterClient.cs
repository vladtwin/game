using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class NetworkMasterClient : NetworkManager
{
	public bool dedicatedServer;
	public string MasterServerIpAddress;
	public int MasterServerPort;
	public int updateRate;
	public string gameTypeName;
	public string gameName;
	public int gamePort=55005;

	[SerializeField]
	public int yoffset = 0;

	string HostGameType = "";
	string HostGameName = "";

	MasterMsgTypes.Room[] hosts = null;

	public NetworkClient client = null;   

	static NetworkMasterClient singleton;

    public NetworkManager maneger;
    public GameObject falsePrefab;
    public GameObject testPrefab;

    public void Start()
    {
        maneger = gameObject.AddComponent<NetworkManager>();
        maneger.playerPrefab = falsePrefab;
        maneger.networkPort = gamePort;
        maneger.spawnPrefabs.Add(testPrefab);

        SkillManager.singleton.register(maneger);
       
       // maneger.autoCreatePlayer = false;
    }


    void Awake()
	{
		if (singleton == null)
		{
			singleton = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	public void InitializeClient()
	{
		if (client != null)
		{
			Debug.LogError("Already connected");
			return;
		}

		client = new NetworkClient();
		client.Connect(MasterServerIpAddress, MasterServerPort);

		// system msgs
		client.RegisterHandler(MsgType.Connect, OnClientConnect);
		client.RegisterHandler(MsgType.Disconnect, OnClientDisconnect);
		client.RegisterHandler(MsgType.Error, OnClientError);

		// application msgs
		client.RegisterHandler(MasterMsgTypes.RegisteredHostId, OnRegisteredHost);
		client.RegisterHandler(MasterMsgTypes.UnregisteredHostId, OnUnregisteredHost);
		client.RegisterHandler(MasterMsgTypes.ListOfHostsId, OnListOfHosts);


		DontDestroyOnLoad(gameObject);
        

    }

	public void ResetClient()
	{
		if (client == null)
			return;

		client.Disconnect();
		client = null;
		hosts = null;
	}

	public bool isConnected
	{
		get
		{
			if (client == null) 
				return false;
			else 
				return client.isConnected;
		}
	}

	// --------------- System Handlers -----------------

	void OnClientConnect(NetworkMessage netMsg)
	{
		Debug.Log("Client Connected to Master");
	}

	void OnClientDisconnect(NetworkMessage netMsg)
	{
		Debug.Log("Client Disconnected from Master");
		ResetClient();
		OnFailedToConnectToMasterServer();
	}

	void OnClientError(NetworkMessage netMsg)
	{
		Debug.Log("ClientError from Master");
		OnFailedToConnectToMasterServer();
	}

	// --------------- Application Handlers -----------------

	void OnRegisteredHost(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<MasterMsgTypes.RegisteredHostMessage>();
		OnServerEvent((MasterMsgTypes.NetworkMasterServerEvent)msg.resultCode);
	}

	void OnUnregisteredHost(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<MasterMsgTypes.RegisteredHostMessage>();
		OnServerEvent((MasterMsgTypes.NetworkMasterServerEvent)msg.resultCode);
	}

	void OnListOfHosts(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<MasterMsgTypes.ListOfHostsMessage>();
		hosts = msg.hosts;
		OnServerEvent(MasterMsgTypes.NetworkMasterServerEvent.HostListReceived);
	}

	public void ClearHostList()
	{
		if (!isConnected)
		{
			Debug.LogError("ClearHostList not connected");
			return;
		}
		hosts = null;

	}

	public MasterMsgTypes.Room[] PollHostList()
	{
		if (!isConnected)
		{
			Debug.LogError("PollHostList not connected");
			return null;
		}
		return hosts;
	}

	public void RegisterHost(string gameTypeName, string gameName, string comment, bool passwordProtected, int playerLimit, int port)
	{
		if (!isConnected)
		{
			Debug.LogError("RegisterHost not connected");
			return;
		}

		var msg = new MasterMsgTypes.RegisterHostMessage();
		msg.gameTypeName = gameTypeName;
		msg.gameName = gameName;
		msg.comment = comment;
		msg.passwordProtected = passwordProtected;
		msg.playerLimit = playerLimit;
		msg.hostPort = port;
		client.Send(MasterMsgTypes.RegisterHostId, msg);

		HostGameType = gameTypeName;
		HostGameName = gameName;
	}

	public void RequestHostList(string gameTypeName)
	{
		if (!isConnected)
		{
			Debug.LogError("RequestHostList not connected");
			return;
		}

		var msg = new MasterMsgTypes.RequestHostListMessage();
		msg.gameTypeName = gameTypeName;
        
		client.Send(MasterMsgTypes.RequestListOfHostsId, msg);
	}

	public void UnregisterHost()
	{
		if (!isConnected)
		{
			Debug.LogError("UnregisterHost not connected");
			return;
		}

		var msg = new MasterMsgTypes.UnregisterHostMessage();
		msg.gameTypeName = HostGameType;
		msg.gameName = HostGameName;
		client.Send(MasterMsgTypes.UnregisterHostId, msg);
		HostGameType = "";
		HostGameName = "";

		Debug.Log("send UnregisterHost");
	}

	public virtual void OnFailedToConnectToMasterServer()
	{
		Debug.Log("OnFailedToConnectToMasterServer");
	}

	public virtual void OnServerEvent(MasterMsgTypes.NetworkMasterServerEvent evt)
	{
		Debug.Log("OnServerEvent " + evt);

		if (evt == MasterMsgTypes.NetworkMasterServerEvent.HostListReceived)
		{
			foreach (var h in hosts)
			{
				Debug.Log("Host:" + h.name + "addr:" + h.hostIp + ":" + h.hostPort);
			}
		}

		if (evt == MasterMsgTypes.NetworkMasterServerEvent.RegistrationSucceeded)
		{
			if (NetworkManager.singleton != null)
			{
				NetworkManager.singleton.StartHost();
			}
		}

		if (evt == MasterMsgTypes.NetworkMasterServerEvent.UnregistrationSucceeded)
		{
			if (NetworkManager.singleton != null)
			{
				NetworkManager.singleton.StopHost();
			}
		}
	}
    void OnConnectedToServer()
    {
        Debug.Log("Connected to server"); // сообщение выводится при успешном подключении к серверу
                                          //    Network.Instantiate(testPrefab, new Vector3(0, 0, 0), new Quaternion(), 0);

       GameObject gp = Instantiate(testPrefab);
       NetworkServer.Spawn(gp);

    }
  
    void OnGUI()
	{

        if (client != null && client.isConnected)
		{
			if (GUI.Button(new Rect(0, 20, 200, 20), "Отключится от сервера"))
			{
				ResetClient();
				if (NetworkManager.singleton != null)
				{
					NetworkManager.singleton.StopServer();
					NetworkManager.singleton.StopClient();
				}
				HostGameType = "";
				HostGameName = "";
			}
		}
		else
		{
			if (GUI.Button(new Rect(0, 20, 200, 20), "Подключится к серверу"))
			{
				InitializeClient();
			}
			return;
		}


		if (HostGameType == "")
		{
			GUI.Label(new Rect(220, 0 , 80, 20), "Режим лобби:");
			gameTypeName = GUI.TextField(new Rect(300, 0 , 200, 20), gameTypeName);

			GUI.Label(new Rect(220, 20 , 80, 20), "Имя лобби:");
			gameName = GUI.TextField(new Rect(300, 20 , 200, 20), gameName);

			if (GUI.Button(new Rect(200, 60 , 200, 20), "Создать лобби"))
			{
                int port = gamePort;
				if (NetworkManager.singleton != null)
				{
					port = NetworkManager.singleton.networkPort;
				}
                if (NetworkManager.singleton == null)
                {
                
                }
                RegisterHost(gameTypeName, gameName, "none", false, 8, port);
                gameObject.AddComponent<NetworkManagerHUD>().showGUI=false;
                
            }

			if (GUI.Button(new Rect(0, 40 , 200, 20), "Список комнот"))
			{
				RequestHostList(gameTypeName);
			}
		}
		else
		{
			if (GUI.Button(new Rect(200, 60, 200, 20), "Остановить лобби"))
			{
				UnregisterHost();
			}
		}
        if (NetworkManager.singleton)
        {    }

        if (hosts != null && HostGameType == "")
		{
			int y = 140;
			foreach (var h in hosts)
			{
				if (GUI.Button(new Rect(120, y + yoffset, 240, 20), "Host:" + h.name + "addr:" + h.hostIp + ":" + h.hostPort))
				{
                    if(NetworkManager.singleton==null)
                    {
                        gameObject.AddComponent<NetworkManager>();
                    }
                    if (NetworkManager.singleton != null)
					{
                        NetworkManager.singleton.networkAddress =h.hostIp;
                        NetworkManager.singleton.networkPort = h.hostPort;
                        NetworkManager.singleton.StartClient();                        
                    }
				}
				y += 22;
			}
		}
	}
}
