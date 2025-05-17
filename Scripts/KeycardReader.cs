using Godot;

public partial class KeycardReader : Node3D
{
	[Export] public Area3D readerArea;
	[Export] public Node3D doorToOpen;

	// 🔊 Optional audio feedback
	[Export] public AudioStreamPlayer3D KeycardSound;
	[Export] public AudioStreamPlayer3D DoorOpenSound;

	private bool _isUnlocked = false;

	public override void _Ready()
	{
		if (readerArea != null)
			readerArea.BodyEntered += OnBodyEntered;

		SetProcessInput(true);
	}

	public override void _Input(InputEvent @event)
	{
		if (_isUnlocked) return;

		if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.F6)
		{
			GD.Print("🧪 DEBUG: Forced door unlock");
			_isUnlocked = true;
			PlaySoundsAndOpen();
		}
	}

	private void OnBodyEntered(Node3D body)
	{
		if (_isUnlocked)
			return;

		if (body.IsInGroup("KeyCard"))
		{
			GD.Print("✅ KeyCard detected! Door unlocked.");
			_isUnlocked = true;
			PlaySoundsAndOpen();
		}
	}

	private void PlaySoundsAndOpen()
	{
		KeycardSound?.Play();
		DoorOpenSound?.Play();
		OpenDoor();
	}

	private void OpenDoor()
	{
		if (doorToOpen != null)
		{
			var anim = doorToOpen.GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
			if (anim != null)
				anim.Play("OpenDoor");
			else
				GD.PrintErr("❌ AnimationPlayer not found on door node.");
		}
	}
}
