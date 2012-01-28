using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public InputType inputType;
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
	
	// Update is called once per frame
	void Update () {
		bool jump = false;
		bool attack = false;
		float x = 0;
		float y = 0;
		float z = 0;
		float w = 0;
		//jump
		try {
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
		
		if(inputType == InputType.Keyboard) {
			if(Input.GetKeyDown(KeyCode.Space)) {
				jump = true;
			}
			if(Input.GetKeyDown(KeyCode.E)) {
				attack = true;
			}
			x = Input.GetAxis("Horizontal");
			y = -Input.GetAxis("Vertical");
			z = Input.GetAxis("Mouse X");
		}
		
		
		if(networkView.isMine) {		
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
		
			if(cc.isGrounded && jump) {
				verticalSpeed = Mathf.Sqrt(2 * gravity)*jumpMultiplier;
			}
			if(dist < 0.1f && obj != null) {
				float actualDist = Vector3.Distance(transform.position,obj.transform.position);
				float desiredDist = (0.5f*cc.height + 0.5f*obj.transform.localScale.y);
				verticalSpeed = desiredDist-actualDist;
			}
			cc.transform.Rotate(Vector3.up*z*Time.deltaTime*turnMultiplier*ratio);
			cc.Move(cc.transform.rotation*(new Vector3(x*moveMultiplier*ratio,verticalSpeed*ratio,-y*moveMultiplier*ratio))*Time.deltaTime);
		}
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.name == "KillBox") {
			Debug.Log("KillBox");
			transform.position = new Vector3(0,5,0);
		}
	}
}
