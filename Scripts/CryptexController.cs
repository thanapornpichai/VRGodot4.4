using Godot;

public partial class CryptexController : Node3D
{
    private const string PASS_CODE = "911";
    private int[] _codeNumbers = new int[3];

    [Export] public MeshInstance3D firstNumber;
    [Export] public MeshInstance3D secondNumber;
    [Export] public MeshInstance3D thirdNumber;

    private TextMesh firstNumberText;
    private TextMesh secondNumberText;
    private TextMesh thirdNumberText;

    [Export] public Area3D IncreaseButton1;
    [Export] public Area3D DecreaseButton1;
    [Export] public Area3D IncreaseButton2;
    [Export] public Area3D DecreaseButton2;
    [Export] public Area3D IncreaseButton3;
    [Export] public Area3D DecreaseButton3;

    [Export] public float delay = 3.0f;
    [Export] public PackedScene keyCard;

    private bool _isUnlocked = false;

    public override void _Ready()
    {
        firstNumber.Mesh = (TextMesh)firstNumber.Mesh.Duplicate();
        secondNumber.Mesh = (TextMesh)secondNumber.Mesh.Duplicate();
        thirdNumber.Mesh = (TextMesh)thirdNumber.Mesh.Duplicate();

        firstNumberText = firstNumber.Mesh as TextMesh;
        firstNumberText.Text = "0";

        secondNumberText = secondNumber.Mesh as TextMesh;
        secondNumberText.Text = "0";

        thirdNumberText = thirdNumber.Mesh as TextMesh;
        thirdNumberText.Text = "0";

        IncreaseButton1.BodyEntered += OnIncreaseButton1;
        DecreaseButton1.BodyEntered += OnDecreaseButton1;

        IncreaseButton2.BodyEntered += OnIncreaseButton2;
        DecreaseButton2.BodyEntered += OnDecreaseButton2;

        IncreaseButton3.BodyEntered += OnIncreaseButton3;
        DecreaseButton3.BodyEntered += OnDecreaseButton3;
    }

    public void ChangeNumber(int position, int change)
    {
        if (_isUnlocked)
            return;

        if (position >= 0 && position < _codeNumbers.Length)
        {
            _codeNumbers[position] += change;

            if (_codeNumbers[position] > 9) _codeNumbers[position] = 0;
            if (_codeNumbers[position] < 0) _codeNumbers[position] = 9;

            firstNumberText.Text = _codeNumbers[0].ToString();
            secondNumberText.Text = _codeNumbers[1].ToString();
            thirdNumberText.Text = _codeNumbers[2].ToString();
            GD.Print($"Updated Code: {_codeNumbers[0]} {_codeNumbers[1]} {_codeNumbers[2]}");

            if (CheckCode())
            {
                UnlockCryptex();
            }
        }
    }

    private bool CheckCode()
    {
        string enteredCode = $"{_codeNumbers[0]}{_codeNumbers[1]}{_codeNumbers[2]}";
        return enteredCode == PASS_CODE;
    }

    private async void UnlockCryptex()
    {
        if (_isUnlocked)
            return;

        _isUnlocked = true;
        GD.Print("Cryptex Unlocked! You got the key!");

        IncreaseButton1.Monitoring = false;
        DecreaseButton1.Monitoring = false;
        IncreaseButton2.Monitoring = false;
        DecreaseButton2.Monitoring = false;
        IncreaseButton3.Monitoring = false;
        DecreaseButton3.Monitoring = false;

        await ToSignal(GetTree().CreateTimer(delay), "timeout");

        this.Visible = false;

        if (keyCard != null)
        {
            Node3D cardInstance = keyCard.Instantiate<Node3D>();
            GetParent().AddChild(cardInstance);
            cardInstance.GlobalTransform = this.GlobalTransform;
        }
    }

    private void OnIncreaseButton1(Node3D body)
    {
        if (body.IsInGroup("PlayerHand"))
            ChangeNumber(0, 1);
    }

    private void OnDecreaseButton1(Node3D body)
    {
        if (body.IsInGroup("PlayerHand"))
            ChangeNumber(0, -1);
    }

    private void OnIncreaseButton2(Node3D body)
    {
        if (body.IsInGroup("PlayerHand"))
            ChangeNumber(1, 1);
    }

    private void OnDecreaseButton2(Node3D body)
    {
        if (body.IsInGroup("PlayerHand"))
            ChangeNumber(1, -1);
    }

    private void OnIncreaseButton3(Node3D body)
    {
        if (body.IsInGroup("PlayerHand"))
            ChangeNumber(2, 1);
    }

    private void OnDecreaseButton3(Node3D body)
    {
        if (body.IsInGroup("PlayerHand"))
            ChangeNumber(2, -1);
    }
}
