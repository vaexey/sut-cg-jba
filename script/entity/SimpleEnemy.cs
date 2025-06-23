using Godot;
using System;

public partial class SimpleEnemy : CharacterBody2D, IEntityContainer
{
	[ExportSubgroup("Handlers")]
	// [Export]
	// public GravityHandler GravityHandler { get; set; }

	[Export]
	public MovementHandler MovementHandler { get; set; }

	[Export]
	public JumpHandler JumpHandler { get; set; }
	
	[Export]
	public AnimationHandler AnimationHandler { get; set; }

	[ExportSubgroup("Nodes")]
	[Export]
	public Entity Entity { get; set; }

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

        float horizontal = 0;
        bool jump = false;
        bool jumpR = false;

		// GravityHandler.HandleGravity(this, delta);
		MovementHandler.HandleHorizontal(this, Entity, delta, horizontal);
		JumpHandler.HandleJump(this, jump, jumpR);
		AnimationHandler.HandleHorizontalFlip(horizontal);

		MoveAndSlide();
	}
}
