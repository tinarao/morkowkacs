using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class World : Node2D
{

	[Signal]
	public delegate void PlowedSuccessfullyEventHandler();

	[Signal]
	public delegate void HarvestedSuccessfullyEventHandler(int seedsAmount);

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

	public void OnPlayerHarvestRequest()
	{
		Vector2 _mousePos = GetLocalMousePosition();
		Vector2I _tilePosition = _plantsLayer.LocalToMap(_mousePos);
		TileData _tile = _plantsLayer.GetCellTileData(_tilePosition);
		if (_tile == null)
		{
			return;
		}

		int _seedsToReturn = 1;
		if (_plantsLayer.GetCellAtlasCoords(_tilePosition).X == 4)
		{
			_seedsToReturn += _random.Next(2, 4);
		}

		EmitSignal(SignalName.HarvestedSuccessfully, _seedsToReturn);
		_plantedVeggiesPositions.RemoveWhere(v => v == _tilePosition);
		
		int _vegIndex = -1;
		foreach (var _veggie in _plantedVeggiesArr)
		{
			if (_veggie.TileCoordinates == _tilePosition)
			{
				_vegIndex = _plantedVeggiesArr.IndexOf(_veggie);
			}
		}
		Debug.Assert(_vegIndex > -1);

		_plantedVeggiesArr.RemoveAt(_vegIndex);
		_plantsLayer.SetCell(_tilePosition);
	}

	// Time-based events
	public void OnDayTickEvents()
	{
		for (int i = 0; i < _plantedVeggiesArr.Count; i++)
		{
			var _randint = _random.Next(1, 100);
			if (_randint > 65)
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
