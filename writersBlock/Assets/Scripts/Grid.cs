using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    Tile[,] grid;

    public int sizeX = 5, sizeY = 5;
    public int chunkSizeX = 1, chunkSizeY = 1;

    public GameObject gridChunk;
    List<GridMesh> gridChunks;

    void Start()
    {
        GameData.grid = this;
        grid = new Tile[sizeX * chunkSizeX, sizeY * chunkSizeY];

        createGridMesh();
    }

    void createGridMesh()
    {
        for (int x = 0; x < grid.GetLength(1); x++)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                bool b = System.Convert.ToBoolean(Random.Range(0, 6));
                grid[x, y] = new Tile(new Vec2i(x, y), b);
            }
        }

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
                Debug.Log("Triangulazing at " + x * sizeX + ", " + y * sizeY);
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
}
