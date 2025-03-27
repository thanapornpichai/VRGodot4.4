using Godot;
using System;

public partial class Scene2FloorController : BaseFloorController
{
	private Area3D _area;
	private double _timer = 0f;
	private bool _playerInside = false;
	private bool _isTrigger = false;
	private bool _finishedFloor = false;
	
	private bool _enteredStorageRoom = false;
	public static Scene2FloorController Instance { get; private set; }
	
	public override void _Ready()
	{
		_area = null;
		if (Instance == null)
		{
			Instance = this;
			GD.Print("FloorController2 Loaded");
		}
		else
		{
			QueueFree();
		}
		SetProcess(true);
	}

	public void OnFinishFloor()
	{
		_area = GetNode<Area3D>("Area3D");

		_area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		_area.Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}
	
	public void OnEnterStorageRoom()
	{
		_enteredStorageRoom = true;
		OnFinishFloor();
	}
	
	public override void LoadFloor(int floorNumber)
	{
		base.LoadFloor(floorNumber);
	}
	
	public override void _Process(double delta)
	{
		if (_finishedFloor)
		{
			if (_playerInside)
			{
				_timer += delta;

				if (_timer >= 2.0f)
				{
					if (!_isTrigger)
					{
						this.LoadFloor(2);
						_isTrigger = true;
					}
					_timer = 2;
				}
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
