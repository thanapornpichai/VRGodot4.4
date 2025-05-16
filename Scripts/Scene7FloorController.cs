using System.Threading.Tasks;
using Godot;

public partial class Scene7FloorController : BaseFloorController
{
	private Area3D _area;
	private bool _isTrigger = false;
	public static Scene7FloorController Instance { get; private set; }
	[Export] public PackedScene finalScene;
	
	public override void _Ready()
	{
		_area = null;
		if (Instance == null)
		{
			Instance = this;
			GD.Print("FloorController7 Loaded");
		}
		else
		{
			QueueFree();
		}
		SetProcess(true);
	}

	public void OnFinishFloor()
	{
		_area = GetNode<Area3D>("Area3D");
		_area.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}
	
	public override void LoadFloor(int floorNumber)
	{
		base.LoadFloor(floorNumber);
	}
	
	public async Task OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			await Task.Delay(3000);
			GetTree().ChangeSceneToPacked(finalScene);
		}
	}
}
