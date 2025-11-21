using Godot;
using System;
using System.Linq;

public partial class Interactable : StaticBody3D, IInteractable
{
  [Export] public Vector3 ShapeScale { get; set; }

  private Label3D _hintLabel;

  public void Interact()
  {
    GD.Print($"Congratulation! You interacted with {GetParent().Name}");
  }

  public void ShowHint(Node3D head)
  {
    _hintLabel.Visible = true;
  }

  public void HideHint()
  {
    _hintLabel.Visible = false;
  }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
    var collisonShape = GetNode<CollisionShape3D>("Collision");
    collisonShape.Scale = ShapeScale;

    _hintLabel = GetNode<Label3D>("%Hint");
    _hintLabel.Text = $"Press \"{GetActionKey("interact")}\" to interact";

    HideHint();
	}

  public Key? GetActionKey(string actionName)
  {
    var events = InputMap.ActionGetEvents(actionName);
    foreach (var ev in events)
    {
      if (ev is InputEventKey keyEvent)
      {
        return keyEvent.PhysicalKeycode;
      }
    }
    return null;
  }
}
