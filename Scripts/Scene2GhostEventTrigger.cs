using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public partial class Scene2GhostEventTrigger : BaseFloorController
{
	[Export] public Area3D _area;
	[Export] public double MaxTimer = 10.0;
	[Export] public Node3D ghostNode3D;
	[Export] public Node3D floorCheck;
	[Export] public AnimationPlayer _animPlayer;
	[Export] public Array<Node3D> _nodeArray = new();
	[Export] public Array<OmniLight3D> FlickerLights = new();
	[Export] public AudioStreamPlayer3D GhostSound;

	private bool _triggered = false;
	private double _timer = 0;

	public override void _Ready()
	{
		ghostNode3D.Visible = false;
		_area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		SetProcess(true);
	}

	public override void _Process(double delta)
	{
		if (_triggered)
		{
			_timer += delta;
			if (_timer >= MaxTimer)
			{
				ghostNode3D.Visible = false;
				_triggered = false;
				SetProcess(false);
				ActiveNodeArray(true);
				RestoreLights();
			}
		}
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player") && !_triggered)
		{
			GD.Print("👻 Ghost triggered!");
			_triggered = true;
			_timer = 0;

			ActiveNodeArray(false);
			ghostNode3D.Visible = true;
			_animPlayer?.Play("NungRumHead");

			GhostSound?.Play();
			FlickerLightsCrazy();

			_area.Visible = false;
			_area.Monitoring = false;

			floorCheck.Visible = true;
			floorCheck.GetNode<Area3D>("Area3D").Monitoring = true;
		}
	}

	private async void FlickerLightsCrazy()
	{
		foreach (var light in FlickerLights)
			light.LightColor = new Color(1, 0, 0); // Red

		for (int i = 0; i < 12; i++)
		{
			foreach (var light in FlickerLights)
				light.Visible = !light.Visible;

			await ToSignal(GetTree().CreateTimer(0.04f + (float)GD.RandRange(0.01, 0.03)), "timeout");
		}

		foreach (var light in FlickerLights)
			light.Visible = true;
	}

	private void RestoreLights()
	{
		foreach (var light in FlickerLights)
		{
			light.Visible = true;
			light.LightColor = new Color(1, 1, 1); // back to normal white
		}
	}

	public void ActiveNodeArray(bool active)
	{
		for (int i = 0; i < _nodeArray.Count; i++)
			_nodeArray[i].Visible = active;
	}
}
