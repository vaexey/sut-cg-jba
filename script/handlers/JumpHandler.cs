using Godot;
using System;

public partial class JumpHandler : Node
{
	[ExportSubgroup("Nodes")]
	[Export]
	public Entity Entity { get; set; }
	[Export]
	public Timer JumpBufferTimer { get; set; }
	[Export]
	public Timer CoyoteTimer { get; set; }

	[Signal]
	public delegate void JumpStartedEventHandler();

	bool IsGoingUp = false;
	bool IsJumping = false;
	bool LastFrameOnFloor = false;

	public bool HasJustLanded(CharacterBody2D body) => body.IsOnFloor() && !LastFrameOnFloor && IsJumping;
	public bool IsAllowedToJump(CharacterBody2D body) => body.IsOnFloor() || !CoyoteTimer.IsStopped();
	public bool HasJustSteppedOfTheLedge(CharacterBody2D body) => !body.IsOnFloor() && LastFrameOnFloor && !IsJumping;

	public void HandleJump(CharacterBody2D body, bool wantToJump, bool wantsToStopJump)
	{
		if (!Multiplayer.IsServer())
			return;

		if (HasJustLanded(body))
				IsJumping = false;

		if(IsAllowedToJump(body) && wantToJump && Entity.CanJump())
			jump(body);
		
		handleCoyoteTime(body);
		handleJumpBuffer(body, wantToJump);
		handleVariableJumpHeight(body, wantsToStopJump);

		IsGoingUp = body.Velocity.Y < 0 && !body.IsOnFloor();
		LastFrameOnFloor = body.IsOnFloor();
	}

	private void handleCoyoteTime(CharacterBody2D body)
	{
		if(HasJustSteppedOfTheLedge(body))
			CoyoteTimer.Start();
		
		if(!CoyoteTimer.IsStopped() && !IsJumping)
		{
			body.Velocity = new(body.Velocity.X, 0);
		}
	}

	private void handleJumpBuffer(CharacterBody2D body, bool wantToJump)
	{
		if(wantToJump && !body.IsOnFloor())
			JumpBufferTimer.Start();
		
		if(!JumpBufferTimer.IsStopped() && body.IsOnFloor() && Entity.CanJump())
			jump(body);
	}

	private void handleVariableJumpHeight(CharacterBody2D body, bool wantsToStopJump)
	{
		if(wantsToStopJump && IsGoingUp)
			body.Velocity = new(body.Velocity.X, 0);
	}

	private void jump(CharacterBody2D body)
	{
		Entity.DidJump();
		EmitSignal(SignalName.JumpStarted);
		body.Velocity = new(body.Velocity.X, -Entity.PassiveAttributes.JumpVelocity);
		IsJumping = true;
		JumpBufferTimer.Stop();
		CoyoteTimer.Stop();
	}
}
