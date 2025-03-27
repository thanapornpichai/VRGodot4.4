using Godot;
using System;

public partial class BaseFloorController : Node
{
    public virtual void LoadFloor(int floorNumber)
    {
        GameManager.Instance.LoadScene(GameManager.Instance.SceneList[floorNumber-1]);
    }
    
    public override void _Process(double delta)
    {
        GD.Print("MyNode _Process");
    }
}
