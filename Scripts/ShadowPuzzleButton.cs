using Godot;
using System;

public partial class ShadowPuzzleButton : Node
{
    [Export] public Area3D puzzleButton;
    [Export] public Node3D targetObject;
    [Export] public Vector3 directionDegrees = new Vector3(0, 90, 0);
    public override void _Ready()
    {
        puzzleButton.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        puzzleButton.Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
    }
    
    private void OnBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            Vector3 directionRadians = new Vector3(
                Mathf.DegToRad(directionDegrees.X),
                Mathf.DegToRad(directionDegrees.Y),
                Mathf.DegToRad(directionDegrees.Z)
            );

            targetObject.Rotation += directionRadians;
        }
    }
    
    private void OnBodyExited(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            
        }
    }
}
