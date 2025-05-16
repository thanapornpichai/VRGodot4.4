using Godot;
using System;

public partial class BookShelfInteraction : Node
{
	[Export] public Node3D BookNode;
	[Export] public Area3D area;
	[Export] public AnimationPlayer _animPlayer;

	public override void _Ready()
	{
		area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("PlayerHand"))
		{
			GD.Print("Insert book");
			BookNode.Visible = true;
			
			if (_animPlayer != null)
				_animPlayer.Play("BookShelfInteration");
		}
	}
}
