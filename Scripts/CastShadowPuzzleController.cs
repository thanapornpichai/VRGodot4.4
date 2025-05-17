using Godot;
using System;
using System.Collections.Generic;

public partial class CastShadowPuzzleController : Node3D
{
	[Export] public Node3D PuzzleObject;
	[Export] public float RotationSpeed = 90f; // degrees per second

	[Export] public Area3D ButtonLeft;
	[Export] public Area3D ButtonRight;
	[Export] public Area3D ButtonUp;
	[Export] public Area3D ButtonDown;

	private HashSet<string> activeDirections = new HashSet<string>();

	private AudioStreamPlayer3D RotateStartSound;
	private AudioStreamPlayer3D RotateLoopSound;

	private bool wasRotatingLastFrame = false;
	private bool puzzleSolved = false;

	public override void _Ready()
	{
		// Button input connections
		ButtonLeft.BodyEntered += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Add("L"); };
		ButtonLeft.BodyExited += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Remove("L"); };

		ButtonRight.BodyEntered += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Add("R"); };
		ButtonRight.BodyExited += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Remove("R"); };

		ButtonUp.BodyEntered += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Add("U"); };
		ButtonUp.BodyExited += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Remove("U"); };

		ButtonDown.BodyEntered += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Add("D"); };
		ButtonDown.BodyExited += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Remove("D"); };

		// Find sounds
		RotateStartSound = GetNodeOrNull<AudioStreamPlayer3D>("RotateStartSound");
		RotateLoopSound = GetNodeOrNull<AudioStreamPlayer3D>("RotateLoopSound");

		if (RotateStartSound == null)
			GD.PrintErr("❌ RotateStartSound not found.");
		if (RotateLoopSound == null)
			GD.PrintErr("❌ RotateLoopSound not found.");
	}

	public override void _Process(double delta)
	{
		float step = RotationSpeed * (float)delta;
		bool isRotating = activeDirections.Count > 0;

		foreach (var dir in activeDirections)
		{
			switch (dir)
			{
				case "L":
					PuzzleObject.RotateY(Mathf.DegToRad(step));
					break;
				case "R":
					PuzzleObject.RotateY(Mathf.DegToRad(-step));
					break;
				case "U":
					PuzzleObject.RotateX(Mathf.DegToRad(step));
					break;
				case "D":
					PuzzleObject.RotateX(Mathf.DegToRad(-step));
					break;
			}
		}

		// Audio feedback
		if (isRotating && !wasRotatingLastFrame)
		{
			RotateStartSound?.Play();
			RotateLoopSound?.Play();
		}
		else if (!isRotating && wasRotatingLastFrame)
		{
			RotateLoopSound?.Stop();
		}
		wasRotatingLastFrame = isRotating;

		// Optional win condition
		if (!puzzleSolved)
			CheckSolved();
	}

	private void CheckSolved()
	{
		float yRot = Mathf.RadToDeg(PuzzleObject.Rotation.Y);
		float normalizedY = Mathf.Wrap(yRot, -180f, 180f);

		if (Mathf.Abs(normalizedY) <= 5f)
		{
			GD.Print("🎉 Puzzle Solved! Perfect alignment.");
			puzzleSolved = true;
			RotateLoopSound?.Stop();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.F7)
		{
			PuzzleObject.Rotation = Vector3.Zero;
			GD.Print("🧪 Debug: PuzzleObject rotation reset.");
		}
	}
}
