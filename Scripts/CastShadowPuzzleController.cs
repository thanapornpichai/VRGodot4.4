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

    public override void _Ready()
    {
        ButtonLeft.BodyEntered += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Add("L"); };
        ButtonLeft.BodyExited += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Remove("L"); };

        ButtonRight.BodyEntered += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Add("R"); };
        ButtonRight.BodyExited += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Remove("R"); };

        ButtonUp.BodyEntered += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Add("U"); };
        ButtonUp.BodyExited += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Remove("U"); };

        ButtonDown.BodyEntered += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Add("D"); };
        ButtonDown.BodyExited += body => { if (body.IsInGroup("PlayerHand")) activeDirections.Remove("D"); };
    }

    public override void _Process(double delta)
    {
        float step = RotationSpeed * (float)delta;

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
    }
}
