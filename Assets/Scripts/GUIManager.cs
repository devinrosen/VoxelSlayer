using UnityEngine;
using System;
	
public class GUIManager : MonoBehaviour
{
	GameObject title;
	//Display game title.
		
	/*Display Controls
	* Space(Start Button)- Begin Game
	* Joystick 1(WASD) - Move
	* Joystick 2(Mouse) - Look
	* Space(Button A) - Jump
	* Left Click(Left bumper) - Left punch;
	* Right Click (Right bumper) - Right punch;
	* E (Button X) - Shoot
	**/
			
	enum Stage{ Menu, Learn, Play, Credits};
	Stage current;
	int buttonX = 100;
	int buttonY = 50;
	String controls =
					  "Begin Game - Space (Start Button)" + Environment.NewLine +
					  "Move - Joystick 1(WASD)" + Environment.NewLine +
					  "Look - Joystick 2(Mouse)" + Environment.NewLine +
					  "Change Level - Arrow Keys(Left Trigger/Right Trigger)" + Environment.NewLine +
					  "Left Punch - Left Click (Left bumper)" + Environment.NewLine +
					  "Right Punch - Right Click (Right bumper)" + Environment.NewLine +
					  "Jump - Space (Button A)" + Environment.NewLine +
					  "Shoot - E (Button X)";
	
	String credits ="Game Credits" + Environment.NewLine +
					"Programming - Devin Rosen" + Environment.NewLine +
					"Modeling - Joseph Rivera" + Environment.NewLine +
					"Animations - Steve Peters" + Environment.NewLine +
					"Music - Charles Schultz" + Environment.NewLine + 
					"Game Design - Yaciel Coto" + Environment.NewLine +
					"Dinosaur Handler - Lazaro Herrera"  + Environment.NewLine + " " + Environment.NewLine +
					"Special Thanks" + Environment.NewLine +
					"Frank Hernandez" + Environment.NewLine+  "Steve Louis" + Environment.NewLine + "Game Developers Guild" ;
					
	
	void Start()
	{
		title = GetComponentInChildren<TextMesh>().gameObject;
		current = Stage.Menu;	
	}
		
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && current != Stage.Play){
			Application.Quit();
		}
		
		if (MultiManager.Instance.PlayerCount > 0 && current != Stage.Play){
			current = Stage.Play;
			title.active = false;
		}
		else if(MultiManager.Instance.PlayerCount == 0 && current == Stage.Play) {
			current = Stage.Menu;
			title.active = true;
		}
	}
	
	void OnGUI()
	{
		if(current == Stage.Menu)
		{
			//Drop Title Here. Thank you.
			GUI.Box (new Rect(Screen.width/2 - buttonX, Screen.height/2 - buttonY, buttonX*2, buttonY*3.2f), "Player Menu");
			if(GUI.Button(new Rect(Screen.width/2- buttonX/2, Screen.height/2 - buttonY/2, buttonX, buttonY),"Play/Learn"))
			{
				//Takes you to "learn"
				current = Stage.Learn;
			}
			else if (GUI.Button(new Rect(Screen.width/2- buttonX/2, Screen.height/2 - buttonY/2 + buttonY*1.5f, buttonX, buttonY),"Credits"))
			{
				//Takes you to "Credits"
				current = Stage.Credits;
			}
		}
		else if (current == Stage.Learn)
		{
			GUI.Box(new Rect(Screen.width/2 - buttonX*1.5f, Screen.height/2 - buttonY*2, buttonX*3.5f, buttonY *4.3f),"Instructions");
			GUI.Label(new Rect(Screen.width/2 - buttonX, Screen.height/2 - buttonY - 20, buttonX*3, buttonY *4), controls);
			//Print instructions. Check for "start" game events and have a "return button"
			if (GUI.Button(new Rect(Screen.width/2- buttonX/2, Screen.height/2 - buttonY/2 + buttonY*3, buttonX, buttonY),"Return To Main"))
			{
				//Takes you to "Menu"
				current = Stage.Menu;
			}
		}
		else if(current == Stage.Credits)
		{
			//Print credits and have a "return button".
			GUI.Box(new Rect(Screen.width/2 - buttonX*1.5f, Screen.height/2 - buttonY*2, buttonX*3.5f, buttonY *4.5f),"");
			GUI.Label(new Rect(Screen.width/2 - buttonX, Screen.height/2 - buttonY-45, buttonX*3, buttonY *4), credits);
			if (GUI.Button(new Rect(Screen.width/2- buttonX/2, Screen.height/2 - buttonY/2 + buttonY*3, buttonX, buttonY),"Return To Main"))
			{
				//Takes you to "Menu"
				current = Stage.Menu;
			}
		}
	}
}

