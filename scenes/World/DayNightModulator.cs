// https://www.youtube.com/watch?v=HjwWe-V3nHs
// 5:04

using Godot;
using System;

public partial class DayNightModulator : CanvasModulate
{

	private float _time;
	private const int MINUTES_PER_DAY = 1440;
	private const int MINUTES_PER_HOUR = 60;
	private const float INGAME_TO_REAL_MINUTE_DURATION = (2 * (float)Math.PI) / MINUTES_PER_DAY;

	private int _lastDay = -1;
	private int _lastHour = -1;

	[Export]
	public GradientTexture1D Gradient;

	[Export]
	public float IngameSpeed = 24f;

	[Export]
	public int InitialHour = 10;

	private int _initialHour;

	[Signal]
	public delegate void TimeCounterEventHandler(int day, int hour, int minute);

	[Signal]
	public delegate void HourTickEventHandler();

	[Signal]
	public delegate void DayTickEventHandler();


    public override void _Ready()
    {
        base._Ready();

		_time = INGAME_TO_REAL_MINUTE_DURATION * InitialHour * MINUTES_PER_HOUR;
    }

    public override void _Process(double delta)
	{
		_time += (float)delta * IngameSpeed * INGAME_TO_REAL_MINUTE_DURATION;
		float _value = (float)(Math.Sin(_time - Math.PI / 2) + 1.0f) / 2.0f;

		Color = Gradient.Gradient.Sample(_value);

		RecalculateTime();
	}

	private void RecalculateTime()
	{
		int _totalMinutes = (int)(_time / INGAME_TO_REAL_MINUTE_DURATION);
		int _currentDayMinutes = _totalMinutes % MINUTES_PER_DAY;

		int _day = _totalMinutes / MINUTES_PER_DAY;
		int _hour = _currentDayMinutes / MINUTES_PER_HOUR;
		int _minute = _currentDayMinutes % MINUTES_PER_HOUR;

		if (_lastHour != _hour)
		{
			_lastHour = _hour;
			EmitSignal(SignalName.HourTick);
			EmitSignal(SignalName.TimeCounter, _day, _hour, _minute);
		}

		if (_lastDay != _day)
		{
			_lastDay = _day;
			EmitSignal(SignalName.DayTick);
		}
	}
}
