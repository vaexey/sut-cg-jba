using Godot;
using System;

public partial class PassiveAttributes : Node
{
	#region Parameters

	[ExportGroup("Main attributes")]
	// Max health modifier
	[Export] public double BoozeToleranceIndex { get; set; } = 10.0;

	// Stamina usage modifier
	[Export] public double Swiftness { get; set; } = 1.0;

	// Max mana modifier
	[Export] public double OpenMindedness { get; set; } = 10.0;

	[ExportGroup("Health")]
	[Export] public double BeverageMaxFromBTI = 10.0;

	[ExportGroup("Magic")]
	[Export] public double InspirationMaxFromOM = 20.0;

	[ExportGroup("Movement")]

	[ExportSubgroup("Base values")]

	[Export] public double StaminaRegen = 0.1;
	[Export] public double StaminaUsageJump = 0.1;
	[Export] public double StaminaMinJump = 0.15;
	[Export] public float JumpVelocity = 350;
	[Export] public float BaseSpeed { get; set; } = 200;
	[Export] public float GroundAcceleration { get; set; } = 3000;
	[Export] public float GroundDeceleration { get; set; } = 4000;
	[Export] public float AirAcceleration { get; set; } = 5000;
	[Export] public float AirDeceleration { get; set; } = 1500;
	
	[ExportSubgroup("Modifiers")]

	[Export] public float SpeedModifierMultiplicative { get; set; } = 1;
	[Export] public float SpeedModifierFlat { get; set; } = 0;

	#endregion

	#region Calculated

	public float Speed {
		get {
			return BaseSpeed * SpeedModifierMultiplicative + SpeedModifierFlat;
		}
	}

	#endregion

}
