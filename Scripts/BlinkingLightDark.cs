using Godot;
using System;

public partial class BlinkingLightDark : Node
{
    [Export] public Node3D _light;

    [Export] public float FlickerInterval = 0.2f;
    [Export] public float OnTime = 0.05f;
    [Export] public float FlickerDuration = 2.0f;
    [Export] public float WaitBetweenFlicker = 1.5f;

    private Random _random = new Random();

    public override void _Ready()
    {
        BlinkLoop();
    }

    private async void BlinkLoop()
    {
        while (true)
        {
            float delay = (float)(_random.NextDouble() * 1.0 + WaitBetweenFlicker); // 1.5 ± 0.5
            await ToSignal(GetTree().CreateTimer(delay), "timeout");

            float timeElapsed = 0f;

            while (timeElapsed < FlickerDuration)
            {
                _light.Visible = true;
                await ToSignal(GetTree().CreateTimer(OnTime), "timeout");

                _light.Visible = false;
                float interval = (float)(_random.NextDouble() * FlickerInterval * 0.5 + FlickerInterval * 0.5);
                await ToSignal(GetTree().CreateTimer(interval), "timeout");

                timeElapsed += OnTime + interval;
            }

            _light.Visible = false;
        }
    }
}