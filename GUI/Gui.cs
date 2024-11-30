using Godot;
using System;

public partial class Gui : CanvasLayer
{
	private Player _player;
	private Label _handItemLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetParent().GetNode<Player>("Player");
		_handItemLabel = GetNode<Label>("HandItemLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_handItemLabel.Text = _player.HandSelectedItem.Label;
	}
}
