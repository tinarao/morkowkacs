using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class World : Node2D
{

	[Signal]
	public delegate void PlowedSuccessfullyEventHandler();

	private TileMapLayer _hillsLayer;
	private TileMapLayer _plowsLayer;

	private HashSet<Vector2> _tilesAllowedToPlowAtlases;
	private Array<Vector2I> _plowedTilesPosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hillsLayer = GetNode<TileMapLayer>("Hills");
		_plowsLayer = GetNode<TileMapLayer>("Plows");

		_plowedTilesPosition = new Array<Vector2I>();
		_tilesAllowedToPlowAtlases = new HashSet<Vector2>();

		_tilesAllowedToPlowAtlases.Add(new Vector2(1, 1));
		
		_tilesAllowedToPlowAtlases.Add(new Vector2(0, 5));
		_tilesAllowedToPlowAtlases.Add(new Vector2(1, 5));
		_tilesAllowedToPlowAtlases.Add(new Vector2(2, 5));
		_tilesAllowedToPlowAtlases.Add(new Vector2(3, 5));
		_tilesAllowedToPlowAtlases.Add(new Vector2(4, 5));
		_tilesAllowedToPlowAtlases.Add(new Vector2(5, 5));

		_tilesAllowedToPlowAtlases.Add(new Vector2(0, 6));
		_tilesAllowedToPlowAtlases.Add(new Vector2(1, 6));
		_tilesAllowedToPlowAtlases.Add(new Vector2(2, 6));
		_tilesAllowedToPlowAtlases.Add(new Vector2(3, 6));
		_tilesAllowedToPlowAtlases.Add(new Vector2(4, 6));
		_tilesAllowedToPlowAtlases.Add(new Vector2(5, 6));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayerPlowRequest()
	{
		Vector2 _mousePos = GetLocalMousePosition();
		Vector2I _tilePosition = _hillsLayer.LocalToMap(_mousePos);
		TileData _tile = _hillsLayer.GetCellTileData(_tilePosition);

		if (_tile == null)
		{
			return;
		}

		Vector2 _tileAtlasCoordinates = _hillsLayer.GetCellAtlasCoords(_tilePosition);
		if (!_tilesAllowedToPlowAtlases.Contains(_tileAtlasCoordinates))
		{
			return;
		}

		_plowedTilesPosition.Add(_tilePosition);
		_plowsLayer.SetCellsTerrainConnect(_plowedTilesPosition, 0, 0, false);
		EmitSignal(SignalName.PlowedSuccessfully);
	}
}
