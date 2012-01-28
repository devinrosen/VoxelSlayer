using UnityEngine;
using System.Collections;

public class HealthBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision collision) {
		if(collision.collider.tag == "KillBox") {
			transform.position = CircularBoard.Instance.GetSpawnPoint();
		}
	}
}
