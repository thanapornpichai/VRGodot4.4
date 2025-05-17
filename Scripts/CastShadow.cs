using Godot;
using System;

public partial class CastShadow : Node3D
{
	[Export] private Node3D Player;
	[Export] private Area3D ButtonLeft;
	[Export] private Area3D ButtonRight;
	[Export] private Area3D Object1;
	[Export] private Area3D Object2;

	public override void _Ready()
	{
		ButtonLeft.BodyEntered += OnButtonLeftEntered;
		ButtonRight.BodyEntered += OnButtonRightEntered;
	}

	private void OnButtonLeftEntered(Node3D body)
	{
		if (body == Player)
		{
			Object1.Position += new Vector3(1, 0, 0);
			CheckWin();
		}
	}

	private void OnButtonRightEntered(Node3D body)
	{
		if (body == Player)
		{
			Object1.Position += new Vector3(-1, 0, 0); // ขยับไปซ้าย
			CheckWin();
		}
	}

	private void CheckWin()
	{
	var overlapping = Object1.GetOverlappingBodies();
	foreach (var body in overlapping)
	{
		if (body == Object2)
		{
			GD.Print("Win!");
		}
	}
	}

}
