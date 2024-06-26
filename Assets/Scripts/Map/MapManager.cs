using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager instance;

    private void Awake()
    {
        // Ensure that there is only one instance of MapManager
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Singleton instance getter
    public static MapManager Get { get => instance; }

    [Header("TileMaps")]
    public Tilemap FloorMap;
    public Tilemap ObstacleMap;
    public Tilemap FogMap;

    [Header("Tiles")]
    public TileBase FloorTile;
    public TileBase WallTile;
    public TileBase FogTile;

    [Header("Features")]
    public Dictionary<Vector2Int, Node> Nodes = new Dictionary<Vector2Int, Node>();
    public List<Vector3Int> VisibleTiles;
    public Dictionary<Vector3Int, TileData> Tiles;

    [Header("Map Settings")]
    public int width = 80;
    public int height = 45;
    public int roomMaxSize = 10;
    public int roomMinSize = 6;
    public int maxRooms = 30;
    public int maxEnemies = 2;
    public int maxItems = 2;
    public int floor = 0;

    private void Start()
    {
        // Generate the dungeon at the start
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        // Clear the current floor's data and visuals
        ClearFloor();

        // Initialize Tiles and VisibleTiles collections
        Tiles = new Dictionary<Vector3Int, TileData>();
        VisibleTiles = new List<Vector3Int>();

        // Initialize and configure the dungeon generator
        var generator = new DungeonGenerator();
        generator.SetSize(width, height);
        generator.SetRoomSize(roomMinSize, roomMaxSize);
        generator.SetMaxRooms(maxRooms);
        generator.SetMaxEnemies(maxEnemies);
        generator.SetMaxItems(maxItems); // Set the maximum number of items
        generator.SetCurrentFloor(floor); // Set the current floor
        generator.Generate();

        // Add the generated tiles to the Tiles dictionary
        AddTileMapToDictionary(FloorMap);
        AddTileMapToDictionary(ObstacleMap);

        // Set up the fog of war on the map
        SetupFogMap();
    }

    // Method to handle moving the player up one floor in the dungeon
    public void MoveUp()
    {
        // Clear the current floor's data and visuals
        ClearFloor();

        // Decrement the floor variable to move up one floor
        floor--;

        // Generate the dungeon layout and content for the new floor
        GenerateDungeon();

        // Update the UI text to reflect the current floor number
        UpdateFloorText();
    }

    // Method to handle moving the player down one floor in the dungeon
    public void MoveDown()
    {
        // Clear the current floor's data and visuals
        ClearFloor();

        // Increment the floor variable to move down one floor
        floor++;

        // Generate the dungeon layout and content for the new floor
        GenerateDungeon();

        // Update the UI text to reflect the current floor number
        UpdateFloorText();
    }

    // Method to create an actor at a given position
    public GameObject CreateActor(string name, Vector2 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        actor.name = name;
        return actor;
    }

    // Method to check if a position is within the map bounds
    public bool InBounds(int x, int y) => 0 <= x && x < width && 0 <= y && y < height;

    // Method to check if a position is walkable
    public bool IsWalkable(Vector3 position)
    {
        Vector3Int gridPosition = FloorMap.WorldToCell(position);
        if (!InBounds(gridPosition.x, gridPosition.y) || ObstacleMap.HasTile(gridPosition))
        {
            return false;
        }
        return true;
    }

    // Method to add tiles from a tilemap to the Tiles dictionary
    private void AddTileMapToDictionary(Tilemap tilemap)
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos))
            {
                continue;
            }
            TileData tile = new TileData(
                name: tilemap.GetTile(pos).name,
                isExplored: false,
                isVisible: false
            );

            Tiles.Add(pos, tile);
        }
    }

    // Method to set up the fog of war on the map
    private void SetupFogMap()
    {
        foreach (Vector3Int pos in Tiles.Keys)
        {
            if (!FogMap.HasTile(pos))
            {
                FogMap.SetTile(pos, FogTile);
                FogMap.SetTileFlags(pos, TileFlags.None);
            }

            if (Tiles[pos].IsExplored)
            {
                FogMap.SetColor(pos, new Color(1.0f, 1.0f, 1.0f, 0.5f));
            }
            else
            {
                FogMap.SetColor(pos, Color.white);
            }
        }
    }

    // Method to update the fog of war based on the player's field of view
    public void UpdateFogMap(List<Vector3Int> playerFOV)
    {
        foreach (var pos in VisibleTiles)
        {
            if (!Tiles[pos].IsExplored)
            {
                Tiles[pos].IsExplored = true;
            }

            Tiles[pos].IsVisible = false;
            FogMap.SetColor(pos, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        }

        VisibleTiles.Clear();

        foreach (var pos in playerFOV)
        {
            Tiles[pos].IsVisible = true;
            FogMap.SetColor(pos, Color.clear);
            VisibleTiles.Add(pos);
        }
    }

    // Method to clear the current floor's data and visuals
    private void ClearFloor()
    {
        FloorMap.ClearAllTiles();
        ObstacleMap.ClearAllTiles();
        FogMap.ClearAllTiles();
        Nodes.Clear();
        Tiles.Clear();
        VisibleTiles.Clear();
    }

    // Method to update the UI text to reflect the current floor number
    private void UpdateFloorText()
    {
        // Assuming FloorInfo is a component that handles the UI text for the floor number
        FloorInfo floorInfo = FindObjectOfType<FloorInfo>();
        if (floorInfo != null)
        {
            floorInfo.UpdateFloorText();
        }
    }
}
