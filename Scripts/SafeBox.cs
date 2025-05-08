using Godot;

public partial class SafeBox : MeshInstance3D
{
    [Export] public Node3D CapPivot;
    private AnimationPlayer _animPlayer;

    private bool _isOpen = false;

    public override void _Ready()
    {
        AddToGroup("safe_boxes");
        _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void OpenBox()
    {
        if (_isOpen)
            return;

        _isOpen = true;
        GD.Print("SafeBox opened!");

        if (_animPlayer != null)
            _animPlayer.Play("LockerOpen");
    }
}