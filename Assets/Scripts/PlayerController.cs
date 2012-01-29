using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IInputHandler {
	public float jumpMultiplier = 1;
	public float moveMultiplier = 1;
	public float turnMultiplier = 1;

	CharacterController cc;
	
	float verticalSpeed;
	public float gravity = 9.8f;
	
	Player player;
	
	// Use this for initialization
	void Start () {
		player = transform.parent.GetComponent<Player>();
		cc = gameObject.GetComponent<CharacterController>();
	}
	void Update() {
		float f = player.Mass/200.0f;
		transform.localScale = new Vector3(f,f,f);
		if(player.Mass < 10) {
			player.SetPlayerMode(PlayerMode.Turret);
		}
	}
	public void HandleInput(bool jump, bool attack, float x, float y, float z, float w) {
		if(cc == null) {
			return;
		}
		
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
		cc.transform.Rotate(Vector3.up*z*Time.deltaTime*turnMultiplier*player.Ratio);
		cc.Move(cc.transform.rotation*(new Vector3(x*moveMultiplier*player.Ratio,verticalSpeed*player.Ratio,-y*moveMultiplier*player.Ratio))*Time.deltaTime);
	}
	public void Spawn() {
		player.SetMass(player.Mass*0.9f);
		transform.position = CircularBoard.Instance.GetSpawnPoint();
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.name == "KillBox") {
			Spawn();
		}
		else if(hit.collider.tag == "Health") {
			player.SetMass(player.Mass*1.1f);
			Destroy(hit.collider.gameObject);
		}
	}
}
