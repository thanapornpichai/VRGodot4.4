using System.Collections.Generic;
using Godot;
using System.Threading.Tasks;

public partial class ElevatorController : Node
{
	private const string FLOOR_SCENE_PATH = "res://Scenes/Floors/";

	[Export]
	public NodePath FloorContainerPath;
	public List<PackedScene> SceneList = new List<PackedScene>();
	
	private Node _floorContainer;
	private Node _currentFloor = null;


	public override void _Ready()
	{
		LoadScenesFromFloder(FLOOR_SCENE_PATH);
		_floorContainer = GetNode(FloorContainerPath);
		
		// if (SceneList.Count > 0)
		// {
		// 	_ = LoadFloorAsync(SceneList[2]);
		// }
		// else
		// {
		// 	GD.PrintErr("SceneList is empty!");
		// }
	}

	private void LoadScenesFromFloder(string folderPath)
	{
		GD.Print(folderPath);
		DirAccess dir = DirAccess.Open(folderPath);
		
		if (dir == null)
		{
			GD.PrintErr("Cannot open folder: " + folderPath);
			return;
		}
		List<string> fileNames = new List<string>();

		dir.ListDirBegin();
		string fileName = dir.GetNext();
		while (!string.IsNullOrEmpty(fileName))
		{
			if (!dir.CurrentIsDir() && fileName.EndsWith(".tscn"))
			{
				fileNames.Add(fileName);
			}
			fileName = dir.GetNext();
		}
		dir.ListDirEnd();
		
		fileNames.Sort();

		foreach (string file in fileNames)
		{
			string fullPath = folderPath + file;
			PackedScene scene = ResourceLoader.Load<PackedScene>(fullPath);
			if (scene != null)
			{
				SceneList.Add(scene);
				GD.Print("Load scene: " + fullPath);
			}
			else
			{
				GD.PrintErr("Cannot load scene: " + fullPath);
			}
		}
	}

	public async Task LoadFloorAsync(PackedScene scene)
	{
		UnloadCurrentFloor();
		
		_currentFloor = scene.Instantiate();
		_floorContainer.AddChild(_currentFloor);
		
		await Task.CompletedTask;
	}
	
	private void UnloadCurrentFloor()
	{
		if (_currentFloor != null)
		{
			_currentFloor.GetParent()?.RemoveChild(_currentFloor);
			_currentFloor.QueueFree();
			_currentFloor = null;
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (SceneList.Count > 1)
		{
			if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
			{
				if (keyEvent.Keycode == Key.Key1)
				{
					GD.Print("Load Floor1");
					_ = LoadFloorAsync(SceneList[0]);
					
				}
				else if (keyEvent.Keycode == Key.Key2)
				{
					GD.Print("Load Floor2");
					_ = LoadFloorAsync(SceneList[1]);
				}
				else if (keyEvent.Keycode == Key.Key3)
				{
					GD.Print("Load Floor3");
					_ = LoadFloorAsync(SceneList[2]);
				}
			}
		}
	}
}
