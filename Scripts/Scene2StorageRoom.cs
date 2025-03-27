using Godot;
using System;

public partial class Scene2StorageRoom : BaseFloorController
{
	private Area3D _area;
	
	public override void _Ready()
	{
		SetProcess(true);
	}

	public void OnFinishFloor()
	{
		_area = GetNode<Area3D>("Area3D");

		_area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}
	
	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			Scene2FloorController.Instance.OnEnterStorageRoom();
			GD.Print("Player entered the Storage room!");
		}
	}

}
