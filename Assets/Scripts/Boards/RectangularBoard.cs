using UnityEngine;
using System.Collections;

public class RectangularBoard : MonoBehaviour {
	public int width;
	public int height;
	
	void Awake () {
		for(int w = 0; w < width; w++) {
			for(int h = 0; h < height; h++) {
				GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
				go.AddComponent<Voxel>();
				go.transform.parent = transform;
				go.transform.localPosition = new Vector3(w,0,h);
			}
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
