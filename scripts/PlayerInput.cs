using Godot;
using System;

public partial class PlayerInput : Node
{
  public event EventHandler<Vector2> MoveInputChanged;
  public event EventHandler<Vector2> LookInputChanged;
  public event EventHandler<bool> SprintChanged;
  public event EventHandler CrouchToggled;
  public event EventHandler Interacted;

  public override void _UnhandledInput(InputEvent @event)
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
      
    Vector2 movementDirection = new Vector2(
      Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
      Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward")
    );
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
