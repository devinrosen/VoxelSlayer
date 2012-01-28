using UnityEngine;
using System.Collections;

public class Voxel : MonoBehaviour {
	float amplitude;
	float speed;
	float dir = 1;
	
	bool dropping;
	// Use this for initialization
	void Start () {
		dropping = false;
		amplitude = Random.Range(0,0.25f);
		speed = Random.Range(0,0.5f);
		if(Random.Range(0.0f,1.0f) < 0.5f) {
			dir *= -1;
		}
		renderer.material = new Material(Shader.Find("Diffuse"));
		float c = Random.Range(0.25f,0.75f);
		renderer.material.color = new Color(c,c,c,1);
	}
	
	// Update is called once per frame
	void Update () {
		if(!dropping) {
			Vector3 v = transform.localPosition;
			v.y += speed*Time.deltaTime*dir;
			if(v.y > amplitude || v.y < -amplitude) {
				dir *= -1;
			}
			transform.localPosition = v;
		}
	}
	public void Drop() {
		dropping = true;
		Rigidbody rb = gameObject.AddComponent<Rigidbody>();
		if(rb != null) {
			rb.useGravity = true;
		}
	}
	void OnCollisionEnter(Collision collision) {
		if(collision.collider.tag == "KillBox") {
			Destroy(gameObject);
			CircularBoard.Instance.DropHealth();
		}
	}
}
