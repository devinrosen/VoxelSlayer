using UnityEngine;
using System.Collections;

public class TurretGun : MonoBehaviour {
	public float propSpeed = 3.0f;
	public float rotationSpeed = 15.0f;
	
	public float bulletScale = 0.5f;
	public int bulletsPerShot = 3;
	public float spread = 5;
	
	GameObject prop;
	GameObject canon;
	Transform canonJoint;
	Transform fire;
	
	float fireDelta = 0;
	public float fireTime = 3;
	
	public float amplitude;
	public float cycle;
	
	void Awake () {
		Transform[] ts = GetComponentsInChildren<Transform>();
		foreach(Transform t in ts) {
			if(t.name == "Prop") {
				prop = t.gameObject;
			}
			else if(t.name == "Canon") {
				canon = t.gameObject;
			}
			else if(t.name == "CanonJoint") {
				canonJoint = t;
			}
			else if(t.name == "Fire") {
				fire = t;
			}
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = prop.transform.localEulerAngles;
		v.y += Time.deltaTime*propSpeed;
		prop.transform.localEulerAngles = v;
		
		transform.RotateAround(Vector3.zero,Vector3.up,rotationSpeed*Time.deltaTime);
		v = transform.position;
		v.y = amplitude*Mathf.Cos(cycle*Time.deltaTime);
		transform.position = v;
		canonJoint.LookAt(new Vector3(0,canonJoint.position.y,0));
		
		fireDelta += Time.deltaTime;
		if(fireDelta > fireTime) {
			fireDelta = 0;
			Fire();
		}
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			Fire();
		}
		bulletScale += Time.deltaTime*0.02f;//0.01f;
		if (bulletScale > 2.5f)
		{
			bulletScale = 0.5f;
		}
	}
	void Fire() {
		Vector3 f = -fire.position;
		f.y = 0;
		
		GameObject player = GameObject.FindWithTag("Player");
		f = player.transform.position - fire.position;
		Vector3 cross = Vector3.Cross(Vector3.up,-fire.position);
		for(float i = -0.5f*(bulletsPerShot-1); i <= 0.5f*(bulletsPerShot-1); i++) {
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			go.AddComponent<NetworkView>();
			go.tag = "Bullet";
			go.transform.localScale = new Vector3(bulletScale,bulletScale,bulletScale);
			Collider c = go.GetComponent<Collider>();
			c.isTrigger = true;
			Rigidbody rb = go.AddComponent<Rigidbody>();
			rb.useGravity = false;
			rb.AddForce(f*75+i*spread*cross);
			go.AddComponent<Bullet>();
			go.transform.position = fire.position;
		}
	}
}
