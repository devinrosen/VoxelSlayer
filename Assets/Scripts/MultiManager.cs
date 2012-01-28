using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiManager : MonoBehaviour {
	public GameObject playerPrefab;
	List<PlayerController> players;
	List<Camera> cameras;
	int[] connected;
	// Use this for initialization
	void Start () {
		players = new List<PlayerController>();
		cameras = new List<Camera>();
		connected = new int[]{-1,-1,-1,-1};	
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
	}
	void JoystickButton(int joystick, int button) {
		Debug.Log(button);
		
		foreach(PlayerController pc in players) {
			if(pc.playerNumber == joystick) {
				pc.ButtonPressed(button);
				return;
			}
		}
					
					
		//start button
		if(button == 9) {
			//already connected
			for(int i = 0; i < 4; i++) {
				if(connected[i] == joystick) {
					return;
				}
			}
			//connect
			for(int i = 0; i < 4; i++) {
				if(connected[i] == -1) {
					connected[i] = joystick;
					CreatePlayer(joystick);
					return;
				}
			}
		}
	}
	void CreatePlayer(int playerNumber){
		GameObject go = (GameObject)Instantiate(playerPrefab);
		go.transform.parent = transform;
		PlayerController pc = go.GetComponent<PlayerController>();
		pc.playerNumber = playerNumber;
		players.Add(pc);
		
		Camera c;
		if(players.Count == 1) {
			c = GetComponentInChildren<Camera>();
		}
		else {
			GameObject goCam = new GameObject("Camera"+playerNumber);
			c = goCam.AddComponent<Camera>();
		}
		cameras.Add(c);
		c.transform.parent = transform;
		SmoothFollow sf = c.GetComponent<SmoothFollow>();
		if(sf == null) {
			sf = c.gameObject.AddComponent<SmoothFollow>();
		}
		sf.target = pc.transform;
		SmoothLookAt sla = c.GetComponent<SmoothLookAt>();
		if(sla == null) {
			sla = c.gameObject.AddComponent<SmoothLookAt>();
		}
		sla.target = pc.transform;
		if(cameras.Count == 1) {
			c.rect = new Rect(0,0,1,1);
		}
		else if(cameras.Count == 2) {
			cameras[0].rect = new Rect(0,0,0.5f,1);
			cameras[1].rect = new Rect(0.5f,0,0.5f,1);
		}
		
	}
}
