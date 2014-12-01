using UnityEngine;
using System.Collections;

public class CubeControl : MonoBehaviour {
	public Light cubeLight;
	public bool isPlayer;
	public bool availible = true;

	void Update ()
	{
	}

	public Vector2 getCubePos ()
	{
		float x = 0f;
		float y = 0f;
		if (transform.position.x > 0)
			x = 1;
		else if (transform.position.x < 0)
			x = -1;
		if (transform.position.z > 0)
			y = 1;
		else if (transform.position.z < 0)
			y = -1;
		return new Vector2 (x, y);
	}

	public void select (bool player)
	{
		renderer.enabled = true;
		availible = false;
		isPlayer = player;
		GameConroller gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameConroller> ();
		if (isPlayer)
			cubeLight.color = gc.playerColor;
		else
			cubeLight.color = gc.AIcolor;
	}

	public void reset ()
	{
		renderer.enabled = false;
		availible = true;
		cubeLight.color = Color.white;
	}
}
