using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMesh : MonoBehaviour {

    Mesh mesh;

    void Awake()
    {

        mesh = new Mesh();
        mesh.name = "gridChunk";

    }

    List<Vector3> vertices;
    List<int> indices;
    List<Vector3> uv;

    public void triangulizeGrid(Tile[,] grid, int startX, int startY, int width, int height)
    {

        vertices = new List<Vector3>();
        indices = new List<int>();
        uv = new List<Vector3>();

        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                triangulizeTile(grid[x, y]);
            }
        }

        Debug.Log(vertices.Count);
        mesh.SetVertices(vertices);
        mesh.SetTriangles(indices, 0);
        mesh.SetUVs(0, uv);

        mesh.RecalculateNormals();

        gameObject.GetComponent<MeshFilter>().mesh = mesh;

    }

    void triangulizeTile(Tile t)
    {

        if (!t.isWalkAble())
            return;

        float px = t.pos.x * TileMetrics.tileLength;
        float py = t.pos.y * TileMetrics.tileLength;

        Vector3 v1 = new Vector3(px + TileMetrics.tileHalfLength, TileMetrics.gridHeight, py - TileMetrics.tileHalfLength);
        Vector3 v2 = new Vector3(px - TileMetrics.tileHalfLength, TileMetrics.gridHeight, py - TileMetrics.tileHalfLength);
        Vector3 v3 = new Vector3(px + TileMetrics.tileHalfLength, TileMetrics.gridHeight, py + TileMetrics.tileHalfLength);
        Vector3 v4 = new Vector3(px - TileMetrics.tileHalfLength, TileMetrics.gridHeight, py + TileMetrics.tileHalfLength);

        addQuad(v2, v1, v4, v3);
        AddQuadUV(new Vector2(1,1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0));

    }

    void addQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int indiceCount = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        indices.Add(indiceCount);
        indices.Add(indiceCount + 2);
        indices.Add(indiceCount + 1);
        indices.Add(indiceCount + 1);
        indices.Add(indiceCount + 2);
        indices.Add(indiceCount + 3);
    }

    void addTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int indiceCount = indices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        indices.Add(indiceCount);
        indices.Add(indiceCount + 1);
        indices.Add(indiceCount + 2);
    }

    public void AddTriangleUV(Vector2 uv1, Vector2 uv2, Vector3 uv3)
    {
        uv.Add(uv1);
        uv.Add(uv2);
        uv.Add(uv3);
    }

    public void AddQuadUV(Vector2 uv1, Vector2 uv2, Vector3 uv3, Vector3 uv4)
    {
        uv.Add(uv1);
        uv.Add(uv2);
        uv.Add(uv3);
        uv.Add(uv4);
    }
}
