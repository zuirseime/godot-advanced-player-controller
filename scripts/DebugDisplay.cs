using Godot;
using System;

public partial class DebugDisplay : VBoxContainer
{
  [Export] private Label _positionDisplay; 
  [Export] private Label _speedDisplay; 

  private Player _player;

	public override void _Ready()
	{
    _player = GetNode<Player>("../../Player");

    _player.GetNode<PlayerMovement>("PlayerMovement").Moved += OnPlayerMoved;
	}

  private void OnPlayerMoved(object sender, (Vector3 position, float speed) args)
  {
    _positionDisplay.Text = $"[{args.position.X:F2}, {args.position.Y:F2}, {args.position.Z:F2}]";
    _speedDisplay.Text = $"{args.speed:F2} m/s";
  }
}
