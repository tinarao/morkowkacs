using Godot;

public partial class Camera2d : Camera2D
{
	private const float MIN_ZOOM = 1.0f;
	private const float MAX_ZOOM = 5.0f;

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

		if (Input.IsActionJustPressed("mouse_wheel_up") && Zoom.X < MAX_ZOOM)
		{
			Vector2 _newZoom = new Vector2(Zoom.X + 0.25f, Zoom.X + 0.25f);
			Zoom = _newZoom;
		}

		if (Input.IsActionJustPressed("mouse_wheel_down") && Zoom.X > MIN_ZOOM)
		{
			Vector2 _newZoom = new Vector2(Zoom.X - 0.25f, Zoom.X - 0.25f);
			Zoom = _newZoom;
		}
    }
}
