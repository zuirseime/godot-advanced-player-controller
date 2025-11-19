using Godot;
using System;

public partial class PlayerMovement : Node
{
  private PlayerData _data { get; set; }

  private Vector2 _movement;

  private bool _sprint;
  private bool _crouch;

  private float _stamina;

  private CharacterBody3D _body;
  private Node3D _head;
  private AnimationTree _animator;

  public override void _Ready()
  {
    _data = GetParent<Player>().Data;

    _body = GetParent<CharacterBody3D>();
    _head = GetNode<Node3D>("%Head");
    _animator = GetNode<AnimationTree>("../Model/AnimationTree");

    _stamina = _data.MaxStamina;

    var input = GetNode<PlayerInput>("%PlayerInput");

    input.MoveInputChanged += (s, v) => _movement = v;
    input.LookInputChanged += OnLookChanged;
    input.SprintChanged += (s, b) => _sprint = b;

    input.CrouchToggled += OnCrouchToggled;
  }

  public override void _PhysicsProcess(double delta)
  {
    Vector3 velocity = _body.Velocity;

    if (!_body.IsOnFloor())
    {
      velocity += _body.GetGravity() * (float)delta;
    }

    // ApplyLook(delta); 
    ApplyMovement(delta);
  }

  private void OnLookChanged(object sender, Vector2 motion)
  {
    float yaw = -motion.X * _data.MouseSensitivityX;
    float pitch = -motion.Y * _data.MouseSensitivityY;

    _body.RotateY(Mathf.DegToRad(yaw));
    _head.RotateX(Mathf.DegToRad(pitch));
    _head.Rotation = new Vector3(Mathf.Clamp(_head.Rotation.X, _data.CameraMinPitch, _data.CameraMaxPitch), 0, 0);
  }

  private void ApplyMovement(double delta)
  {
    
  }

  private void ApplyCharacterHeight()
  {
    var collider = GetNode<CollisionShape3D>("PhysicsBody");
    var shape = collider.Shape as CapsuleShape3D;

    if (shape is null) 
    {
      GD.Print("Collision Shape not found!");
      return;
    }

    if (_crouch)
    {
      shape.Height = _data.CrouchHeight;
    }
    else
    {
      shape.Height = _data.StandHeight;
    }
  }

  private void OnCrouchToggled(object sender, EventArgs args)
  {
    _crouch = !_crouch;

    if (_crouch)
    {
      _sprint = false;
    }

    ApplyCharacterHeight();
  }
}
