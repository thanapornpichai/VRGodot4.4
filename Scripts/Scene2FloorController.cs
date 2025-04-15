using Godot;
using System;

public partial class Scene2FloorController : BaseFloorController
{
	private double _timer = 0f;
	private bool _isTrigger = false;
	private bool _finishedFloor = false;
	
	private bool _enteredStorageRoom = false;
	public static Scene2FloorController Instance { get; private set; }
	
	public override void _Ready()
	{
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
		_finishedFloor = true;
	}
	
	public override void _Process(double delta)
	{
		if (_finishedFloor)
		{
			if (!_isTrigger)
			{
				_timer += delta;

				if (_timer >= 2.0f)
				{

					this.LoadFloor(2);
					_isTrigger = true;
					_timer = 2;
				}
			}
		}
	}
}
