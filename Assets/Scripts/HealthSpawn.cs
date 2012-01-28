using UnityEngine;
using System.Collections;

public class HealthSpawn : MonoBehaviour 
{
	private int initHealth;
	private int currentHealth;
	private int healthDrop = 10;
	public GameObject spawn;
	
	// Use this for initialization
	void Start () 
	{
		initHealth = 100;
		currentHealth = initHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (currentHealth <= 0)
		{
			respawn();
		}
	}
	
	void respawn()
	{
		Debug.Log("respawn()");
		transform.position = spawn.transform.position;
		transform.rotation = spawn.transform.rotation;
		currentHealth = initHealth;
	}
}
