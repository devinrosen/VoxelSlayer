using UnityEngine;
using System.Collections;

public class CircularBoard : MonoBehaviour {
	private static CircularBoard instance = null;
	public static CircularBoard Instance {
		get {
			if(instance == null) {
				instance = FindObjectOfType(typeof(CircularBoard)) as CircularBoard;
			}
			return instance;
		}
	}
	
	float delta;
	float deltaMax;
	
	public int radius;
	Voxel[] voxels;
	void Awake () {
		for(int w = -radius; w < radius; w++) {
			for(int h = -radius; h < radius; h++) {
				if(Mathf.Sqrt(w*w+h*h) < radius) {
					GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
					go.AddComponent<Voxel>();
					go.transform.parent = transform;
					go.transform.localPosition = new Vector3(w,0,h);
				}
			}
		}
		voxels = GetComponentsInChildren<Voxel>();
	}
	// Use this for initialization
	void Start () {
		delta = 0;
		deltaMax = Random.Range(0.5f,1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		delta += Time.deltaTime;
		if(delta > deltaMax) {
			delta = 0;
			deltaMax = Random.Range(0.5f,1.5f);
			int i = Random.Range(0,voxels.Length);
			if(voxels[i] != null) {
				voxels[i].Drop();
			}
			else {
				voxels = GetComponentsInChildren<Voxel>();
			}
		}
	}
	public Vector3 GetSpawnPoint() {
		int i;
		Voxel voxel = null;	
		while(voxel == null) {
			i = Random.Range(0,voxels.Length);
			voxel = voxels[i];
		}
		Vector3 v = voxel.transform.position;
		v.y = 7;
		return v;
	}
}
