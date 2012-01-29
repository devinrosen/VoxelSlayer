using UnityEngine;
using System.Collections;

public class CircularBoard : MonoBehaviour,IBoard {
	float delta;
	float deltaMax;
	
	public int radius = 10;
	Voxel[] voxels;
	void Awake () {
		ResetBoard();
	}
	// Use this for initialization
	void Start () {
		
	}
	public void ResetBoard() {
		foreach(Transform t in transform) {
			Destroy(t.gameObject);
		}
		
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
			if(voxels.Length > 0 && voxels[i] != null) {
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
		while(voxel == null && voxels.Length > 0) {
			i = Random.Range(0,voxels.Length);
			voxel = voxels[i];
			if(voxel == null) {
				voxels = GetComponentsInChildren<Voxel>();
			}
		}
		Vector3 v = Vector3.zero;
		if(voxel != null) {
			v = voxel.transform.position;
			v.y = 3;
		}
		return v;
	}
	public void DropHealth() {
		GameObject go = (GameObject)Instantiate(MultiManager.Instance.healthPrefab);
		go.transform.position = GetSpawnPoint();
		go.transform.parent = transform;
	}
}
