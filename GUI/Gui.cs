using Godot;
using System;

public partial class Gui : CanvasLayer
{
	private Player _player;
	private Label _handItemLabel;
	private Label _timeLabel;

	public override void _Ready()
	{
		_player = GetParent().GetNode<Player>("Player");
		_handItemLabel = GetNode<Label>("HandItemLabel");
		_timeLabel = GetNode<Label>("TimeLabel");
	}

	public override void _Process(double delta)
	{
		var _item = _player.GetHandItem();
		_handItemLabel.Text = $"{_item.Label} {_item.Amount}";
	}

	public void OnTimeTick(int day, int hour, int minute)
	{
		_timeLabel.Text = $"Day: {day}; Hour: {hour}; Minute: {minute}";
	}
}
