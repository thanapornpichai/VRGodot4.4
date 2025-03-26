using Godot;
using System;

public partial class FloorController1 : BaseFloorController
{
	private Area3D _area;
	private float _timer = 0f;
	private bool _playerInside = false;
	
	public override void _Ready()
	{
		_area = GetNode<Area3D>("Area3D");

		_area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		_area.Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}
	public override void LoadFloor(int floorNumber)
	{
		base.LoadFloor(floorNumber);
	}
	
	public override void _Process(float delta)
	{
		GD.Print(_timer);
		GD.Print(_playerInside);

		if (_playerInside)
		{
			_timer += delta;

			if (_timer >= 2.0f)
			{
				this.LoadFloor(2);
				_timer = 0;
			}
		}

	   
	}
	
	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInside = true;
			GD.Print("Player entered the floor area!");
		}
	}

	private void OnBodyExited(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInside = false;
			_timer = 0; // Reset timer when player leaves
			GD.Print("Player left the floor area.");
		}
	}
}
