using Godot;
using System.Threading.Tasks;

public partial class Scene3KumanThong : BaseFloorController
{
	[Export]
	public Area3D area;
	private Node3D _offerNode3D;
	private Node3D _hintNode3D;
	private bool _triggered = false;

	[Export] public double MaxTimer = 0;
	
	public override void _Ready()
	{
		_offerNode3D.Visible = false;
		SetProcess(true);
		
		area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Offer"))
		{
			if (!_triggered)
			{
				_offerNode3D.Visible = true;
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
		_hintNode3D.Visible = true;
		GD.Print("Show hint!");
		Scene3FloorController.Instance.OnFinishFloor();
	}
}
