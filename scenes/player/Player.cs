using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public bool IsIdle;

	private Vector2 _moveVec;
	private Vector2 _latestFacingDirection;

	private AnimationTree _animTree;

	public enum InvItemType {
		Tool,
		Seed,
	}

	public struct InvItem {
		public InvItem(string label, uint amount, InvItemType type)
		{
			Label = label;
			Amount = amount;
			Type = type;
		}

		public string Label;
		public uint Amount;
		public InvItemType Type;
	}

    public override void _Ready()
    {
        base._Ready();

		_animTree = GetNode<AnimationTree>("AnimationTree");

		_animTree.Active = true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		IsIdle = Velocity == Vector2.Zero;
    }

    public override void _PhysicsProcess(double delta)
	{
		_moveVec = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Normalized();
		Velocity = _moveVec * Speed;
		

		HandleAnimations();
		MoveAndSlide();
	}

	private void HandleAnimations()
	{
		bool _isIdle = Velocity == Vector2.Zero;
		if (_isIdle)
		{
			_latestFacingDirection = Velocity.Normalized();
		}

		_animTree.Set("parameters/conditions/IsIdle", Velocity == Vector2.Zero);
		_animTree.Set("parameters/conditions/IsWalking", Velocity != Vector2.Zero);

		_animTree.Set("parameters/Idle/blend_position", _latestFacingDirection);
		_animTree.Set("parameters/Walk/blend_position", _latestFacingDirection);
	}
}
