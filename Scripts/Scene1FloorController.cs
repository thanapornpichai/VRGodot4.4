using Godot;
using System;

public partial class Scene1FloorController : BaseFloorController
{
	private Area3D _area;
	private double _timer = 0f;
	private bool _playerInside = false;
	private bool _isTrigger = false;

	// Audio nodes
	private AudioStreamPlayer _enterSound;
	private AudioStreamPlayer _ambientSound;

	// Fade control
	private bool _isFadingOut = false;
	private float _fadeSpeedDbPerSecond = 30f;

	public override void _Ready()
	{
		_area = GetNode<Area3D>("Area3D");
		_enterSound = GetNode<AudioStreamPlayer>("EnterSound");
		_ambientSound = GetNode<AudioStreamPlayer>("AmbientSound");

		// AmbientSound should already be playing via Autoplay and Loop in the editor

		_area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		_area.Connect("body_exited", new Callable(this, nameof(OnBodyExited)));

		SetProcess(true);
	}

	public override void LoadFloor(int floorNumber)
	{
		base.LoadFloor(floorNumber);
	}

	public override void _Process(double delta)
	{
		if (_playerInside)
		{
			_timer += delta;

			if (_timer >= 2.0f && !_isTrigger)
			{
				// Trigger fade-out only once
				_isFadingOut = true;
				_isTrigger = true;
			}
		}

		if (_isFadingOut && _ambientSound != null)
		{
			_ambientSound.VolumeDb -= (float)(delta * _fadeSpeedDbPerSecond);

			if (_ambientSound.VolumeDb <= -80f)
			{
				_ambientSound.Stop();
				_isFadingOut = false;
				LoadFloor(1);
			}
		}
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInside = true;
			_timer = 0;
			_isTrigger = false;

			GD.Print("Player entered the floor area!");

			if (_enterSound != null)
				_enterSound.Play();
		}
	}

	private void OnBodyExited(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInside = false;
			_timer = 0;
			_isTrigger = false;
			_isFadingOut = false;

			GD.Print("Player left the floor area.");
		}
	}
}
