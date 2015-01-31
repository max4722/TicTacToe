using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameConroller : MonoBehaviour
{
	public Color PlayerColor;
	public Color AIcolor;
	public bool IsGameOver;
	public Material[] MatList;
	public bool IsAIfirstMove;
	private bool _firstRun;

    private Canvas _mainWindow;
    public Canvas MainWindow
    {
        get
        {
            if (!_mainWindow)
                _mainWindow = GameObject.FindGameObjectWithTag("MainWindow").GetComponent<Canvas>();
            return _mainWindow;
        }
    }

    private Text _resultText;
    public Text ResultText
    {
        get
        {
            if (!_resultText)
            _resultText = GameObject.FindGameObjectWithTag("ResultText").GetComponent<Text>();
            return _resultText;
        }
    }

    private Canvas _aiFirstMoveToggle; 
    public Canvas AIfirstMoveToggle
    {
        get
        {
            if (!_aiFirstMoveToggle)
                _aiFirstMoveToggle = GameObject.FindGameObjectWithTag("AIfirstMoveToggle").GetComponent<Canvas>();
            return _aiFirstMoveToggle;
        }
    }

    private ArrayList _cubes;
    public ArrayList Cubes
    {
        get { return _cubes ?? (_cubes = new ArrayList(GameObject.FindGameObjectsWithTag("Cube"))); }
    }


	private void Start ()
	{
		RestartGame ();
		audio.Play ();
	}

	private void Update ()
	{
		if (IsGameOver)
			return;
	    if (!_firstRun || !IsAIfirstMove) return;
	    MakeAIturn();
	    _firstRun = false;
	}

	public void MakeAIturn ()
	{
        var cubeList = GetCubeList(true).ToArray();
		if (cubeList.Length == 0)
			return;
		var selectedCube = cubeList[Random.Range (0, cubeList.Length - 1)];
	    if (selectedCube != null)
            selectedCube.Select (false);
	    if (CheckWinState ())
			GameOver();
	}
	
	public bool CheckWinState ()
	{
		var cubeList = GetCubeList (false) .ToArray();
		if (cubeList.Length == 0)
			return false;
	    var playerCubes = GetCubeList(false).Where(cc => !cc.Availible && cc.IsPlayer).ToArray();
        var aiCubes = GetCubeList(false).Where(cc => !cc.Availible && !cc.IsPlayer).ToArray();
        var win = false;
		if (CheckCoins (playerCubes))
		{
			ResultText.text = "You Win!";
			win = true;
		}
		else if (CheckCoins (aiCubes))
		{
			ResultText.text = "You lose";
			win = true;
		}
		else if (playerCubes.Length + aiCubes.Length == cubeList.Length)
		{
			ResultText.text = "Draw.";
			GameOver ();
		}
		return win;
	}

    static bool CheckCoins(CubeControl[] cubsList)
	{
		int count;
        const double tolerance = 0.1f;
        var cubes = cubsList;
        for (var row = -1; row <= 1; ++row)
		{
			count = 0;
			for (var col = -1; col <= 1; ++col)
			    count += cubes.Select(cube => cube.GetCubePos()).Count(pos => Math.Abs(pos.x - col) < tolerance && Math.Abs(pos.y - row) < tolerance);
		    if (count != 3) continue;
		    Debug.Log("Row " + row + " win");
		    return true;
		}
		for (var col = -1; col <= 1; ++col)
		{
			count = 0;
			for (var row = -1; row <= 1; ++row)
			    count += cubes.Select(cube => cube.GetCubePos()).Count(pos => Math.Abs(pos.x - col) < tolerance && Math.Abs(pos.y - row) < tolerance);
		    if (count != 3) continue;
		    Debug.Log("Col " + col + " win");
		    return true;
		}
		count = 0;
		for (var i = -1; i <= 1; ++i)
		    count += cubes.Select(cube => cube.GetCubePos()).Count(pos => Math.Abs(pos.x - i) < tolerance && Math.Abs(pos.y - i) < tolerance);
		if (count == 3)
		{
			Debug.Log("/ win");
			return true;
		}
		count = 0;
		for (var i = -1; i <= 1; ++i)
		    count += cubes.Select(cube => cube.GetCubePos()).Count(pos => Math.Abs(pos.x - i) < tolerance && Math.Abs(pos.y - (-i)) < tolerance);
        if (count != 3) return false;
        Debug.Log("\\ win");
        return true;
	}

	public void GameOver()
	{
		IsGameOver = true;
		MainWindow.enabled = true;
		AIfirstMoveToggle.enabled = true;
	}

    public void RestartGame()
	{
		_firstRun = true;
		MainWindow.enabled = false;
		foreach(var cube in GetCubeList (false))
			cube.Reset();
		IsGameOver = false;
		IsAIfirstMove = AIfirstMoveToggle.GetComponentInChildren<Toggle> ().isOn;
		AIfirstMoveToggle.enabled = false;
	}

    private IEnumerable<CubeControl> GetCubeList(bool onlyAvalible)
	{
        return Cubes.OfType<GameObject>().Select(cube => cube.GetComponent<CubeControl>())
            .Where(cc => cc != null && (cc.Availible || !onlyAvalible));
	}

	public void SetAIfirstMove(bool par)
	{
		IsAIfirstMove = par;
	}
}
