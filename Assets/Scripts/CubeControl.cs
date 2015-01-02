using UnityEngine;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once CheckNamespace
public class CubeControl : MonoBehaviour
{
    // ReSharper disable once UnassignedField.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public Light CubeLight;
    public bool IsPlayer;
    public bool Availible = true;
    private GameConroller _gameCon;

    // ReSharper disable once UnusedMember.Local
    private void Start()
    {
        _gameCon = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameConroller>();
    }

    // ReSharper disable once UnusedMember.Local
    private void Update()
    {
        if (!Availible)
            transform.Rotate(new Vector3(0, 50, 0)*Time.deltaTime);
    }

    // ReSharper disable once UnusedMember.Local
    private void OnMouseDown()
    {
        if (_gameCon.IsGameOver)
            return;
        if (_gameCon == null || !Availible)
            return;
        Select(true);
        if (_gameCon.CheckWinState())
        {
            _gameCon.GameOver();
            return;
        }
        _gameCon.MakeAIturn();
    }

    public Vector2 GetCubePos()
    {
        var x = 0f;
        var y = 0f;
        if (transform.position.x > 0)
            x = 1;
        else if (transform.position.x < 0)
            x = -1;
        if (transform.position.z > 0)
            y = 1;
        else if (transform.position.z < 0)
            y = -1;
        return new Vector2(x, y);
    }

    public void Select(bool player)
    {
        renderer.enabled = true;
        Availible = false;
        IsPlayer = player;
        CubeLight.color = IsPlayer ? _gameCon.PlayerColor : _gameCon.AIcolor;
        renderer.material.CopyPropertiesFromMaterial(
            _gameCon.MatList[(_gameCon.IsAIfirstMove && player || !_gameCon.IsAIfirstMove && !player) ? 1 : 0]);
    }

    // ReSharper disable once UnusedMember.Local
    public void Reset()
    {
        renderer.enabled = false;
        Availible = true;
        CubeLight.color = Color.white;
    }
}