using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once CheckNamespace
public class GameConroller : MonoBehaviour
{
    // ReSharper disable once UnassignedField.Global
	public Color PlayerColor;
    // ReSharper disable once UnassignedField.Global
	public Color AIcolor;
	public bool IsGameOver;
    // ReSharper disable once UnassignedField.Global
	public Material[] MatList;
	public bool IsAIfirstMove;
	bool _firstRun;

    // ReSharper disable once UnusedMember.Local
	void Start ()
	{
		RestartGame ();
		audio.Play ();
	}

    // ReSharper disable once UnusedMember.Local
	void Update ()
	{
		if (IsGameOver)
			return;
	    if (!_firstRun || !IsAIfirstMove) return;
	    MakeAIturn();
	    _firstRun = false;
	}

	public void MakeAIturn ()
	{
		ArrayList cubeList;
		GetCubeList (true, out cubeList);
		if (cubeList.Count == 0)
			return;
		var selectedCube = cubeList[Random.Range (0, cubeList.Count - 1)] as CubeControl;
	    if (selectedCube != null)
            selectedCube.Select (false);
	    if (CheckWinState ())
			GameOver();
	}
	
	public bool CheckWinState ()
	{
		ArrayList cubeList;
		GetCubeList (false, out cubeList);
		if (cubeList.Count == 0)
			return false;
		var playerCubes = new ArrayList ();
		var aiCubes = new ArrayList ();
        /*
        foreach (var go in cubeList)
        {
            var cube = go as CubeControl;
            if (cube == null) continue;
            var cc = cube.GetComponent("CubeControl") as CubeControl;
            if (cc == null || cc.Availible) continue;
            if (cc.IsPlayer)
                playerCubes.Add(cc);
            else
                aiCubes.Add(cc);
        }
        */
        foreach (
	        var cc in
	            cubeList.OfType<CubeControl>()
	                .Select(cube => cube.GetComponent("CubeControl") as CubeControl)
	                .Where(cc => cc != null && !cc.Availible))
		{
		    if (cc.IsPlayer)
		        playerCubes.Add(cc);
		    else
		        aiCubes.Add(cc);
		}
		var win = false;
		if (CheckCoins (playerCubes))
		{
			GetResultText().text = "You Win!";
			win = true;
		}
		else if (CheckCoins (aiCubes))
		{
			GetResultText().text = "You lose";
			win = true;
		}
		else if (playerCubes.Count + aiCubes.Count == cubeList.Count)
		{
			GetResultText().text = "Draw.";
			GameOver ();
		}
		return win;
	}

    static bool CheckCoins(IEnumerable cubsList)
	{
		int count;
        const double tolerance = 0.1f;
        var cubes = cubsList as IList<object> ?? cubsList.Cast<object>().ToList();
        for (var row = -1; row <= 1; ++row)
		{
			count = 0;
			for (var col = -1; col <= 1; ++col)
				foreach (CubeControl cube in cubes) 
				{
					var pos = cube.GetCubePos();
					if (Math.Abs(pos.x - col) < tolerance && Math.Abs(pos.y - row) < tolerance)
					++count;
				}
		    if (count != 3) continue;
		    Debug.Log("Row " + row + " win");
		    return true;
		}
		for (var col = -1; col <= 1; ++col)
		{
			count = 0;
			for (var row = -1; row <= 1; ++row)
			{
				foreach (CubeControl cube in cubes)
				{
					var pos = cube.GetCubePos();
					if (Math.Abs(pos.x - col) < tolerance && Math.Abs(pos.y - row) < tolerance)
						++count;
				}
			}
		    if (count != 3) continue;
		    Debug.Log("Col " + col + " win");
		    return true;
		}
		count = 0;
		for (var i = -1; i <= 1; ++i)
		{
			foreach (CubeControl cube in cubes)
			{
				var pos = cube.GetCubePos();
				if (Math.Abs(pos.x - i) < tolerance && Math.Abs(pos.y - i) < tolerance)
					++count;
			}
		}
		if (count == 3)
		{
			Debug.Log("/ win");
			return true;
		}
		count = 0;
		for (var i = -1; i <= 1; ++i)
		{
			foreach (CubeControl cube in cubes)
			{
				var pos = cube.GetCubePos();
				if (Math.Abs(pos.x - i) < tolerance && Math.Abs(pos.y - (-i)) < tolerance)
					++count;
			}
		}
        if (count != 3) return false;
        Debug.Log("\\ win");
        return true;
	}

	public void GameOver()
	{
		IsGameOver = true;
		GetMainWindow().enabled = true;
		GetAIfirstMoveToggle ().enabled = true;
	}

    public void RestartGame()
	{
		_firstRun = true;
		GetMainWindow ().enabled = false;
		ArrayList cubeList;
		GetCubeList (false, out cubeList);
		foreach(CubeControl cube in cubeList)
			cube.Reset();
		IsGameOver = false;
		IsAIfirstMove = GetAIfirstMoveToggle ().GetComponentInChildren<Toggle> ().isOn;
		GetAIfirstMoveToggle ().enabled = false;
	}

    private static Canvas GetMainWindow()
	{
		return GameObject.FindGameObjectWithTag ("MainWindow").GetComponent<Canvas>();
	}

    private static Text GetResultText()
	{
		return GameObject.FindGameObjectWithTag ("ResultText").GetComponent<Text>();
	}

    private static Canvas GetAIfirstMoveToggle()
	{
		return GameObject.FindGameObjectWithTag ("AIfirstMoveToggle").GetComponent<Canvas> ();
	}

    private static void GetCubeList(bool onlyAvalible, out ArrayList cubeList)
	{
		cubeList = new ArrayList ();
		var cubes = GameObject.FindGameObjectsWithTag ("Cube");
		if (cubes.Length == 0)
			return;
	    foreach (
	        var cc in
	            cubes.Select(cube => cube.GetComponent("CubeControl") as CubeControl)
	                .Where(cc => cc != null && (cc.Availible || !onlyAvalible))) 
		{
		    cubeList.Add(cc);
		}
	}

    // ReSharper disable once UnusedMember.Global
	public void SetAIfirstMove(bool par)
	{
		IsAIfirstMove = par;
	}
}
