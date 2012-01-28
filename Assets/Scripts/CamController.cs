using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {
	GameObject player;
	Transform targetTransform;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
		foreach(Transform t in player.transform) {
			if(t.name == "CamTarget") {
				targetTransform = t;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v =  targetTransform.position - transform.position;
		float m = v.magnitude;
		v *= 15f*m;
		transform.position += v*Time.deltaTime;
		transform.LookAt(player.transform.position);
	}
}
