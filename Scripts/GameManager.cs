using System.Collections.Generic;
using Godot;
using System.Threading.Tasks;

public enum ElevatorState
{
	Idle,
	Open,
	Close
}

public partial class GameManager : Node
{
	private const string FLOOR_SCENE_PATH = "res://Scenes/Floors/";
	private AnimationPlayer _animationPlayer;
	private Node _floorContainer;
	private Node _currentFloor = null;
	private bool _isOpen = false;

	[Export]
	public NodePath FloorContainerPath;
	public List<PackedScene> SceneList = new List<PackedScene>();
	[Export]
	public Node Elevator;
	public static GameManager Instance { get; private set; }

	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
			GD.Print("GameManager Loaded");
		}
		else
		{
			QueueFree();
		}
		
		LoadScenesFromFloder(FLOOR_SCENE_PATH);
		_floorContainer = GetNode(FloorContainerPath);
		
		_animationPlayer = Elevator.GetNode<AnimationPlayer>("AnimationPlayer");
		SetElevatorState(ElevatorState.Open);
		_isOpen = true;
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

	public async Task LoadScene(PackedScene scene)
	{
		if (_isOpen)
		{
			await DelayLoadScene();
		}

		if (!_isOpen)
		{
			await LoadFloorAsync(scene);
		}
	}
	
	private async Task DelayLoadScene()
	{
		SetElevatorState(ElevatorState.Close);
		var animationLength = _animationPlayer.GetAnimation("Close");
		await Task.Delay((int)(animationLength.Length * 1000));
		_isOpen = false;
	}

	private void SetElevatorState(ElevatorState state)
	{
		if (state == ElevatorState.Idle)
		{
			_animationPlayer.Play("Idle");
		}
		if (state == ElevatorState.Open)
		{
			_animationPlayer.Play("Open");
			_isOpen = true;
		}
		if (state == ElevatorState.Close)
		{
			_animationPlayer.Play("Close");
		}
	}

	public async Task LoadFloorAsync(PackedScene scene)
	{
		UnloadCurrentFloor();
		
		_currentFloor = scene.Instantiate();
		_floorContainer.AddChild(_currentFloor);
		await Task.Delay(3000);
		SetElevatorState(ElevatorState.Open);
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
					_ = LoadScene(SceneList[0]);
				}
				else if (keyEvent.Keycode == Key.Key2)
				{
					GD.Print("Load Floor2");
					_ = LoadScene(SceneList[1]);
				}
				else if (keyEvent.Keycode == Key.Key3)
				{
					GD.Print("Load Floor3");
					_ = LoadScene(SceneList[2]);
				}
				else if (keyEvent.Keycode == Key.Key4)
				{
					GD.Print("Open Elevator");
					SetElevatorState(ElevatorState.Open);
				}
				else if (keyEvent.Keycode == Key.Key5)
				{
					GD.Print("Close Elevator");
					SetElevatorState(ElevatorState.Close);
				}
			}
		}
	}
}
