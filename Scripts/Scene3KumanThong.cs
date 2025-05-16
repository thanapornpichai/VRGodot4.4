using Godot;
using System.Threading.Tasks;

public partial class Scene3KumanThong : BaseFloorController
{
	[Export]
	public Area3D area;
	[Export] public Node3D offerNode3D;
	[Export] public MeshInstance3D dialogue;
	private TextMesh dialogueText;

	private bool _triggered = false;

	public override void _Ready()
	{
		SetProcess(true);
		dialogueText = dialogue.Mesh as TextMesh;
		
		area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			if (!_triggered)
			{
				offerNode3D.Visible = false;
				body.Visible = false;
				_triggered = true;
				GD.Print("Player give offer!");
				ShowHintDelay();
			}
		}
	}
	
	private async void ShowHintDelay()
	{
		dialogueText.Text = string.Empty;
		dialogueText.Text = "Good....";
		GD.Print("Waiting 3 seconds...");
		await ToSignal(GetTree().CreateTimer(3), "timeout");
		dialogueText.Text = "Go down to the third floor";
		GD.Print("Show hint!");
		Scene3FloorController.Instance.OnFinishFloor();
	}
}
