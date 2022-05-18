using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public PlayerController player;
    public CameraController cameraController;

    [Header("Tile Atlas")]
    public TileAtlas tileAtlas;


    [Header("Trees and grain")]
    public int treeChance = 15;
    public int grainChance = 3;
    public int minTreeHeight = 3;
    public int maxTreeHeight = 5;

    [Header("Generation settings")]    
    public int worldSize = 100;
    public int dirtLayerHeight = 5;
    public float heightMultiplier = 20f;
    public int heightAddition = 35;

    private float terrainFreq = 0.06f;
    private float seed;
    public Texture2D noiseTexture;
    private float limit = 0.25f;

    [Header("Mineral settings")]
    public float coalRarity = 0.3f;
    private float coalBlockSize = 0.7f;
    public float ironRarity = 0.5f;
    private float ironBlockSize = 0.75f;
    public float diamondRarity = 0.9f;
    private float diamondBlockSize = 0.8f;
    public Texture2D coalSpread;
    public Texture2D ironSpread;
    public Texture2D diamondSpread;

    [Header("Cabin settings")]
    public int cabinLength = 16;
    private float cabinAtHeigth;
    private float cabinHeigth = 4;
    public Texture2D cabinSpread;
    public bool insideCabin;

    public List<Vector2> worldTiles = new List<Vector2>();
    public List<GameObject> worldTilesObjects = new List<GameObject>();
    public List<TileClass> worldTileClasses = new List<TileClass>();

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
        GenerateTerrain();
    }
    private void Update()
    {
        HideCabinOutside();
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

   
    public void GenerateTerrain()
    {

        bool cabinHeightSet = false;
        cabinSpread = new Texture2D(worldSize, worldSize);

        float height = Mathf.PerlinNoise((0 + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;
        GenerateCabinTexture(cabinLength, cabinSpread);
        for (int x = 0; x < worldSize; x++)
        {
            if (x == worldSize/2)
            {
                player.spawPosition = new Vector2(x, height + 2);
                cameraController.Spawn(new Vector3(player.spawPosition.x, player.spawPosition.y, cameraController.transform.position.z));
                cameraController.worldSize = worldSize;
                player.Spawn();
            }
            for (int y = 0; y < height; y++)
            {
                TileClass tileClass;
                if(y < height - dirtLayerHeight)
                {
                    if (coalSpread.GetPixel(x, y).r > 0.5f)
                    {
                        tileClass = tileAtlas.coal;
                    }
                    else if (ironSpread.GetPixel(x, y).r > 0.5f)
                    {
                        tileClass = tileAtlas.iron;
                    }
                    else if (diamondSpread.GetPixel(x, y).r > 0.5f)
                    {
                        tileClass = tileAtlas.diamond;
                    }
                    else tileClass = tileAtlas.stone;
                }
                else if (y < height - 1)
                {
                    tileClass = tileAtlas.dirt;
                }
                else
                {
                    tileClass = tileAtlas.grass;
                }
                PlaceTile(tileClass, x, y, false, 1);
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
            if(x > worldSize/2 & x < (worldSize/2) + cabinLength)
            {
                if(!cabinHeightSet)
                {
                    cabinAtHeigth = height;
                    cabinHeightSet = true;
                    Debug.Log(cabinAtHeigth);
                }
                TileClass tileClassOutside;
                TileClass tileClassInside;
                if(cabinSpread.GetPixel(x, (int)cabinAtHeigth ).r > 0.5f)
                {
                    for (int i = 0; i < cabinHeigth; i++)
                    {
                        if ((cabinSpread.GetPixel(x - 1, (int)cabinAtHeigth).r > 0.5f) ^ (cabinSpread.GetPixel(x + 1, (int)cabinAtHeigth).r > 0.5f)) {
                            
                            tileClassOutside = tileAtlas.cabinWalls;
                            tileClassInside = tileAtlas.cabinWallsInside;
                        }
                        else
                        {
                            if(i == cabinHeigth/2 - 1)
                            {
                                tileClassOutside = tileAtlas.cabinWindow;
                                tileClassInside = tileAtlas.cabinWindow;
                            }
                            else
                            {
                                tileClassOutside = tileAtlas.cabin;
                                tileClassInside = tileAtlas.cabinInside;
                            }
                        }
                        PlaceTile(tileClassOutside, x, (int)cabinAtHeigth + 1 + i, true, -2);
                        PlaceTile(tileClassInside, x, (int)cabinAtHeigth + 1 + i, true, -5);
                    }
                }

                height = cabinAtHeigth;
            }
            else
            {

                height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;
            }
        }
    }

    private void GenerateGrass(int x, int y)
    {
        PlaceTile(tileAtlas.grain, x, y, true, 0);
    }

    void GenerateTree(int x, int y)
    {
        int treeHeight = Random.Range(minTreeHeight, maxTreeHeight);

        //pien drzewa
        for (int i = 0; i < treeHeight; i++)
        {
            PlaceTile(tileAtlas.log, x, y + i, true, 0);
        }
        //liscie
        for (int i = 0; i < 3; i++)
        {
            PlaceTile(tileAtlas.leaf, x + 1, y + treeHeight + i, true, 0);
            PlaceTile(tileAtlas.leaf, x - 1, y + treeHeight + i, true, 0);
        }
        for (int i = 0; i < 4; i++)
        {
            PlaceTile(tileAtlas.leaf, x, y + treeHeight + i, true, 0);
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
    public void GenerateCabinTexture(int cabinLength, Texture2D cabin)
    {
        for (int x = 0; x < cabin.width; x++)
        {
            for (int y = 0; y < cabin.height; y++)
            {
                if(x > cabin.width/2 & x < (cabin.width/2) + cabinLength)
                {
                    cabin.SetPixel(x, y, Color.white);
                }
                else
                {
                    cabin.SetPixel(x, y, Color.black);
                }
            }
        }
        cabin.Apply();
    }

    public void RemoveTile(int x, int y)
    {
        if (worldTiles.Contains(new Vector2Int(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            int index = worldTiles.IndexOf(new Vector2(x, y));
            Debug.Log(index);
            Destroy(worldTilesObjects[index]);
            worldTilesObjects.RemoveAt(index);
            worldTiles.RemoveAt(index);
            worldTileClasses.RemoveAt(index);
        }
    }

    public void PlaceTile(TileClass tile, int x, int y, bool backgroundElement, int sortingOrder)
    {

            GameObject newTile = new GameObject();

            //int chunkCoord = Mathf.RoundToInt(Mathf.RoundToInt(x / chunkSize) * chunkSize);
            //chunkCoord /= chunkSize;
            //newTile.transform.parent = worldChunks[chunkCoord].transform;

            newTile.AddComponent<SpriteRenderer>();
            if (!backgroundElement)
            {
                newTile.AddComponent<BoxCollider2D>();
                newTile.GetComponent<BoxCollider2D>().size = Vector2.one;
                newTile.tag = "Ground";
            }

            int rand = Random.Range(0, tile.tileSprite.Length);
            var tileSprite = tile.tileSprite[rand];

            newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
            newTile.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            newTile.name = tile.tileName;
            newTile.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
            

            worldTiles.Add(newTile.transform.position - (Vector3.one * 0.5f));
            worldTilesObjects.Add(newTile);
            worldTileClasses.Add(tile);
        
    }

    public void InsideCabin(Vector2 playerPosition)
    {
        if (cabinSpread.GetPixel((int)playerPosition.x, (int)playerPosition.y).r > 0.5f & playerPosition.y >= cabinAtHeigth)
        {
            var tileIndex = worldTiles.IndexOf(new Vector2((int)playerPosition.x, (int)playerPosition.y));
            insideCabin = true;
        }
        else
        {
            insideCabin = false;
        }
    }
    public void HideCabinOutside()
    {
        if(insideCabin)
        {
            foreach (var tile in worldTilesObjects)
            {
                if(tile.name.Contains("Inside"))
                {
                    tile.GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
            }
        }
        else
        {
            foreach (var tile in worldTilesObjects)
            {
                if (tile.name.Contains("Inside"))
                {
                    tile.GetComponent<SpriteRenderer>().sortingOrder = -5;
                }
            }
        }
    }
}
    