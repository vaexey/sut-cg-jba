using Godot;
using System;

public partial class PassiveAttributes : Node
{
	[ExportSubgroup("Attributes")]
	// Max health modifier
	[Export]
	public double BoozeToleranceIndex { get; set; } = 1.0;

	// Stamina usage modifier
	[Export]
	public double Swiftness { get; set; } = 1.0;

	// Max mana modifier
	[Export]
	public double OpenMindedness { get; set; } = 1.0;

	[ExportSubgroup("Game constants")]
	[Export]
	public double BTI_BEVERAGEMAX_MODIFIER = 10.0;
	[Export]
	public double OM_INSPIRATION_MODIFIER = 10.0;


}
