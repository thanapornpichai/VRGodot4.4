using Godot;

public partial class SafeBox : Node3D
{
    [Export] public Node3D CapPivot;
    [Export] public Node3D book;
    [Export] public AnimationPlayer _animPlayer;
    
    private bool _isOpen = false;

    public override void _Ready()
    {
        AddToGroup("safe_boxes");
    }

    public void OpenBox()
    {
        if (_isOpen)
            return;

        _isOpen = true;
        GD.Print("SafeBox opened!");
        book.Visible = true;

        if (_animPlayer != null)
            _animPlayer.Play("LockerOpen");
    }
}