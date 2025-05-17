using Godot;
using System;

public partial class CastShadow : Node3D
{
	[Export] private Node3D Player;
	[Export] private Area3D ButtonLeft;
	[Export] private Area3D ButtonRight;
	[Export] private Area3D Object1;
	[Export] private Area3D Object2;

	private AudioStreamPlayer3D MoveSound;
	private AudioStreamPlayer3D WinSound;

	private bool _isSolved = false;

	public override void _Ready()
	{
		ButtonLeft.BodyEntered += OnButtonLeftEntered;
		ButtonRight.BodyEntered += OnButtonRightEntered;

		// Auto-find Audio players by name
		MoveSound = GetNodeOrNull<AudioStreamPlayer3D>("MoveSound");
		WinSound = GetNodeOrNull<AudioStreamPlayer3D>("WinSound");

		if (MoveSound == null)
			GD.PrintErr("❌ MoveSound not found in CastShadow node.");
		if (WinSound == null)
			GD.PrintErr("❌ WinSound not found in CastShadow node.");
	}

	private void OnButtonLeftEntered(Node3D body)
	{
		if (_isSolved || body != Player) return;

		Object1.Position += new Vector3(1, 0, 0);
		MoveSound?.Play();
		CheckWin();
	}

	private void OnButtonRightEntered(Node3D body)
	{
		if (_isSolved || body != Player) return;

		Object1.Position += new Vector3(-1, 0, 0);
		MoveSound?.Play();
		CheckWin();
	}

	private void CheckWin()
	{
		var overlapping = Object1.GetOverlappingBodies();
		foreach (var body in overlapping)
		{
			if (body == Object2)
			{
				if (!_isSolved)
				{
					GD.Print("🎉 Puzzle Complete!");
					WinSound?.Play();
					_isSolved = true;
				}
				return;
			}
		}

		GD.Print("❌ Puzzle not complete. Keep adjusting.");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.F7)
		{
			GD.Print("🧪 Debug: Force Win Triggered");
			WinSound?.Play();
			_isSolved = true;
		}
	}
}
