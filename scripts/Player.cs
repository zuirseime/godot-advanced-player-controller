using Godot;
using System;

public partial class Player : CharacterBody3D
{
  [Export] public PlayerData Data { get; set; }
  private PlayerInput _input;

  public override void _Ready()
  {
    _input  = GetNode<PlayerInput>("%PlayerInput");
  }
}
