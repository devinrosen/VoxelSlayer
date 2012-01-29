using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiManager : MonoBehaviour {
	private static MultiManager instance = null;
	public static MultiManager Instance {
		get {
			if(instance == null) {
				instance = FindObjectOfType(typeof(MultiManager)) as MultiManager;
			}
			return instance;
		}
	}
	
	public GameObject fighterPrefab;
	public GameObject turretPrefab;
	
	List<Player> players;
	List<Camera> cameras;
	public int[] connected;
	Camera guiCamera;
	public float guiMargin = 0.05f;
	// Use this for initialization
	void Start () {
		players = new List<Player>();
		cameras = new List<Camera>();
		connected = new int[]{-1,-1,-1,-1};	
		guiCamera = GameObject.Find("GUICamera").camera;
	}
	
	// Update is called once per frame
	void Update () {
		for(int j = 1; j <= 2; j++) {
			for(int b = 0; b <= 10; b++) {
				if(Input.GetButtonDown("joystick "+j+" button "+b+"")) {
					JoystickButton(j,b);
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Space)) 
		{
			JoystickButton(0,9);
		}
		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			JoystickButton(0,8);
		}
	}
	void OnGUI() {
		if(players.Count == 1) {
			GUI.Label(new Rect(guiCamera.pixelWidth*guiMargin,guiCamera.pixelHeight*guiMargin,100,30),players[0].Health);
		}
		else if(players.Count == 2) {
			GUI.Label(new Rect(guiCamera.pixelWidth*guiMargin,guiCamera.pixelHeight*guiMargin,100,30),players[0].Health);
			GUI.Label(new Rect(guiCamera.pixelWidth*(guiMargin+0.5f),guiCamera.pixelHeight*guiMargin,100,30),players[1].Health);
		}
		else if(players.Count > 2) {
			GUI.Label(new Rect(guiCamera.pixelWidth*guiMargin,guiCamera.pixelHeight*(guiMargin+0.5f),100,30),players[0].Health);
			GUI.Label(new Rect(guiCamera.pixelWidth*(guiMargin+0.5f),guiCamera.pixelHeight*(guiMargin+0.5f),100,30),players[1].Health);
			GUI.Label(new Rect(guiCamera.pixelWidth*guiMargin,guiCamera.pixelHeight*guiMargin,100,30),players[2].Health);
			if(players.Count == 4) {
				GUI.Label(new Rect(guiCamera.pixelWidth*(guiMargin+0.5f),guiCamera.pixelHeight*guiMargin,100,30),players[3].Health);
			}
		}
	}
	void JoystickButton(int joystick, int button) {
		Debug.Log(joystick+":"+button);
		//start button
		if(button == 9) 
		{
			//already connected
			for(int i = 0; i < 4; i++) 
			{
				if(connected[i] == joystick) 
				{
					
					Debug.Log(joystick + " already connected");
					return;
				}
			}
			//connect
			for(int i = 0; i < 4; i++) 
			{
				if(connected[i] == -1) 
				{
					connected[i] = joystick;
					CreatePlayer(joystick);
					return;
				}
			}
		}
		
		/*End Button*/
		if (button == 8)
		{
			for(int i = 0; i < 4; i++) {
				if(connected[i] == joystick) 
				{
					connected[i] = -1;
					RemovePlayer(joystick);
					return;
				}
			}	
		}
	}
	
	void RemovePlayer(int playerNumber)
	{
		Debug.Log("RemovePlayer: " + playerNumber);
		Player player = null;
		foreach(Player p in players) {
			if(p.PlayerNumber == playerNumber) {
				player = p;
			}
		}
		if(player != null) {
			players.Remove(player);
			Destroy(player.gameObject);
		}
		//only remove a camera if there is one to remove
		if(cameras.Count > 1) {
			Camera cam = null;
			foreach(Camera c in cameras) {
				if(c.name == "Camera"+playerNumber) {
					cam = c;
				}
			}
			if(cam != null) {
				cameras.Remove(cam);
				Destroy(cam.gameObject);
			}
			
		}
		UpdateCameras();			
	}
	void CreatePlayer(int playerNumber){
		Debug.Log("CreatePlayer: "+playerNumber);
		GameObject go = new GameObject("Player"+playerNumber);
		go.transform.parent = transform;
		Player p = go.AddComponent<Player>();
		p.PlayerNumber = playerNumber;
		players.Add(p);
		p.transform.position = CircularBoard.Instance.GetSpawnPoint();
		
		go = (GameObject)Instantiate(fighterPrefab);
		go.transform.parent = p.transform;
		go.transform.localPosition = Vector3.zero;
		
		if(playerNumber == 0) {
			p.inputType = InputType.Keyboard;
		}
		else {
			p.inputType = InputType.Gamepad;
		}
		
		Camera c;
		if(players.Count == 1) {
			c = GetComponentInChildren<Camera>();
			c.name = "Camera"+playerNumber;
		}
		else {
			GameObject goCam = new GameObject("Camera"+playerNumber);
			c = goCam.AddComponent<Camera>();
		}
		if(!cameras.Contains(c)) {
			cameras.Add(c);
		}
		c.transform.parent = transform;
		SmoothFollow sf = c.GetComponent<SmoothFollow>();
		if(sf == null) {
			sf = c.gameObject.AddComponent<SmoothFollow>();
			sf.player = p;
		}
		sf.target = go.transform;
		sf.distance = 3;
		sf.rotationDamping = 15;
		SmoothLookAt sla = c.GetComponent<SmoothLookAt>();
		if(sla == null) {
			sla = c.gameObject.AddComponent<SmoothLookAt>();
		}
		sla.target = go.transform;

		UpdateCameras();
		
		go = (GameObject)Instantiate(turretPrefab);
		go.transform.parent = p.transform;
		go.transform.localPosition = Vector3.zero;
	}
	void UpdateCameras() {
		if(cameras.Count == 1) {
			cameras[0].rect = new Rect(0,0,1,1);
		}
		else if(cameras.Count == 2) {
			cameras[0].rect = new Rect(0,0,0.5f,1);
			cameras[1].rect = new Rect(0.5f,0,0.5f,1);
		}
		else if(cameras.Count == 3) {
			cameras[0].rect = new Rect(0,0,0.5f,0.5f);
			cameras[1].rect = new Rect(0.5f,0,0.5f,0.5f);
			cameras[2].rect = new Rect(0,0.5f,0.5f,0.5f);
		}
		else if(cameras.Count == 4) {
			cameras[0].rect = new Rect(0,0,0.5f,0.5f);
			cameras[1].rect = new Rect(0.5f,0,0.5f,0.5f);
			cameras[2].rect = new Rect(0,0.5f,0.5f,0.5f);
			cameras[3].rect = new Rect(0.5f,0.5f,0.5f,0.5f);
		}
		if(players.Count == 0) {
			cameras[0].transform.position = new Vector3(0,-25,0);
			cameras[0].transform.localEulerAngles = new Vector3(270,0,0);
		}
	}
	public Camera GetCameraForPlayer(int playerNumber) {
		Camera[] cs = GetComponentsInChildren<Camera>();
		foreach(Camera c in cs) {
			if(c.name == "Camera"+playerNumber) {
				return c;
			}
		}
		return null;
	}
}
