using Godot;
using System;

public partial class PlayerMovement : Node
{
  [Export] private float _acceleration = 25f;
  [Export] private float _deceleration = 20f;

  public event EventHandler<(Vector3 position, float speed)> Moved;

  private PlayerData _data;

  private Vector3 _movement;
  private float _moveSpeed;

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

    input.MoveInputChanged += OnMovementChanged;
    input.LookInputChanged += OnLookChanged;
    input.SprintChanged += (s, b) => _sprint = b;

    input.CrouchToggled += OnCrouchToggled;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!_body.IsOnFloor())
    {
      _body.Velocity += _body.GetGravity() * (float)delta;
    }

    ApplyMovement(delta);
  }

  private void ApplyMovement(double delta)
  {
    float targetSpeed;

    if (_crouch)
    {
      targetSpeed = _data.CrouchSpeed;
    }
    else if (_sprint && _stamina > 0f)
    {
      targetSpeed = _data.SprintSpeed;
    }
    else
    {
      targetSpeed = _data.WalkSpeed;
    }

    Vector3 targetVelocity = _movement * targetSpeed;
    Vector3 currentVelocity = _body.Velocity;

    float acceleration = _movement.Length() > 0.1f ? _acceleration : _deceleration;

    Vector3 smoothVelocity = new Vector3(
      Mathf.MoveToward(currentVelocity.X, targetVelocity.X, acceleration * (float)delta),
      currentVelocity.Y,
      Mathf.MoveToward(currentVelocity.Z, targetVelocity.Z, acceleration * (float)delta)
    );

    _body.Velocity = smoothVelocity;
    _body.MoveAndSlide();

    Moved?.Invoke(this, (_body.Position, _body.Velocity.Length()));
  }

  private void OnLookChanged(object sender, Vector2 motion)
  {
    float yaw = -motion.X * _data.MouseSensitivityX;
    float pitch = -motion.Y * _data.MouseSensitivityY;

    _body.RotateY(Mathf.DegToRad(yaw));
    _head.RotateX(Mathf.DegToRad(pitch));
    _head.Rotation = new Vector3(Mathf.Clamp(_head.Rotation.X, _data.CameraMinPitch, _data.CameraMaxPitch), 0, 0);
  }

  private void OnMovementChanged(object sender, Vector2 motion)
  {
    _movement = _body.GlobalTransform.Basis * new Vector3(motion.X, 0f, motion.Y);
  }

  private void ApplyCharacterHeight()
  {
    var collider = GetNode<CollisionShape3D>("%PhysicsBody");
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
