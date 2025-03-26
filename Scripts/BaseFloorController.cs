using Godot;
using System;

public partial class BaseFloorController : Node
{
    public virtual void LoadFloor(int floorNumber)
    {
        GameManager.Instance.LoadScene(GameManager.Instance.SceneList[floorNumber-1]);
    }
}
