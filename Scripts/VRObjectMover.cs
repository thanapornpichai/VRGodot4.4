
using Godot;
using System;

public partial class VRObjectMover : Area3D
{
	[Export] public Node3D MovableLeft;
	[Export] public Node3D MovableRight;
	[Export] public float MoveStep = 0.1f;
	[Export] public float SnapDistance = 0.1f;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("hand"))
		{
			GD.Print("VR Button Pressed by Hand!");
			MoveObjects();
			TrySnapObjects();
		}
	}

	private void MoveObjects()
	{
		MovableLeft.Translate(Vector3.Right * MoveStep);
		MovableRight.Translate(Vector3.Left * MoveStep);
	}

	private void TrySnapObjects()
	{
		float distance = MovableLeft.GlobalPosition.DistanceTo(MovableRight.GlobalPosition);
		if (distance <= SnapDistance)
		{
			Vector3 center = (MovableLeft.GlobalPosition + MovableRight.GlobalPosition) / 2;
			MovableLeft.GlobalPosition = center;
			MovableRight.GlobalPosition = center;
		}
	}
}
