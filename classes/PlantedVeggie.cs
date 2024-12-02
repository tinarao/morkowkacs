using Godot;

public class PlantedVeggie
{
    public int Stage = 1;
    public Vector2I TileCoordinates;
    public Vector2I AtlasCoordinates;
    private const int MAX_STAGE = 4;

    public PlantedVeggie(Vector2I pTileCoordinates, Vector2I pAtlasCoordinates)
    {
        Stage = 1;
        TileCoordinates = pTileCoordinates;
        AtlasCoordinates = pAtlasCoordinates;
    }

    public void UpdateStage()
    {
        if (Stage == MAX_STAGE)
        {
            return;
        }

        Stage += 1;
    }
}