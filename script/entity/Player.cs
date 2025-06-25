using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody2D, IEntityContainer
{
	[ExportSubgroup("Handlers")]
	// [Export] public GravityHandler GravityHandler { get; set; }
	[Export] public RemoteInputHandler InputHandler { get; set; }
	[Export] public MovementHandler MovementHandler { get; set; }
	[Export] public JumpHandler JumpHandler { get; set; }
	[Export] public AnimationHandler AnimationHandler { get; set; }
	[Export] public AbilityHandler AbilityHandler { get; set; }

	[ExportSubgroup("Nodes")]
	[Export]
	public Entity Entity { get; set; }

	[ExportSubgroup("Multiplayer")]
	[Export] public long Id { get; set; }

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (Multiplayer.IsServer() && InputHandler.GetJumpPressed())
			GD.Print(Id);

		Entity.PointingAt = InputHandler.PointingAt;

		// GravityHandler.HandleGravity(this, delta);
		MovementHandler.HandleHorizontal(this, Entity, delta, InputHandler.HorizontalInput);
		MovementHandler.HandleVertical(this, Entity, delta, InputHandler.VerticalInput);
		JumpHandler.HandleJump(this, InputHandler.GetJumpPressed(), InputHandler.GetJumpReleased());
		AnimationHandler.HandleHorizontalFlip(InputHandler.HorizontalInput);
		// AnimationHandler.HandleWalk(this.Entity.PassiveAttributes.Speed);
		AnimationHandler.HandleAnimations(this, this.Entity);

		AbilityHandler.HandleAbilities(Entity, InputHandler, delta);

		InputHandler.ResetInputSemaphores();

		MoveAndSlide();
	}
}
