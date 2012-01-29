using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour,IInputHandler {
	float deltaWait = 0;
	float maxWait = 2.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void HandleInput(bool jump, bool attack, bool leftPunch, bool rightPunch, float x, float y, float z, float w) {
		if(x != 0) {
			transform.RotateAround(Vector3.zero,Vector3.up,-x*Time.deltaTime*100);
		}
		
		if(attack) {
			gameObject.SendMessage("Fire");
		}
		deltaWait += Time.deltaTime;
		if(deltaWait > maxWait) {
			deltaWait = 0;
			Player p = transform.parent.GetComponent<Player>();
			p.SetPlayerMode(PlayerMode.Fighter);
		}
	}
}
