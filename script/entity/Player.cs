using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody2D, IEntityContainer
{
	[ExportSubgroup("Handlers")]
	[Export] public GravityHandler GravityHandler { get; set; }
	[Export] public InputHandler InputHandler { get; set; }
	[Export] public MovementHandler MovementHandler { get; set; }
	[Export] public JumpHandler JumpHandler { get; set; }
	[Export] public AnimationHandler AnimationHandler { get; set; }
	[Export] public AbilityHandler AbilityHandler { get; set; }

	[ExportSubgroup("Nodes")]
	[Export]
	public Entity Entity { get; set; }

    // public override string[] _GetConfigurationWarnings()
    // {
    //     return Assertions.Stack(base._GetConfigurationWarnings())
	// 		.AssertNotNull(GravityHandler)
	// 		.AssertNotNull(InputHandler)
	// 		.AssertNotNull(JumpHandler);
    // }

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		GravityHandler.HandleGravity(this, delta);
		MovementHandler.HandleHorizontal(this, Entity, delta, InputHandler.HorizontalInput);
		JumpHandler.HandleJump(this, InputHandler.GetJumpPressed(), InputHandler.GetJumpReleased());
		AnimationHandler.HandleHorizontalFlip(InputHandler.HorizontalInput);

		MoveAndSlide();
	}

    public override void _Process(double delta)
    {
		// TODO: propagate delta
		AbilityHandler.HandleAbilities(this, InputHandler, delta);

		// if(Input.IsActionJustPressed("ability_basic"))
		// {
		// 	var proj = ProjectileLibrary.AutoJodlerProjectile.Make();

		// 	// proj.Position = Position;
		// 	// proj.LookAt(GetGlobalMousePosition());
		// 	// proj.Velocity = new Vector2(20,0).Rotated(proj.Rotation);
		// 	proj.Shoot(Position, GetGlobalMousePosition());
		// 	proj.OwnerEntity = Entity;

		// 	Entity.World.AddProjectile(proj);
		// }

		// if(Input.IsActionJustPressed("ability_godlike"))
		// {
		// 	if(Entity.Abilities.Godlike.CanUse(this) == AbilityUsageTrialResult.OK)
		// 		Entity.Abilities.Godlike.Use(this);
		// }

		// if(Input.IsActionJustPressed("ability_basic"))
		// {
		// 	if(Entity.Abilities.Basic[0].CanUse(this) ==
		// 		AbilityUsageTrialResult.OK)
		// 		Entity.Abilities.Basic[0].Use(this);
		// }
    }

	// public int MaxSpeed { get; set; } = 500;

	// public int MoveAcceleration { get; set; } = 5000;

	// public int JumpForce { get; set; } = 3000;
	// // public int JumpGravity { get; set; } = 10000;
	// public int Gravity { get; set; } = 2000;

	// // Normalized vector of movement input
	// private Vector2 _moveInput = Vector2.Zero;
	// private Vector2 _moveVelocity = Vector2.Zero;

	// private Vector2 _jumpVelocity = Vector2.Zero;

	// public override void _Ready()
	// {
	// 	base._Ready();
	// }

	// public override void _Process(double delta)
	// {
	// 	base._Process(delta);
	// }

	// public override void _PhysicsProcess(double delta)
	// {
	// 	base._PhysicsProcess(delta);

	// 	// MOVEMENT <-->
	// 	_moveInput = Vector2.Zero;

	// 	if(Input.IsActionPressed("move_left"))
	// 	{
	// 		_moveInput.X -= 1;
	// 	}

	// 	if(Input.IsActionPressed("move_right"))
	// 	{
	// 		_moveInput.X += 1;
	// 	}

	// 	// if(Input.IsActionPressed("move_up"))
	// 	// {
	// 	// 	_moveInput.Y -= 1;
	// 	// }

	// 	// if(Input.IsActionPressed("move_down"))
	// 	// {
	// 	// 	_moveInput.Y += 1;
	// 	// }

	// 	_moveInput = _moveInput.Normalized();
	// 	_moveVelocity += _moveInput * MoveAcceleration * (float)delta;

	// 	if(_moveVelocity.Length() > MaxSpeed)
	// 	{
	// 		_moveVelocity *= MaxSpeed / _moveVelocity.Length();
	// 	}

	// 	if(_moveInput.Length() == 0)
	// 	{
	// 		var slowValue = _moveVelocity.Normalized() * MoveAcceleration * (float)delta;
	// 		var newMoveVelocity = _moveVelocity - slowValue;

	// 		if(_moveVelocity.Length() < slowValue.Length())
	// 		{
	// 			newMoveVelocity = Vector2.Zero;
	// 		}

	// 		_moveVelocity = newMoveVelocity;
	// 	}

	// 	// JUMPS
	// 	if(_jumpVelocity.Length() > 0)
	// 	{
	// 		var slowValue = _jumpVelocity.Normalized() * MoveAcceleration * (float)delta;
	// 		var newJumpVelocity = _jumpVelocity - slowValue;

	// 		if(_jumpVelocity.Length() < slowValue.Length())
	// 		{
	// 			newJumpVelocity = Vector2.Zero;
	// 		}

	// 		_jumpVelocity = newJumpVelocity;
	// 	}

	// 	if(Input.IsActionJustPressed("move_jump") && _jumpVelocity.Length() < Mathf.Epsilon)
	// 	{
	// 		_jumpVelocity.Y -= JumpForce;
	// 	}


	// 	var velocity = Vector2.Zero;

	// 	velocity.Y += Gravity;
	// 	velocity += _moveVelocity;
	// 	velocity += _jumpVelocity;

	// 	Velocity = velocity;
	// 	MotionMode = MotionModeEnum.Grounded;
	// 	MoveAndSlide();
	// }

	// // public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	// // {
	// //     base._IntegrateForces(state);

	// // 	if(state.GetContactCount() > 0)
	// // 	{
	// // 		_contactNorm = state.GetContactLocalNormal(0);
	// // 	} else {
	// // 		_contactNorm = null;
	// // 	}
	// // }

	// // public override void _PhysicsProcess(double delta)
	// // {
	// //     base._PhysicsProcess(delta);

	// // 	if(Input.IsActionJustPressed("move_jump") && _contactNorm != null)
	// // 	{
	// // 		ApplyCentralImpulse(_contactNorm * JumpForce);
	// // 	}

	// // 	// var force = Vector2.Zero;

	// // 	if(Input.IsActionPressed("move_left") && _contactNorm != null)
	// // 	{
	// // 		ApplyCentralForce(new Vector2(
	// // 			-1, 0
	// // 		) * (float)delta * Speed);
	// // 		// force.X -= 1;
	// // 	}

	// // 	if(Input.IsActionPressed("move_right") && _contactNorm != null)
	// // 	{
	// // 		// force.X += 1;
	// // 	}

	// // 	// force = force.Normalized() * (float)delta * 1000;

	// // 	// ApplyCentralForce(force);



	// // 	// if(Input.IsActionPressed("move_jump"))
	// // 	// {
	// // 	// 	ConstantForce = new(0, -1000);
	// // 	// } else {
	// // 	// 	ConstantForce = Vector2.Zero;
	// // 	// }
	// // }
}
