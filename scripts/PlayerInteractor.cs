using Godot;
using System;

public partial class PlayerInteractor : RayCast3D
{
  private PlayerInput _inputSystem;
  private IInteractable _interactable;

	public override void _Ready()
	{
    _inputSystem = GetNode<PlayerInput>("%PlayerInput");
    _inputSystem.Interacted += OnInteracted;
	}

	public override void _Process(double delta)
	{
    if (IsColliding())
    {
      if (GetCollider() is IInteractable interactable)
      {
        _interactable = interactable;
        _interactable.ShowHint(GetParent<Node3D>());
      }
    }
    else
    {
      _interactable?.HideHint();
      _interactable = null;
    }
	}

  private void OnInteracted(object sender, EventArgs args)
  {
    if (_interactable is null)
    {
      return;
    }

    _interactable.Interact();
  }
}
