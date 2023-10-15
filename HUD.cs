using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();
	
	[Signal]
	public delegate void ToggleMusicEventHandler();
	
	public override void _Ready()
	{
		GetNode<TextureButton>("SoundOffButton").Hide();
	}
	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();
		
		GetNode<Timer>("MessageTimer").Start();
	}

	async public void ShowGameOver()
	{
		ShowMessage("Game Over");
		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, Timer.SignalName.Timeout);
		
		var message = GetNode<Label>("Message");
		message.Text = "Dodge the\nCreeps!";
		message.Show();
		
		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
	}
	
	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}
	
	private void OnSoundButtonPressed()
	{
		if(GetNode<TextureButton>("SoundOnButton").Visible)
		{
			OnSoundOnButtonPressed();
		}
		else
		{
			OnSoundOffButtonPressed();
		}
		EmitSignal(SignalName.ToggleMusic);
	}

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}
	private void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}
	
	private void OnSoundOnButtonPressed()
	{
		GetNode<TextureButton>("SoundOnButton").Hide();
		GetNode<TextureButton>("SoundOffButton").Show();
	}
    
	private void OnSoundOffButtonPressed()
	{
		GetNode<TextureButton>("SoundOffButton").Hide();
		GetNode<TextureButton>("SoundOnButton").Show();
	}
	
}
