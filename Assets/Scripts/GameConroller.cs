using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameConroller : MonoBehaviour
{
	public Color playerColor;
	public Color AIcolor;
	public bool isGameOver = false;
	public Material[] matList;
	public bool isAIfirstMove = false;
	bool firstRun;

	void Start ()
	{
		restartGame ();
	}

	void Update ()
	{
		if (isGameOver)
			return;
		if (firstRun && isAIfirstMove)
		{
			makeAIturn();
			firstRun = false;
		}
	}

	public void makeAIturn ()
	{
		ArrayList cubeList;
		getCubeList (true, out cubeList);
		if (cubeList.Count == 0)
			return;
		CubeControl selectedCube = cubeList[Random.Range (0, cubeList.Count - 1)] as CubeControl;
		selectedCube.select (false);
		if (checkWinState ())
			gameOver();
	}
	
	public bool checkWinState ()
	{
		ArrayList cubeList;
		getCubeList (false, out cubeList);
		if (cubeList.Count == 0)
			return false;
		ArrayList playerCubes = new ArrayList ();
		ArrayList AIcubes = new ArrayList ();
		foreach (object go in cubeList)
		{
			CubeControl cube = go as CubeControl;
			CubeControl cc = cube.GetComponent ("CubeControl") as CubeControl;
			if (cc != null && !cc.availible)
				if (cc.isPlayer)
					playerCubes.Add(cc);
				else
					AIcubes.Add(cc);
		}
		bool win = false;
		if (checkCoins (playerCubes))
		{
			getResultText().text = "You Win!";
			win = true;
		}
		else if (checkCoins (AIcubes))
		{
			getResultText().text = "You lose";
			win = true;
		}
		else if (playerCubes.Count + AIcubes.Count == cubeList.Count)
		{
			getResultText().text = "Draw.";
			gameOver ();
		}
		return win;
	}

	bool checkCoins(ArrayList cubsList)
	{
		int count = 0;
		for (int row = -1; row <= 1; ++row)
		{
			count = 0;
			for (int col = -1; col <= 1; ++col)
				foreach (CubeControl cube in cubsList) 
				{
					Vector2 pos = cube.getCubePos();
					if (pos.x == col && pos.y == row)
					++count;
				}
			if (count == 3)
			{
				Debug.Log("Row " + row + " win");
				return true;
			}
		}
		for (int col = -1; col <= 1; ++col)
		{
			count = 0;
			for (int row = -1; row <= 1; ++row)
			{
				foreach (CubeControl cube in cubsList)
				{
					Vector2 pos = cube.getCubePos();
					if (pos.x == col && pos.y == row)
						++count;
				}
			}
			if (count == 3)
			{
				Debug.Log("Col " + col + " win");
				return true;
			}
		}
		count = 0;
		for (int i = -1; i <= 1; ++i)
		{
			foreach (CubeControl cube in cubsList)
			{
				Vector2 pos = cube.getCubePos();
				if (pos.x == i && pos.y == i)
					++count;
			}
		}
		if (count == 3)
		{
			Debug.Log("/ win");
			return true;
		}
		count = 0;
		for (int i = -1; i <= 1; ++i)
		{
			foreach (CubeControl cube in cubsList)
			{
				Vector2 pos = cube.getCubePos();
				if (pos.x == i && pos.y == -i)
					++count;
			}
		}
		if (count == 3)
		{
			Debug.Log("\\ win");
			return true;
		}
		return false;
	}

	public void gameOver()
	{
		isGameOver = true;
		getMainWindow().enabled = true;
		getAIfirstMoveToggle ().enabled = true;
	}

	public void restartGame()
	{
		firstRun = true;
		getMainWindow ().enabled = false;
		ArrayList cubeList;
		getCubeList (false, out cubeList);
		foreach(CubeControl cube in cubeList)
			cube.reset();
		isGameOver = false;
		isAIfirstMove = getAIfirstMoveToggle ().GetComponentInChildren<Toggle> ().isOn;
		getAIfirstMoveToggle ().enabled = false;
	}

	public Canvas getMainWindow()
	{
		return GameObject.FindGameObjectWithTag ("MainWindow").GetComponent<Canvas>();
	}

	public Text getResultText()
	{
		return GameObject.FindGameObjectWithTag ("ResultText").GetComponent<Text>();
	}

	public Canvas getAIfirstMoveToggle()
	{
		return GameObject.FindGameObjectWithTag ("AIfirstMoveToggle").GetComponent<Canvas> ();
	}

	void getCubeList(bool onlyAvalible, out ArrayList cubeList)
	{
		cubeList = new ArrayList ();
		GameObject[] cubes = GameObject.FindGameObjectsWithTag ("Cube");
		if (cubes.Length == 0)
			return;
		foreach (GameObject cube in cubes)
		{
			CubeControl cc = cube.GetComponent("CubeControl") as CubeControl;
			if (cc != null && (cc.availible || !onlyAvalible))
				cubeList.Add(cc);
		}
	}

	public void setAIfirstMove(bool par)
	{
		isAIfirstMove = par;
	}
}
