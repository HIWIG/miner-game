using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public PlayerController player;
    public CameraController camera;

    [Header("Tile Atlas")]
    public TileAtlas tileAtlas;


    [Header("Trees and grain")]
    public int treeChance = 15;
    public int grainChance = 3;
    public int minTreeHeight = 3;
    public int maxTreeHeight = 5;

    [Header("Generation settings")]
    public int chunkSize = 20;    
    public int worldSize = 100;
    public int dirtLayerHeight = 5;
    public float heightMultiplier = 20f;
    public int heightAddition = 35;

    private float terrainFreq = 0.06f;
    private float seed;
    private Texture2D noiseTexture;
    private float limit = 0.25f;

    [Header("Mineral settings")]
    public float coalRarity = 0.3f;
    private float coalBlockSize = 0.7f;
    public float ironRarity = 0.5f;
    private float ironBlockSize = 0.75f;
    public float diamondRarity = 0.9f;
    private float diamondBlockSize = 0.8f;
    private Texture2D coalSpread;
    private Texture2D ironSpread;
    private Texture2D diamondSpread;

    private GameObject[] worldChunks;
    private List<Vector2> worldTiles = new List<Vector2>();
    private List<GameObject> worldTilesObjects = new List<GameObject>();

    private void OnValidate()
    {
        SpreadElement();
        GenerateMinerals();
    }

    private void Start()
    {
        //rozprzestrzenienie elementów na mapie
        SpreadElement();

        seed = Random.Range(-10000, 10000);
        //generate minerals:
        GenerateMinerals();
        CreateChunks();
        GenerateTerrain();
    }

    private void GenerateMinerals()
    {
        GenerateNoiseTexture(terrainFreq, limit, noiseTexture);
        GenerateNoiseTexture(coalRarity, coalBlockSize, coalSpread);
        GenerateNoiseTexture(ironRarity, ironBlockSize, ironSpread);
        GenerateNoiseTexture(diamondRarity, diamondBlockSize, diamondSpread);
    }

    private void SpreadElement()
    {

        if (noiseTexture == null)
        {
            noiseTexture = new Texture2D(worldSize, worldSize);
            coalSpread = new Texture2D(worldSize, worldSize);
            ironSpread = new Texture2D(worldSize, worldSize);
            diamondSpread = new Texture2D(worldSize, worldSize);
        }
    }

    public void CreateChunks()
    {
        int numChunks = worldSize / chunkSize;
        worldChunks = new GameObject[numChunks];

        for (int i = 0; i < numChunks; i++)
        {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            newChunk.transform.parent = this.transform;
            worldChunks[i] = newChunk;
        }
    }

    public void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;
            if(x == worldSize/2)
            {
                player.spawPosition = new Vector2(x, height + 2);
                camera.Spawn(new Vector3(player.spawPosition.x, player.spawPosition.y, camera.transform.position.z));
                camera.worldSize = worldSize;
                player.Spawn();
            }
            for (int y = 0; y < height; y++)
            {
                Sprite tileSprite;
                if(y < height - dirtLayerHeight)
                {
                    if (coalSpread.GetPixel(x, y).r > 0.5f)
                    {
                        tileSprite = SetRandomSprite(tileAtlas.coal);
                    }
                    else if (ironSpread.GetPixel(x, y).r > 0.5f)
                    {
                        tileSprite = SetRandomSprite(tileAtlas.iron);
                    }
                    else if (diamondSpread.GetPixel(x, y).r > 0.5f)
                    {
                        tileSprite = SetRandomSprite(tileAtlas.diamond);
                    }
                    else tileSprite = SetRandomSprite(tileAtlas.stone);
                }
                else if (y < height - 1)
                {
                    tileSprite = SetRandomSprite(tileAtlas.dirt);
                }
                else
                {
                    tileSprite = tileAtlas.grass.tileSprite;
                }
                PlaceTile(tileSprite, x, y, false);
                if (y >= height - 1)
                {
                    int t = Random.Range(0, treeChance);
                    if (t == 1)
                    {
                        GenerateTree(x, y + 1);   
                    }
                    t = Random.Range(0, grainChance);
                    if (t == 1)
                    {
                        GenerateGrass(x, y + 1);
                    }
                }
            }
        }
    }

    private Sprite SetRandomSprite(TileClass[] spread)
    {
        int rand = Random.Range(0, spread.Length);
        return spread[rand].tileSprite;
    }

    private void GenerateGrass(int x, int y)
    {
        int t = Random.Range(0, tileAtlas.grain.Length);
        var tileSprite = tileAtlas.grain[t].tileSprite;
        PlaceTile(tileSprite, x, y, true);
    }

    void GenerateTree(int x, int y)
    {
        int treeHeight = Random.Range(minTreeHeight, maxTreeHeight);

        //pien drzewa
        for (int i = 0; i < treeHeight; i++)
        {
            PlaceTile(tileAtlas.log.tileSprite, x, y + i, true);
        }
        //liscie
        for (int i = 0; i < 3; i++)
        {
            PlaceTile(tileAtlas.leaf.tileSprite, x + 1, y + treeHeight + i, true);
            PlaceTile(tileAtlas.leaf.tileSprite, x - 1, y + treeHeight + i, true);
        }
        for (int i = 0; i < 4; i++)
        {
            PlaceTile(tileAtlas.leaf.tileSprite, x, y + treeHeight + i, true);
        }
    }
    public void GenerateNoiseTexture(float frequency, float limit, Texture2D noise)
    {
        for (int x = 0; x < noise.width; x++)
        {
            for (int y = 0; y < noise.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * frequency, (y + seed) * frequency);
                if (v > limit)
                {
                    noise.SetPixel(x, y, Color.white);
                }
                else
                {
                    noise.SetPixel(x, y, Color.black);
                }
            }
        }
        noise.Apply();
    }

    public void PlaceTile(Sprite tileSprite, int x, int y, bool backgroundElement)
    {
        if (!worldTiles.Contains(new Vector2Int(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            GameObject newTile = new GameObject();

            int chunkCoord = Mathf.RoundToInt(Mathf.RoundToInt(x / chunkSize) * chunkSize);
            chunkCoord /= chunkSize;
            newTile.transform.parent = worldChunks[chunkCoord].transform;

            newTile.AddComponent<SpriteRenderer>();
            if (!backgroundElement)
            {
                newTile.AddComponent<BoxCollider2D>();
                newTile.GetComponent<BoxCollider2D>().size = Vector2.one;
                newTile.tag = "Ground";
            }

            newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
            newTile.name = tileSprite.name;
            newTile.transform.position = new Vector3(x + 0.5f, y + 0.5f, -2);

            worldTiles.Add(newTile.transform.position - (Vector3.one * 0.5f));
            worldTilesObjects.Add(newTile);
        }
    }
}
