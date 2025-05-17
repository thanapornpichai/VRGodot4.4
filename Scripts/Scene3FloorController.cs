using Godot;
using System;

public partial class Scene3FloorController : BaseFloorController
{
	[Export] public Area3D _area;
	[Export] public AudioStreamPlayer3D AreaEnteredSound;
	[Export] public AudioStreamPlayer3D FloorCompleteSound;

	private double _timer = 0f;
	private bool _playerInside = false;
	private bool _isTrigger = false;
	private bool _finishedFloor = false;
	private bool _enterSoundPlayed = false;

	public static Scene3FloorController Instance { get; private set; }

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
			GD.Print("FloorController3 Loaded");
		}
		else
		{
			QueueFree();
		}

		SetProcess(true);
	}

	public void OnFinishFloor()
	{
		GD.Print("OnFinishScene3");

		_area.Visible = true;
		_area.Monitoring = true;
		_finishedFloor = true;

		_area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		_area.Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}

	public override void LoadFloor(int floorNumber)
	{
		base.LoadFloor(floorNumber);
	}

	public override void _Process(double delta)
	{
		if (_finishedFloor && _playerInside)
		{
			_timer += delta;

			if (_timer >= 2.0f && !_isTrigger)
			{
				_isTrigger = true;
				GD.Print("✅ Scene 3 complete. Loading next floor...");

				FloorCompleteSound?.Play();
				LoadFloor(3);
			}
		}
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInside = true;
			_timer = 0;

			GD.Print("Player entered the floor area!");

			if (!_enterSoundPlayed && AreaEnteredSound != null)
			{
				AreaEnteredSound.Play();
				_enterSoundPlayed = true;
			}
		}
	}

	private void OnBodyExited(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInside = false;
			_timer = 0;
			GD.Print("Player left the floor area.");
		}
	}
}
