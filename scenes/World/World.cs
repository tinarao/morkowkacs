using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class World : Node2D
{

	[Signal]
	public delegate void PlowedSuccessfullyEventHandler();

	[Signal]
	public delegate void PlantedBeetrootSuccessfullyEventHandler();

	private TileMapLayer _hillsLayer;
	private TileMapLayer _plowsLayer;
	private TileMapLayer _plantsLayer;

	private HashSet<Vector2> _tilesAllowedToPlowAtlases;
	private Array<Vector2I> _plowedTilesPosition;
	private HashSet<Vector2I> _plantedVeggiesPositions;
	private List<PlantedVeggie> _plantedVeggiesArr;

	private Random _random;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_random = new Random();

		_hillsLayer = GetNode<TileMapLayer>("Hills");
		_plowsLayer = GetNode<TileMapLayer>("Plows");
		_plantsLayer = GetNode<TileMapLayer>("PlantedSeeds");

		_plantedVeggiesArr = new List<PlantedVeggie>();
		_plowedTilesPosition = new Array<Vector2I>();
		_tilesAllowedToPlowAtlases = new HashSet<Vector2>();
		_plantedVeggiesPositions = new HashSet<Vector2I>();

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

		Vector2I _tileAtlasCoordinates = _hillsLayer.GetCellAtlasCoords(_tilePosition);
		if (!_tilesAllowedToPlowAtlases.Contains(_tileAtlasCoordinates))
		{
			return;
		}

		_plowedTilesPosition.Add(_tilePosition);
		_plowsLayer.SetCellsTerrainConnect(_plowedTilesPosition, 0, 0, false);
		EmitSignal(SignalName.PlowedSuccessfully);
	}

	public void OnPlayerPlantBeetrootRequest()
	{
		Vector2 _mousePos = GetLocalMousePosition();
		Vector2I _tilePosition = _plowsLayer.LocalToMap(_mousePos);
		TileData _tile = _plowsLayer.GetCellTileData(_tilePosition);
		if (_tile == null)
		{
			return;
		}

		if (_plantedVeggiesPositions.Contains(_tilePosition))
		{
			return;
		}

		Vector2I _atlas = new Vector2I(1, 1);

		PlantedVeggie _veggie = new PlantedVeggie(_tilePosition, _atlas);
		_plantedVeggiesArr.Add(_veggie);

		_plantedVeggiesPositions.Add(_tilePosition);
		_plantsLayer.SetCell(_tilePosition, 0, _atlas);

		EmitSignal(SignalName.PlantedBeetrootSuccessfully);
	}

	// Time-based events
	public void OnDayTickEvents()
	{
		for (int i = 0; i < _plantedVeggiesArr.Count; i++)
		{
			var _randint = _random.Next(1, 100);
			// _randint > 65
			if (true)
			{
				if (_plantedVeggiesArr[i].AtlasCoordinates.X == 4)
				{
					return;
				}

				_plantedVeggiesArr[i].AtlasCoordinates.X += 1;
				_plantsLayer.SetCell(_plantedVeggiesArr[i].TileCoordinates, 0, _plantedVeggiesArr[i].AtlasCoordinates);
			}
		}
	}
}
