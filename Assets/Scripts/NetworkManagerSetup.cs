using UnityEngine;
using System.Collections;

public class NetworkManagerSetup : MonoBehaviour 
{
	public bool networkMode = true;
	int maxPlayers = 2;
	int portNumber = 11111;
	string serverName = "Standard Server Name";
	string serverDescription = "Standard Description Name";
	HostData[] hosts;

	public GameObject playerPrefab;
	public GameObject spawnPoint;
	private string gameType = "Cubeman";
	
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	void OnGUI () 
	{
		if(networkMode) {
			if(GUI.Button(new Rect(10,70,70,50),"Start Server"))
			{
				startServer();
			}
	
			if (GUI.Button(new Rect(10,130,70,30),"Join Server"))
			{
				joinServer();
			}
		}
	}
	// Update is called once per frame
	void Update() 
	{
		if (MasterServer.PollHostList().Length > 0)
		{
			hosts = MasterServer.PollHostList();
			for (int i = 0; i < hosts.Length; i++)
			{
				Debug.Log("Attempting to connect to " + hosts[i].gameName);
				Network.Connect(hosts[i]);
			}
		}
	}
	
	
	void startServer()
	{
		Network.InitializeServer(maxPlayers, portNumber,!Network.HavePublicAddress());
		MasterServer.RegisterHost(gameType, serverName, serverDescription); 
	}
	
	void joinServer()
	{
		MasterServer.RequestHostList(gameType);
	}
	
	void spawnPlayer()
	{
		GameObject go = (GameObject)Network.Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
		NetworkView nv = go.GetComponent<NetworkView>();
		if(nv.isMine) {
			SmoothFollow sf = Camera.main.GetComponent<SmoothFollow>();
			sf.target = go.transform;
			SmoothLookAt sla = Camera.main.GetComponent<SmoothLookAt>();
			sla.target = go.transform;
		}
	}
	
	void destroyPlayer(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	
	//Static stuff below. DO NOT TOUCH, PLEASE?//
	void OnServerInitialized()
	{
		spawnPlayer();
	}
	
	void OnConnectedToServer()
	{
		spawnPlayer();
	}
	
	void OnPlayerConnected()
	{
		spawnPlayer ();
	}
	
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		destroyPlayer(player);
	}
}
