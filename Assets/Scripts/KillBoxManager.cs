using UnityEngine;
using System.Collections;

public class KillBoxManager : MonoBehaviour {
	Transform n;
	Transform s;
	Transform e;
	Transform w;
	Transform top;
	Transform bottom;
	
	public float x;
	public float y;
	public float z;
	
	float mX;
	float mY;
	float mZ;
	
	public float thick;
	float mThick;
	void Awake() {
		mX = x;
		mY = y;
		mZ = z;
		GameObject go;
		
		//N
		go = new GameObject("KillBox");
		go.layer = 2;
		go.tag = "KillBox";
		go.AddComponent<BoxCollider>();
		go.AddComponent("KillBox");
		n = go.transform;
		n.parent = transform;
		
		//S
		go = new GameObject("KillBox");
		go.layer = 2;
		go.tag = "KillBox";
		go.AddComponent<BoxCollider>();
		go.AddComponent("KillBox");
		s = go.transform;
		s.parent = transform;
		
		
		//E
		go = new GameObject("KillBox");
		go.layer = 2;
		go.tag = "KillBox";
		go.AddComponent<BoxCollider>();
		go.AddComponent("KillBox");
		e = go.transform;
		e.parent = transform;
		
		//W
		go = new GameObject("KillBox");
		go.layer = 2;
		go.tag = "KillBox";
		go.AddComponent<BoxCollider>();
		go.AddComponent("KillBox");
		w = go.transform;
		w.parent = transform;
	
		//Top
		go = new GameObject("KillBox");
		go.layer = 2;
		go.tag = "KillBox";
		go.AddComponent<BoxCollider>();
		go.AddComponent("KillBox");
		top = go.transform;
		top.parent = transform;
		
		//Bottom
		go = new GameObject("KillBox");
		go.layer = 2;
		go.tag = "KillBox";
		go.AddComponent<BoxCollider>();
		go.AddComponent("KillBox");
		bottom = go.transform;
		bottom.parent = transform;		
		
		UpdateKillBox();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(x != mX || y != mY || z != mZ || thick != mThick) {
			mX = x;
			mY = y;
			mZ = z;
			mThick = thick;
			UpdateKillBox();
		}
	}
	void UpdateKillBox() {
		n.localScale = new Vector3(mThick,2*mY,2*mZ);
		n.localPosition = new Vector3(mX,0,0);
		
		s.localScale = new Vector3(mThick,2*mY,2*mZ);
		s.localPosition = new Vector3(-mX,0,0);
		
		e.localScale = new Vector3(2*mX,2*mY,mThick);
		e.localPosition = new Vector3(0,0,mZ);
		
		w.localScale = new Vector3(2*mX,2*mY,mThick);
		w.localPosition = new Vector3(0,0,-mZ);
		
		top.localScale = new Vector3(2*mX,mThick,2*mZ);
		top.localPosition = new Vector3(0,mY,0);
		
		bottom.localScale = new Vector3(2*mX,mThick,2*mZ);
		bottom.localPosition = new Vector3(0,-mY,0);
	}
}
