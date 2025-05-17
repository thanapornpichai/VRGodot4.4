using Godot;
using System.Threading.Tasks;

public partial class Scene3KumanThong : BaseFloorController
{
	[Export] public Area3D area;
	[Export] public Node3D offerNode3D;
	[Export] public MeshInstance3D dialogue;

	[Export] public AudioStreamPlayer3D OfferSound;
	[Export] public AudioStreamPlayer3D LaughSound;
	[Export] public AudioStreamPlayer3D ClueSound;

	private TextMesh dialogueText;
	private bool _triggered = false;

	public override void _Ready()
	{
		SetProcess(true);
		dialogueText = dialogue.Mesh as TextMesh;
		area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	public override void _Input(InputEvent @event)
	{
		if (_triggered) return;

		if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.F6)
		{
			GD.Print("🧪 DEBUG: F6 pressed, simulating offering event.");
			_triggered = true;
			SimulateOffering();
		}
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Offer") && !_triggered)
		{
			_triggered = true;
			offerNode3D.Visible = false;
			body.Visible = false;

			GD.Print("🎁 Offering accepted!");

			SimulateOffering();
		}
	}

	private void SimulateOffering()
	{
		OfferSound?.Play();
		LaughSound?.Play();
		ShowHintDelay();
	}

	private async void ShowHintDelay()
	{
		dialogueText.Text = string.Empty;

		ClueSound?.Play();
		dialogueText.Text = "Good....";
		await ToSignal(GetTree().CreateTimer(3), "timeout");

		ClueSound?.Play();
		dialogueText.Text =
			"Ahh! Thanks! Alright, read carefully.\n" +
			"That soda is giving me enough energy to communicate\n" +
			"to you from the outside.\n" +
			"I’m not actually even in here.";
		await ToSignal(GetTree().CreateTimer(5), "timeout");

		ClueSound?.Play();
		dialogueText.Text =
			"It looks like you’re trapped in some kind of\n" +
			"purgatory realm, and I think I know what’s\n" +
			"keeping you in here… ";
		await ToSignal(GetTree().CreateTimer(7), "timeout");

		ClueSound?.Play();
		dialogueText.Text =
			"I don’t know why she chose you,\n" +
			"but if you wanna get out of here,\n" +
			"you might have to confront her yourself.";
		await ToSignal(GetTree().CreateTimer(7), "timeout");

		ClueSound?.Play();
		dialogueText.Text =
			"But before you do that,\n" +
			"you’ll have to go inside the storage room,\n" +
			"there is something in there,\n" +
			"something very important to her.\n" +
			"It's the deepest part of her psyche,\n" +
			"so you’ll have to go step-by-step.";
		await ToSignal(GetTree().CreateTimer(7), "timeout");

		ClueSound?.Play();
		dialogueText.Text =
			"You’ll first have to go to the big room\n" +
			"at the end of the 3rd floor. There,\n" +
			"you will find a cryptex, the only\n" +
			"cryptex you can open! As for the password…\n" +
			"There might be clues in the other rooms\n" +
			"that are related to it. Good luck!";

		Scene3FloorController.Instance.OnFinishFloor();
	}
}
