using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public partial class Scene2GhostEventTrigger : BaseFloorController
{
	[Export]
	public Area3D _area;

	private bool _triggered = false;
	private double _timer = 0;

	[Export] public double MaxTimer = 0;
	[Export] public Node3D ghostNode3D;
	[Export] public Node3D floorCheck;
	[Export] public AnimationPlayer _animPlayer;
	[Export] public Array<Node3D> _nodeArray = new Array<Node3D>();
	
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
			GD.Print(_timer);
			if (_timer >= MaxTimer)
			{
				ghostNode3D.Visible = false;
				_triggered = false;
				SetProcess(false);
				ActiveNodeArray(true);
			}
		}
	}
	
	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			if (!_triggered)
			{
				ActiveNodeArray(false);
				ghostNode3D.Visible = true;
				if (_animPlayer != null)
					_animPlayer.Play("NungRumHead");
				var ghostArea3D = this.GetNode<Area3D>("Area3D");
				ghostArea3D.Visible = false;
				ghostArea3D.Monitoring = false;
				
				floorCheck.Visible = true;
				floorCheck.GetNode<Area3D>("Area3D").Monitoring = true;
				
				_triggered = true;
			}
			GD.Print("Player entered event area!");
		}
	}

	public void ActiveNodeArray(bool active)
	{
		for (int i = 0; i < _nodeArray.Count; i++)
		{
			_nodeArray[i].Visible = active;
		}
	}
}
