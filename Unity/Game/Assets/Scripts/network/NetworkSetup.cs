using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkSetup : NetworkBehaviour {

    public List<GameObject> destroy = new List<GameObject>();
	void Start () {
        if (!base.isLocalPlayer)
            foreach (GameObject go in destroy)
                Destroy(go);
    }
	
	
}
