using Godot;

public partial class KeycardReader : Node3D
{
    [Export] public Area3D readerArea;
    [Export] public Node3D doorToOpen;

    private bool _isUnlocked = false;

    public override void _Ready()
    {
        if (readerArea != null)
            readerArea.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (_isUnlocked)
            return;

        if (body.IsInGroup("KeyCard"))
        {
            GD.Print("KeyCard detected! Door unlocked.");
            _isUnlocked = true;

            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        if (doorToOpen != null)
        {
            var anim = doorToOpen.GetNode<AnimationPlayer>("AnimationPlayer");
            if (anim != null)
                anim.Play("OpenDoor");
        }
    }

}