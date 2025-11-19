using Godot;

[GlobalClass]
public partial class PlayerData : Resource
{
  [ExportGroup("Move Speed")]
  [Export] public float WalkSpeed { get; set; }
  [Export] public float SprintSpeed { get; set; }
  [Export] public float CrouchSpeed { get; set; }

  [ExportGroup("Stamina")]
  [Export] public float MaxStamina { get; set; }
  [Export] public float StaminaDrainPerSecond { get; set; }
  [Export] public float StaminaRegenPerSecond { get; set; }
  [Export] public float StaminaRegenDelay { get; set; }

  [ExportGroup("Sanity")]
  [Export] public float MaxSanity { get; set; }
  [Export] public float SanityDrainPerSecond { get; set; }
  [Export] public float SanityRegenPerSecond { get; set; }
  [Export] public float SanityRegenCrowdFactor { get; set; }
  [Export] public float SanityRegenDelay { get; set; }

  [ExportGroup("Camera")]
  [ExportSubgroup("Sensitivity")]
  [Export] public float MouseSensitivityX { get; set; }
  [Export] public float MouseSensitivityY { get; set; }

  [ExportSubgroup("Restictions")]
  [Export] public float CameraMinPitch { get; set; }
  [Export] public float CameraMaxPitch { get; set; }

  [ExportGroup("Collider")]
  [Export] public float StandHeight { get; set; }
  [Export] public float CrouchHeight { get; set; }
}
