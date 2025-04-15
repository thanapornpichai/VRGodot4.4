using Godot;
using Godot.Collections;


public partial class BaseFloorController : Node3D
{
    public virtual void LoadFloor(int floorNumber)
    {
        GameManager.Instance.LoadScene(GameManager.Instance.SceneList[floorNumber]);
    }
}
