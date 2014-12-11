using UnityEngine;
using System.Collections;

public class CubeControl : MonoBehaviour {
	public Light cubeLight;
	public bool isPlayer;
	public bool availible = true;
	GameConroller gameCon;

	void Start()
	{
		gameCon = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameConroller>();
	}

	void Update ()
	{
		if (!availible)
			transform.Rotate (new Vector3(0, 50, 0) * Time.deltaTime);
	}

	void OnMouseDown ()
	{
		if (gameCon.isGameOver) 
			return;
		if (gameCon != null && availible)
		{
			select(true);
			if (gameCon.checkWinState())
			{
				gameCon.gameOver();
				return;
			}
			gameCon.makeAIturn();
		}
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
		if (isPlayer)
			cubeLight.color = gameCon.playerColor;
		else
			cubeLight.color = gameCon.AIcolor;
		renderer.material.CopyPropertiesFromMaterial (gameCon.matList [(gameCon.isAIfirstMove && player || !gameCon.isAIfirstMove && !player) ? 1 : 0]);
	}

	public void reset ()
	{
		renderer.enabled = false;
		availible = true;
		cubeLight.color = Color.white;
	}
}
