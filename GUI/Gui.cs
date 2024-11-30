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

	public override void _Process(double delta)
	{
		var _item = _player.GetHandItem();
		_handItemLabel.Text = $"{_item.Label} {_item.Amount}";
	}
}
