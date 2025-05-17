using Godot;
using System;
using System.Collections.Generic;

public partial class Scene4LaserPuzzle : Node
{
	private List<SafeBox> _safeBoxes = new List<SafeBox>();
	private bool _isSolved = false;

	public override void _Ready()
	{
		// Find all SafeBox nodes in group
		foreach (Node node in GetTree().GetNodesInGroup("safe_boxes"))
		{
			if (node is SafeBox box)
			{
				_safeBoxes.Add(box);
			}
		}
	}

	public override void _Process(double delta)
	{
		if (_isSolved) return;

		bool allLasersHit = true;

		foreach (var box in _safeBoxes)
		{
			if (!IsRayValid(box.rayCastlaser1) || !IsRayValid(box.rayCastlaser2))
			{
				allLasersHit = false;
				break;
			}
		}

		if (allLasersHit)
		{
			GD.Print("🔓 All lasers aligned. Opening boxes...");
			foreach (var box in _safeBoxes)
			{
				box.OpenBox();
			}
			_isSolved = true;
		}
	}

	private bool IsRayValid(RayCast3D ray)
	{
		return ray.IsColliding();
	}
}
