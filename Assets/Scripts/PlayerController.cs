using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IInputHandler {
	public float jumpMultiplier = 1;
	public float moveMultiplier = 1;
	public float turnMultiplier = 1;
	
	Transform lhTransform;
	Transform rhTransform;
	
	CharacterController cc;
	
	float verticalSpeed;
	public float gravity = 9.8f;
	
	Player player;
	Animation anim;
	// Use this for initialization
	void Start () {
		player = transform.parent.GetComponent<Player>();
		cc = gameObject.GetComponent<CharacterController>();
		anim = gameObject.GetComponentInChildren<Animation>();
		foreach(AnimationState animState in anim) {
			animState.speed = 5;
		}
		SkinnedMeshRenderer[] smrs = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach(SkinnedMeshRenderer smr in smrs) {
			//Debug.Log(smr.name);
			if(smr.name == "Cylinder") {
				lhTransform = smr.transform;
				//lhTransform.gameObject.AddComponent<Fist>();
				//SphereCollider sc = lhTransform.gameObject.AddComponent<SphereCollider>();
				//sc.isTrigger = true;
			}
			else if(smr.name == "Cylinder_001") {
				rhTransform = smr.transform;
				//rhTransform.gameObject.AddComponent<Fist>();
				//SphereCollider sc = rhTransform.gameObject.AddComponent<SphereCollider>();
				//sc.isTrigger = true;
			}
		}	
	}
	void Update() {
		float f = player.Mass/200.0f;
		transform.localScale = new Vector3(f,f,f);
		if(player.Mass < 10) {
			player.SetPlayerMode(PlayerMode.Turret);
		}
	}
	public void HandleInput(bool jump, bool attack,bool leftPunch,bool rightPunch, float x, float y, float z, float w) {
		if(cc == null) {
			return;
		}
		if(leftPunch) {
			anim.Play("Armature_001Action");
			//anim.Blend("Armature_001Action",1,1);	
		}
		if(rightPunch) {
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
		
		if(attack) {
			Fire();
		}
	}
	void Fire() {
		float bulletScale = 0.1f;
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Bullet bullet = go.AddComponent<Bullet>();
		bullet.sourceGameObject = gameObject;
		go.tag = "Bullet";
		go.transform.localScale = new Vector3(bulletScale,bulletScale,bulletScale);
		Collider c = go.GetComponent<Collider>();
		c.isTrigger = true;
		Rigidbody rb = go.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.AddForce(transform.forward*750);
		go.AddComponent<Bullet>();
		go.transform.position = transform.position+new Vector3(0,0.5f*cc.height,0);
		player.SetMass(player.Mass-5);
	}
	public void Spawn() {
		player.SetMass(player.Mass*0.9f);
		transform.position = MultiManager.Instance.Board.GetSpawnPoint();
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.tag == "Bullet") {
			player.SetMass(player.Mass-20);
		}
		if(hit.collider.name == "KillBox") {
			Spawn();
		}
		else if(hit.collider.tag == "Health") {
			player.SetMass(player.Mass*1.1f);
			Destroy(hit.collider.gameObject);
		}
	}
}
