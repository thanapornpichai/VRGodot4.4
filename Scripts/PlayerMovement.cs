using Godot;
using System;

public partial class PlayerMovement : CharacterBody3D
{
	[Export] public float Speed = 5.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 direction = Vector3.Zero;

		if (Input.IsActionPressed("move_forward"))
			direction -= Transform.Basis.Z;
		if (Input.IsActionPressed("move_backward"))
			direction += Transform.Basis.Z;
		if (Input.IsActionPressed("move_left"))
			direction -= Transform.Basis.X;
		if (Input.IsActionPressed("move_right"))
			direction += Transform.Basis.X;

		direction = direction.Normalized();
		Velocity = direction * Speed;

		MoveAndSlide();
	}
}
