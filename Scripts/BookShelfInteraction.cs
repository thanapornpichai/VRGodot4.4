using Godot;
using System;

public partial class BookShelfInteraction : Node
{
	[Export] public Node3D BookNode;
	[Export] public Area3D area;
	[Export] public AnimationPlayer _animPlayer;
	[Export] public AudioStreamPlayer3D InsertSound;

	private bool _triggered = false;

	public override void _Ready()
	{
		if (area != null)
			area.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (_triggered)
			return;

		if (body.IsInGroup("PlayerHand"))
		{
			GD.Print("📚 Book inserted into shelf.");
			BookNode.Visible = true;
			InsertSound?.Play();
			_animPlayer?.Play("BookShelfInteraction");
			_triggered = true;
		}
	}
}
