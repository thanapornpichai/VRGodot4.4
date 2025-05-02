using Godot;
using System;
using Godot.Collections;

public partial class SoundEmitter : Node
{
	[Export] public bool Is3D = true;
	[Export] public bool RandomizePitch = true;
	[Export] public Vector2 PitchRange = new Vector2(0.9f, 1.1f);
	[Export] public bool Autoplay = false;
	[Export] public Array<AudioStream> SoundList = new();

	private AudioStreamPlayer _player2D;
	private AudioStreamPlayer3D _player3D;

	public override void _Ready()
	{
		if (Is3D)
		{
			_player3D = new AudioStreamPlayer3D();
			AddChild(_player3D);
		}
		else
		{
			_player2D = new AudioStreamPlayer();
			AddChild(_player2D);
		}

		if (Autoplay)
			PlaySound();
	}

	public void PlaySound()
	{
		if (SoundList.Count == 0)
		{
			GD.PushWarning("SoundEmitter: No sound clips assigned.");
			return;
		}

		var chosen = SoundList[(int)(GD.Randi() % (ulong)SoundList.Count)];
		float pitch = RandomizePitch ? (float)GD.RandRange(PitchRange.X, PitchRange.Y) : 1.0f;

		if (Is3D && _player3D != null)
		{
			_player3D.Stream = chosen;
			_player3D.PitchScale = pitch;
			_player3D.Play();
		}
		else if (!Is3D && _player2D != null)
		{
			_player2D.Stream = chosen;
			_player2D.PitchScale = pitch;
			_player2D.Play();
		}
	}

	public void StopSound()
	{
		if (Is3D && _player3D?.Playing == true)
			_player3D.Stop();
		else if (!Is3D && _player2D?.Playing == true)
			_player2D.Stop();
	}

	public void SetVolumeDb(float db)
	{
		if (Is3D && _player3D != null)
			_player3D.VolumeDb = db;
		else if (!Is3D && _player2D != null)
			_player2D.VolumeDb = db;
	}
}
 
