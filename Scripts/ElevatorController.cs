using Godot;
using System.Threading.Tasks;

public partial class ElevatorController : Node
{
	// Path ของ Scene ชั้น (ปรับตามที่ต้องการ)
	private const string FloorScenePath = "res://Scenes/Floors/Floor1.tscn";
	
	// Node ที่จะเก็บ Scene ชั้น (กำหนดผ่าน Inspector)
	[Export]
	public NodePath FloorContainerPath;
	
	private Node FloorContainer;

	public override void _Ready()
	{
		FloorContainer = GetNode(FloorContainerPath);
		// เริ่มโหลดชั้นแรกแบบ asynchronous (จำลองด้วย Task.Run)
		_ = LoadFloorAsync(FloorScenePath);
	}

	public async Task LoadFloorAsync(string scenePath)
	{
		// ใช้ Task.Run เพื่อทำการโหลดแบบ synchronous ใน background thread
		PackedScene floorScene = await Task.Run(() =>
		{
			return (PackedScene)ResourceLoader.Load(scenePath);
		});

		if (floorScene != null)
		{
			// สร้าง Instance ของ Scene และเพิ่มเข้าไปใน FloorContainer (บน Main Thread)
			Node floorInstance = floorScene.Instantiate();
			FloorContainer.AddChild(floorInstance);
		}
		else
		{
			GD.PrintErr("ไม่สามารถโหลด Scene ได้: " + scenePath);
		}
	}
}
