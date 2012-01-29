using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	PlayerMode playerMode;
	public InputType inputType;
	int playerNumber;
	float mass;
	IInputHandler handler;
	
	float massMax = 1000;
	float massMin = 1;
	
	//dirty trick to not do as much division at runtime
	float ratio;
	public bool IsFighter {
		get {
			return playerMode == PlayerMode.Fighter;
		}
	}
	public PlayerMode PlayerMode {
		get {
			return playerMode;
		}
	}
	public int PlayerNumber {
		get {
			return playerNumber;
		}
		set {
			playerNumber = value;
		}
	}
	public string Health {
		get {
			int i = (int)Mathf.Round(mass);
			return i.ToString();
		}
	}
	public float Mass {
		get {
			return mass;
		}
	}
	public float Ratio {
		get {
			return ratio;
		}
	}
	//as mass goes to zero ratio goes to 1
	//as mass goes to max ratio goes to 0
	public void SetMass(float m) {
		mass = m;
		ratio = (massMax - mass)/massMax;
	}
	// Use this for initialization
	void Start () {
		SetMass(11);
		SetPlayerMode(PlayerMode.Fighter);
	}
	// Update is called once per frame
	void Update () {	
		bool jump = false;
		bool attack = false;
		float x = 0;
		float y = 0;
		float z = 0;
		float w = 0;
		
		if(inputType == InputType.Gamepad) {
			try {
				//jump
				jump = Input.GetButtonDown("joystick "+playerNumber+" button 1");
				//attack
				attack = Input.GetButtonDown("joystick "+playerNumber+" button 0");
			
				x = Input.GetAxis("joystick "+playerNumber+" axis x");
				y = Input.GetAxis("joystick "+playerNumber+" axis y");
				z = Input.GetAxis("joystick "+playerNumber+" axis z");
				w = Input.GetAxis("joystick "+playerNumber+" axis w");
			}
			catch(System.Exception e) {
			}
		}
		if(inputType == InputType.Keyboard) {
			if(Input.GetKeyDown(KeyCode.Space)) {
				jump = true;
			}
			if(Input.GetKeyDown(KeyCode.E)) {
				attack = true;
			}
			if(Input.GetKey(KeyCode.D)) {
				x = 1;
			}
			if(Input.GetKey(KeyCode.A)) {
				x = -1;
			}
			if(Input.GetKey(KeyCode.W)) {
				y = -1;
			}
			if(Input.GetKey(KeyCode.S)) {
				y = 1;
			}
			z = Input.GetAxis("Mouse X");
		}
		if(handler != null) {
			handler.HandleInput(jump,attack,x,y,z,w);
		}
		else {
			Debug.Log("Handler NULL");
		}
	}
	public void SetPlayerMode(PlayerMode mode) {
		playerMode = mode;
		Transform targetTransform = null;
		foreach(Transform t in transform) {
			if(playerMode == PlayerMode.Fighter && t.name == "Fighter(Clone)") {
				targetTransform = t;
				Debug.Log("Fighter On");
				t.gameObject.SetActiveRecursively(true);
				handler = (IInputHandler)t.GetComponent("IInputHandler");
			}
			else if(playerMode == PlayerMode.Turret && t.name == "Turret(Clone)") {
				targetTransform = t;
				Debug.Log("Turret On");
				t.gameObject.SetActiveRecursively(true);
				handler = (IInputHandler)t.GetComponent("IInputHandler");
				t.transform.position = new Vector3(15,0,0);
				t.transform.localEulerAngles = new Vector3(0,270,0);
			}
			else {
				Debug.Log(t.name + " Off");
				t.gameObject.SetActiveRecursively(false);
			}
		}
		if(targetTransform != null) {
			Camera c = MultiManager.Instance.GetCameraForPlayer(playerNumber);
			if(c != null) {
				SmoothFollow sf = c.GetComponent<SmoothFollow>();
				if(sf == null) {
					sf = c.gameObject.AddComponent<SmoothFollow>();
					sf.player = this;
				}
				sf.target = targetTransform;
				sf.distance = 3;
				sf.rotationDamping = 15;
				SmoothLookAt sla = c.GetComponent<SmoothLookAt>();
				if(sla == null) {
					sla = c.gameObject.AddComponent<SmoothLookAt>();
				}
				sla.target = targetTransform;
			}
		}
	}
}
