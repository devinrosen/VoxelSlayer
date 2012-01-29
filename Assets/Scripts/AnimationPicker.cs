using UnityEngine;
using System.Collections;

public class AnimationPicker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			animation.Play("running");
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			animation.Play("nuetral");
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			animation.Play("Jump");
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			animation.Play("Striking_1");
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)) {
			animation.Play("Striking_2");
		}
		if(Input.GetKeyDown(KeyCode.Alpha6)) {
			animation.Play("Striking_3");
		}
		if(Input.GetKeyDown(KeyCode.Alpha7)) {
			animation.Play("walkcycle");
		}
		if(Input.GetKeyDown(KeyCode.Alpha8)) {
			animation.Play("Jump");
		}
		if(Input.GetKeyDown(KeyCode.Alpha9)) {
			animation.Play("Jump");
		}
	}
}
