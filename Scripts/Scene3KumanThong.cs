using Godot;
using System.Threading.Tasks;

public partial class Scene3KumanThong : BaseFloorController
{
	[Export]
	public Area3D area;
	[Export] public Node3D offerNode3D;
	[Export] public Node3D hint1Node3D;
	[Export] public Node3D hint2Node3D;
	private bool _triggered = false;

	public override void _Ready()
	{
		SetProcess(true);
		
		area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			if (!_triggered)
			{
				offerNode3D.Visible = true;
				body.Visible = false;
				_triggered = true;
				GD.Print("Player give offer!");
				ShowHintDelay();
			}
		}
	}
	
	private async void ShowHintDelay()
	{
		GD.Print("Waiting 2 seconds...");
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		//hint1Node3D.Visible = false;
		//hint2Node3D.Visible = true;
		GD.Print("Show hint!");
		Scene3FloorController.Instance.OnFinishFloor();
	}
}
