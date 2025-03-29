using Godot;
using Godot.Collections;

public partial class CryptexController : Node
{
	private const string PASS_CODE = "911";
	private int[] _codeNumbers = new int[3];

	[Export] public Array<PackedScene> NumberList = new Array<PackedScene>();
	private Node3D firstNumber;
	private Node3D secondNumber;
	private Node3D thirdNumber;

	[Export] public Node3D firstNumberPosition;
	[Export] public Node3D secondNumberPosition;
	[Export] public Node3D thirdNumberPosition;

	public override void _Ready()
	{
		firstNumber = InstantiateNumber(0, firstNumberPosition);
		secondNumber = InstantiateNumber(0, secondNumberPosition);
		thirdNumber = InstantiateNumber(0, thirdNumberPosition);
	}

	public void ChangeNumber(int position, int change)
	{
		if (position >= 0 && position < _codeNumbers.Length)
		{
			_codeNumbers[position] += change;

			if (_codeNumbers[position] > 9) _codeNumbers[position] = 0;
			if (_codeNumbers[position] < 0) _codeNumbers[position] = 9;

			ReplaceNumber(position, _codeNumbers[position]);

			GD.Print($"Updated Code: {_codeNumbers[0]} {_codeNumbers[1]} {_codeNumbers[2]}");

			if (CheckCode())
			{
				UnlockCryptex();
			}
		}
	}

	private Node3D InstantiateNumber(int numberIndex, Node3D position)
	{
		Node3D instance = NumberList[numberIndex].Instantiate<Node3D>();
		AddChild(instance);
		instance.GlobalTransform = new Transform3D(Basis.Identity, position.Position);
		return instance;
	}

	private void ReplaceNumber(int position, int newNumber)
	{
		switch (position)
		{
			case 0:
				firstNumber.QueueFree();
				firstNumber = InstantiateNumber(newNumber, firstNumberPosition);
				break;
			case 1:
				secondNumber.QueueFree();
				secondNumber = InstantiateNumber(newNumber, secondNumberPosition);
				break;
			case 2:
				thirdNumber.QueueFree();
				thirdNumber = InstantiateNumber(newNumber, thirdNumberPosition);
				break;
		}
	}

	private bool CheckCode()
	{
		string enteredCode = $"{_codeNumbers[0]}{_codeNumbers[1]}{_codeNumbers[2]}";
		return enteredCode == PASS_CODE;
	}

	// Called when the code is correct
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
}
