using Godot;
using System;

public partial class Main : Node
{
    [Export] public PackedScene MobScene { get; set; }
    
    private int _score;
    private float _playbackPosition;
    
    public override void _Ready()
    {
        _playbackPosition = 0.0f;
        GetNode<AudioStreamPlayer2D>("Music").Play();
    }
    public void GameOver()
    {
        GetNode<AudioStreamPlayer2D>("DeathSound").Play();
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        GetNode<HUD>("HUD").ShowGameOver();
    }

    public void NewGame()
    {
        _score = 0;
        var hud = GetNode<HUD>("HUD");
        hud.UpdateScore(_score);
        hud.ShowMessage("Get Ready!");
		
        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);
		
        GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
        GetNode<Timer>("StartTimer").Start();
    }
	
    private void OnScoreTimerTimeout()
    {
        _score++;
        GetNode<HUD>("HUD").UpdateScore(_score);
    }
	
    private void OnStartTimerTimeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    private void OnMobTimerTimeout()
    {
        Mob mob = MobScene.Instantiate<Mob>();
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = GD.Randf();

        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
        mob.Position = mobSpawnLocation.Position;

        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        AddChild(mob);
    }

    private void OnToggleMusic()
    {
        if (GetNode<AudioStreamPlayer2D>("Music").Playing)
        {
            _playbackPosition = GetNode<AudioStreamPlayer2D>("Music").GetPlaybackPosition();
            GetNode<AudioStreamPlayer2D>("Music").Stop();
        }
        else
        {
            GetNode<AudioStreamPlayer2D>("Music").Play(_playbackPosition);
        }

    }
}
