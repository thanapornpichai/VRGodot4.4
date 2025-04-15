using Godot;
using System;

public partial class Scene2StorageRoom : BaseFloorController
{
	[Export]
	public Area3D _area;

	[Export] 
	public Node3D ghostEvent;
	public override void _Ready()
	{
		if (_area != null)
		{
			_area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
			GD.Print("Area3D assigned!");

		}
		else
		{
			GD.Print("Area3D not assigned!");
		}	
	}
	
	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			GD.Print("Player entered the Storage room!");
			var storageArea3D = this.GetNode<Area3D>("Area3D");
			storageArea3D.Visible = false;
			storageArea3D.Monitoring = false;
			this.Visible = false;
			ghostEvent.Visible = true;
			ghostEvent.GetNode<Area3D>("Area3D").Monitoring = true;
		}
	}
}
