using Godot.Collections;
using Godot;

public partial class CryptexController : Node
{
	private const string PASS_CODE = "911";
	private int[] _codeNumbers = new int[3];
	
	[Export] public Array<PackedScene> NumberList = new Array<PackedScene>();

	[Export] public Node firstNumberNew;
	[Export] public Node firstNumberOld;
	[Export] public Node firstNumberNewPosition;
	[Export] public Node firstNumberOldPosition;

	public override void _Ready()
	{
		firstNumberNew = NumberList[0].Instantiate();
		firstNumberOld = firstNumberNew;
		
		AddChild(firstNumberNew);
		
		firstNumberNew = null;
	}

	public void SpawnNumber(int number)
	{
		firstNumberNew = NumberList[number - 1].Instantiate();
		
		firstNumberOld.QueueFree();
		firstNumberOld = null;
		
		firstNumberOld = firstNumberNew;
		AddChild(firstNumberNew);

		firstNumberNew = null;
	}
	
	public void OnIncreaseButtonPressed(int position)
	{
		ChangeNumber(position, 1);
	}

	public void OnDecreaseButtonPressed(int position)
	{
		ChangeNumber(position, -1);
	}
	
	private void ChangeNumber(int position, int change)
	{
		if (position >= 0 && position < _codeNumbers.Length)
		{
			_codeNumbers[position] += change;

			if (_codeNumbers[position] > 9) _codeNumbers[position] = 0;
			if (_codeNumbers[position] < 0) _codeNumbers[position] = 9;
		}

		GD.Print($"Updated Code: {_codeNumbers[0]} {_codeNumbers[1]} {_codeNumbers[2]}");
		
		if (CheckCode())
		{
			UnlockCryptex();
		}
	}
	
	private bool CheckCode()
	{
		string enteredCode = $"{_codeNumbers[0]}{_codeNumbers[1]}{_codeNumbers[2]}";
		return enteredCode == PASS_CODE;
	}
	
	private void UnlockCryptex()
	{
		GD.Print("Cryptex Unlocked! You got the key!");
	}
	
	public void OnIncreaseButton1Pressed() => ChangeNumber(0, 1);
	public void OnDecreaseButton1Pressed() => ChangeNumber(0, -1);
	public void OnIncreaseButton2Pressed() => ChangeNumber(1, 1);
	public void OnDecreaseButton2Pressed() => ChangeNumber(1, -1);
	public void OnIncreaseButton3Pressed() => ChangeNumber(2, 1);
	public void OnDecreaseButton3Pressed() => ChangeNumber(2, -1);
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
		{
			if (keyEvent.Keycode == Key.Key1)
			{
				SpawnNumber(4);
			}
			else if (keyEvent.Keycode == Key.Key2)
			{
				SpawnNumber(9);
			}
		}
	}
}
