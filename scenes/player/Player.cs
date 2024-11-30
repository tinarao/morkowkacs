using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

public partial class Player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public bool IsIdle;
	public bool IsPlowing;

	private Vector2 _moveVec;
	private Vector2 _latestFacingDirection;

	private AnimationTree _animTree;
	private AnimationNodeStateMachinePlayback _stateMachine;

	public enum InvItemType {
		Tool,
		Seed,
		Nothing,
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

	public HashSet<InvItem> Inventory;
	public InvItem HandSelectedItem;

	InvItem _hoe = new InvItem("Мотыга", 1, InvItemType.Tool);
	InvItem _beetrootSeeds = new InvItem("Семяна свеклы", 9, InvItemType.Seed);
	InvItem _nothing = new InvItem("Пусто", 1, InvItemType.Nothing);

	[Signal]
	public delegate void PlowSignalEventHandler();

	[Signal]
	public delegate void PlantBeetrootSignalEventHandler();

    public override void _Ready()
    {
        base._Ready();

		Inventory = new HashSet<InvItem>();

		_animTree = GetNode<AnimationTree>("AnimationTree");
		_stateMachine = (AnimationNodeStateMachinePlayback)_animTree.Get("parameters/playback");

		_stateMachine.Start("Idle");
		_animTree.Active = true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		IsIdle = Velocity == Vector2.Zero;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

		switch (@event.AsText()) {
			case "1":
				HandSelectedItem = _hoe;
				break;
			case "2":
				HandSelectedItem = _beetrootSeeds;
				break;
			case "0":
				HandSelectedItem = _nothing;
				break;

		}

		if (Input.IsActionJustPressed("ui_action"))
		{
			HandleActions();
		}
		else
		{
			IsPlowing = false;
		}
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
			_stateMachine.Travel("Idle");
		}
		else
		{
			_latestFacingDirection = Velocity;
			_stateMachine.Travel("Walk");
		}
		
		_animTree.Set("parameters/Idle/blend_position", _latestFacingDirection);
		_animTree.Set("parameters/Walk/blend_position", _latestFacingDirection);
		_animTree.Set("parameters/Plow/blend_position", _latestFacingDirection);
	}

	public void HandleActions()
	{
		switch (HandSelectedItem.Label)
		{
			case "Мотыга":
				Plow();
				break;
			case "Семяна свеклы":
				PlantBeetroot();
				break;
		}
	}

	public InvItem GetHandItem()
	{
		return HandSelectedItem;
	}

	// Actions
	// 		Plow
	public void Plow()
	{
		EmitSignal(SignalName.PlowSignal);
	}

	public void OnPlowedSuccessfully()
	{
		_stateMachine.Travel("Plow");
	}

	// 		Plant a beetroot
	public void PlantBeetroot()
	{
		if (_beetrootSeeds.Amount == 0)
		{
			return;
		}
		EmitSignal(SignalName.PlantBeetrootSignal);
	}

	public void OnPlantedBeetrootSuccessfully()
	{
		_beetrootSeeds.Amount -= 1;
	}
}
