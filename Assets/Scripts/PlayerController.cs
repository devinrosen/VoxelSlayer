using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public int playerNumber;
	
	public float massMax = 1000;
	public float massMin = 1;
	float mass;
	public float jumpMultiplier = 1;
	public float moveMultiplier = 1;
	public float turnMultiplier = 1;
	
	CharacterController cc;
	
	float verticalSpeed;
	public float gravity = 9.8f;
	
	//dirty trick to not do as much division at runtime\
	float ratio;
	
	float horizontal;
	float vertical;
	
	Transform camTarget;
	// Use this for initialization
	void Start () {
		cc = gameObject.GetComponent<CharacterController>();
		ratio = (massMax - mass)/massMax;
		foreach(Transform t in transform) {
			if(t.name == "CamTarget") {
				camTarget = t;
			}
		}
	}
	//as mass goes to zero ratio goes to 1
	//as mass goes to max ratio goes to 0
	void SetMass(float m) {
		mass = m;
		ratio = (massMax - mass)/massMax;
	}
	
	public void ButtonPressed(int button) {
		Debug.Log(playerNumber +":"+button);
	}
	public void Axis(int axis, float value) {
	}
	
	// Update is called once per frame
	void Update () {
		//jump
		if(Input.GetButtonDown("joystick "+playerNumber+" button 1")) {
				
		}
		//hit
		if(Input.GetButtonDown("joystick "+playerNumber+" button 0")) {
				
		}

		/*
		if(networkView.isMine) {
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");
		
			RaycastHit hit;
			float dist = 0;
			GameObject obj = null;
			if(Physics.Raycast(cc.transform.position,-Vector3.up,out hit)) {
				dist = hit.distance;
				obj = hit.collider.gameObject;
			}
			if (cc.isGrounded) {
				verticalSpeed = -gravity;
			}
			else {
				verticalSpeed -= gravity * Time.deltaTime;
			}
		
			if(cc.isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))) {
				verticalSpeed = Mathf.Sqrt(2 * gravity)*jumpMultiplier;
			}
			if(dist < 0.1f && obj != null) {
				float actualDist = Vector3.Distance(transform.position,obj.transform.position);
				float desiredDist = (0.5f*cc.height + 0.5f*obj.transform.localScale.y);
				verticalSpeed = desiredDist-actualDist;
			}
			float camHorizontal = Input.GetAxis("Mouse X");
			float joyHorizontal = Input.GetAxis("JoyZ");
			if(joyHorizontal != 0) {
				Debug.Log(joyHorizontal);
				camHorizontal = joyHorizontal;
			}
			cc.transform.Rotate(Vector3.up*camHorizontal*Time.deltaTime*turnMultiplier*ratio);
			cc.Move(cc.transform.rotation*(new Vector3(horizontal*moveMultiplier*ratio,verticalSpeed*ratio,vertical*moveMultiplier*ratio))*Time.deltaTime);
		}
		*/
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.name == "KillBox") {
			Debug.Log("KillBox");
			transform.position = new Vector3(0,5,0);
		}
	}
}
