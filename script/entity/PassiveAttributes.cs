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
	[Export] public float JumpVelocityBase = 350;
	[Export] public float BaseSpeed { get; set; } = 200;
	[Export] public float GroundAcceleration { get; set; } = 3000;
	[Export] public float GroundDeceleration { get; set; } = 4000;
	[Export] public float AirAcceleration { get; set; } = 5000;
	[Export] public float AirDeceleration { get; set; } = 1500;

	[ExportSubgroup("Modifiers")]

	[Export] public float SpeedModifierMultiplicative { get; set; } = 1;
	[Export] public float SpeedModifierFlat { get; set; } = 0;
	[Export] public float JumpModifierMultiplicative { get; set; } = 1;
	[Export] public float JumpModifierFlat { get; set; } = 0;
	[Export] public float StaminaHalvingExponent { get; set; } = 20;

	[Export] public double ReceivedDamagePhysicalModifier { get; set; } = 1;
	[Export] public double ReceivedDamageInspiredModifier { get; set; } = 1;

	#endregion

	#region Calculated

	public float Speed
	{
		get
		{
			return Math.Max(0, BaseSpeed * SpeedModifierMultiplicative + SpeedModifierFlat);
		}
	}

	public float JumpVelocity => Math.Max(0, JumpVelocityBase * JumpModifierMultiplicative + JumpModifierFlat);

	public double StaminaUsageModifier => Math.Pow(Math.E, Swiftness * Math.Log(0.5) / StaminaHalvingExponent);

	public double BeverageRegen => BoozeToleranceIndex * 0.1;
	public double InspirationRegen => OpenMindedness * 0.1;

	public double PhysicalDamageProcess(double flat) => Math.Max(0, flat * ReceivedDamagePhysicalModifier);
	public double InspiredDamageProcess(double flat) => Math.Max(0, flat * ReceivedDamageInspiredModifier);

	#endregion

}
