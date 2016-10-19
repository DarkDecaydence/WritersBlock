using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    Tile[,] grid;

    public int sizeX = 5, sizeY = 5;
    public int chunkSizeX = 1, chunkSizeY = 1;

    public int GetTotalSizeX { get { return grid.GetLength(1); } }
    public int GetTotalSizeY { get { return grid.GetLength(0); } }

    public GameObject gridChunk;
    List<GridMesh> gridChunks;

    public Texture2D noiseSource;

    void Awake()
    {
        TileMetrics.noiseSource = noiseSource;
        GameData.grid = this;
    }

    public void CreateGridData()
    {
        createGridData();
    }

    public void CreateGridMesh()
    {
        createGridMesh();
    }

    void createGridData()
    {

        grid = new Tile[sizeX * chunkSizeX, sizeY * chunkSizeY];
        for (int x = 0; x < grid.GetLength(1); x++)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                bool b = System.Convert.ToBoolean(Random.Range(0, 6));

                if (x == 0 || y == 0 || x == grid.GetLength(1) - 1 || y == grid.GetLength(0) - 1)
                    grid[x, y] = new Tile(new Vec2i(x, y), false);
                else
                    grid[x, y] = new Tile(new Vec2i(x, y), b);

            }
        }

        Vec2i exitPos = new Vec2i(GetTotalSizeX - 2, GetTotalSizeY - 2);
        grid[exitPos.x, exitPos.y].setExit(true);
        grid[exitPos.x, exitPos.y].setWalkability(true);
        grid[1, 1].setWalkability(true);

    }

    void createGridMesh()
    {
        clearChildren();
        gridChunks = new List<GridMesh>();
        for (int i = 0; i < chunkSizeX * chunkSizeY; i++)
        {
            GameObject chunk = Instantiate(gridChunk);
            chunk.transform.SetParent(this.transform);
            chunk.transform.name = "chunk" + i;
            gridChunks.Add(chunk.GetComponent<GridMesh>());
        }

        for (int y = 0, i = 0; y < chunkSizeY; y++)
        {
            for (int x = 0; x < chunkSizeY; x++, i++)
            {
                gridChunks[i].triangulizeGrid(grid, x * sizeX, y * sizeY, sizeX, sizeY);
            }
        }
    }

    public Tile getTile(Vec2i pos)
    {
        if(pos.x >= 0 && pos.x < grid.GetLength(1) && pos.y >= 0 && pos.y < grid.GetLength(0))
            return grid[pos.x, pos.y];
        return null;
    }

    void clearChildren()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
