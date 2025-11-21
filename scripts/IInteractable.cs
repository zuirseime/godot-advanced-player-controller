using Godot;
using System;

public interface IInteractable 
{
  Vector3 ShapeScale { get; set; }

  void Interact();
  void ShowHint(Node3D head);
  void HideHint();
}
