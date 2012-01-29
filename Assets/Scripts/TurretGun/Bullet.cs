using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public GameObject sourceGameObject;
	
	float minrot = 250;
	float maxrot = 450;
	float xRot;
	float yRot;
	// Use this for initialization
	void Start () {
		xRot = Random.Range(minrot,maxrot);
		if(Random.Range(0.0f,1.0f) < 0.5f) {
			xRot *= -1;
		}
		yRot = Random.Range(minrot,maxrot);
		if(Random.Range(0.0f,1.0f) < 0.5f) {
			yRot *= -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(xRot*Time.deltaTime,yRot*Time.deltaTime,0));
	}
	void OnTriggerExit(Collider _collider) {
		if(sourceGameObject != null && _collider.gameObject == sourceGameObject) {
			return;
		}
		
		if(_collider.tag == "Bullet") {
		}
			
		
		if(_collider.tag == "Player") {
			Player p = _collider.gameObject.transform.parent.GetComponent<Player>();
			if(p != null) {
				//if big bullet
				if(sourceGameObject == null) {
					p.SetMass(p.Mass - 25);
				}
				//if player bullet
				else {
					p.SetMass(p.Mass - 10);
				}
			}
			Destroy(gameObject);
		}
		if(_collider.tag == "KillBox") {
			Destroy(gameObject);
		}
	}
}
