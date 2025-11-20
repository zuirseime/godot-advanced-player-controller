using Godot;
using System;

public partial class PlayerInput : Node
{
  public event EventHandler<Vector2> MoveInputChanged;
  public event EventHandler<Vector2> LookInputChanged;
  public event EventHandler<bool> SprintChanged;
  public event EventHandler CrouchToggled;
  public event EventHandler Interacted;

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventMouseButton)
    {
      Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    else if (@event.IsActionPressed("pause"))
    {
      Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    if (Input.MouseMode == Input.MouseModeEnum.Captured &&
        @event is InputEventMouseMotion motion)
    {
      LookInputChanged?.Invoke(this, motion.Relative);
    }
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    Vector2 movementDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward").Normalized();
    MoveInputChanged?.Invoke(this, movementDirection);

    SprintChanged?.Invoke(this, Input.IsActionPressed("sprint"));

    if (Input.IsActionJustPressed("crouch"))
    {
      CrouchToggled?.Invoke(this, EventArgs.Empty);
    }

    if (Input.IsActionJustPressed("interact"))
    {
      Interacted?.Invoke(this, EventArgs.Empty);
    }
  }
}
