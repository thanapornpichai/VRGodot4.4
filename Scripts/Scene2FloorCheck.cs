using Godot;

public partial class Scene2FloorCheck : Node3D
{
	[Export]
	public Area3D _area;
	
	public override void _Ready()
	{
		_area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}
	
	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			GD.Print("Player Clear Scene 2!");
			Scene2FloorController.Instance.OnFinishFloor();
		}
	}
}
