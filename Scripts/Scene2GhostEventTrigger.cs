using Godot;
using System;

public partial class Scene2GhostEventTrigger : BaseFloorController
{
	private Area3D _area;
	private Node3D _ghostNode3D;
	private bool _triggered = false;
	private double _timer = 0;

	[Export] public double MaxTimer = 0;
	
	public override void _Ready()
	{
		_ghostNode3D.Visible = false;
		SetProcess(true);
	}

	public override void _Process(double delta)
	{
		if (_triggered)
		{
			_timer += delta;
			if (_timer >= MaxTimer)
			{
				_ghostNode3D.Visible = false;
				SetProcess(false);
			}
		}
	}
	
	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			if (!_triggered)
			{
				_ghostNode3D.Visible = true;
			}
			GD.Print("Player entered the Storage room!");
		}
	}
}
