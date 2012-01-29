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
	Vector3 velocity;
	float costToShoot = 20;
	// Use this for initialization
	void Start () {
		player = transform.parent.GetComponent<Player>();
		cc = gameObject.GetComponent<CharacterController>();
		anim = gameObject.GetComponentInChildren<Animation>();
		foreach(AnimationState animState in anim) {
			if(animState.name == "Striking_3")
				animState.speed = 10;
			else
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
			player.Die();
		}
		else if( velocity.x == 0 && velocity.z == 0) {
			anim.CrossFade("neutral");
		}
		else if(velocity.x != 0 || velocity.z != 0) {
			anim.CrossFade("running");
		}
	}
	public void HandleInput(bool jump, bool attack,bool leftPunch,bool rightPunch, float x, float y, float z, float w) {
		if(cc == null) {
			return;
		}
		if(leftPunch) {
			anim.Play("Striking_1");
		}
		if(rightPunch) {
			anim.Play("Striking_3");
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
		
		velocity.y = verticalSpeed;
		Vector3 curPosition = cc.transform.position;
		cc.transform.Rotate(Vector3.up*z*Time.deltaTime*turnMultiplier*player.Ratio);
		cc.Move(cc.transform.rotation*(new Vector3(x*moveMultiplier*player.Ratio,verticalSpeed*player.Ratio,-y*moveMultiplier*player.Ratio))*Time.deltaTime);
		velocity.x = cc.transform.position.x - curPosition.x;
		velocity.z = cc.transform.position.z  - curPosition.z;
		
		if(attack && player.Mass > costToShoot + 10) {
			anim.Play("Striking_2");
			Fire();
		}
	}
	void Fire() {
		float bulletScale = 1f;
		
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.tag = "Bullet";
		go.transform.localScale = new Vector3(bulletScale,bulletScale,bulletScale);
		
		Bullet bullet = go.AddComponent<Bullet>();
		bullet.SourceGameObject = transform.parent.gameObject;
		
		
		Collider c = go.GetComponent<Collider>();
		c.isTrigger = true;
		
		Rigidbody rb = go.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.AddForce(transform.forward*1500);
		
		go.transform.position = transform.position+new Vector3(0,0.5f*transform.localScale.y,0);
		player.SetMass(player.Mass-costToShoot);
	}
	public void Spawn() {
		player.SetMass(player.Mass*0.9f);
		transform.position = MultiManager.Instance.Board.GetSpawnPoint();
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.tag == "Bullet") {
			Bullet bullet = hit.collider.GetComponent<Bullet>();
			if(bullet.SourceGameObject != player.gameObject) {
				Debug.Log("PlayerController Bullet");
				player.SetMass(player.Mass-20);
			}
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
