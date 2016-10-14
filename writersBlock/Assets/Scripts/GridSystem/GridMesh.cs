using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMesh : MonoBehaviour {

    Mesh mesh;

    void Awake()
    {

        mesh = new Mesh();
        mesh.name = "gridChunk";
        mesh.subMeshCount = 3;

    }

    List<Vector3> vertices;

    //Floor == 0 || Wall == 1
    List<int>[] indices;
    List<Vector3> uv;

    public void triangulizeGrid(Tile[,] grid, int startX, int startY, int width, int height)
    {

        vertices = new List<Vector3>();
        indices = new List<int>[3];
        indices[0] = new List<int>();
        indices[1] = new List<int>();
        indices[2] = new List<int>();

        uv = new List<Vector3>();

        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                triangulizeTile(grid[x, y]);
            }
        }

        mesh.SetVertices(vertices);

        mesh.SetTriangles(indices[0], 0);
        mesh.SetTriangles(indices[1], 1);
        mesh.SetTriangles(indices[2], 2);
        mesh.SetUVs(0, uv);

        mesh.RecalculateNormals();

        gameObject.GetComponent<MeshFilter>().mesh = mesh;

    }

    void triangulizeTile(Tile t)
    {

        if (!t.isWalkAble())
        {
            triangulateWall(t);
        }else
        {
            triangulateFloor(t);
        }
           
    }

    void triangulateFloor(Tile t)
    {
        float px = t.pos.x * TileMetrics.tileLength;
        float py = t.pos.y * TileMetrics.tileLength;

        Vector3 v1 = new Vector3(px + TileMetrics.tileHalfLength, TileMetrics.gridHeight, py - TileMetrics.tileHalfLength);
        Vector3 v2 = new Vector3(px - TileMetrics.tileHalfLength, TileMetrics.gridHeight, py - TileMetrics.tileHalfLength);
        Vector3 v3 = new Vector3(px + TileMetrics.tileHalfLength, TileMetrics.gridHeight, py + TileMetrics.tileHalfLength);
        Vector3 v4 = new Vector3(px - TileMetrics.tileHalfLength, TileMetrics.gridHeight, py + TileMetrics.tileHalfLength);

        addQuad(v2, v1, v4, v3, 0);
        AddQuadUV(new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0));
    }

    void triangulateWall(Tile t)
    {

        float px = t.pos.x * TileMetrics.tileLength;
        float py = t.pos.y * TileMetrics.tileLength;

        float wallOffset = TileMetrics.tileHalfLength * TileMetrics.wallWidth;
        float wallHeight = TileMetrics.gridHeight + TileMetrics.wallHeight;

        Vector3 v1 = new Vector3(px + wallOffset, wallHeight, py - wallOffset);
        Vector3 v2 = new Vector3(px - wallOffset, wallHeight, py - wallOffset);
        Vector3 v3 = new Vector3(px + wallOffset, wallHeight, py + wallOffset);
        Vector3 v4 = new Vector3(px - wallOffset, wallHeight, py + wallOffset);

        addQuad(v2, v1, v4, v3, 2);
        AddQuadUV(new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0));

        Vector3 v5 = new Vector3(px + TileMetrics.tileHalfLength, TileMetrics.gridHeight, py - TileMetrics.tileHalfLength);
        Vector3 v6 = new Vector3(px - TileMetrics.tileHalfLength, TileMetrics.gridHeight, py - TileMetrics.tileHalfLength);
        Vector3 v7 = new Vector3(px + TileMetrics.tileHalfLength, TileMetrics.gridHeight, py + TileMetrics.tileHalfLength);
        Vector3 v8 = new Vector3(px - TileMetrics.tileHalfLength, TileMetrics.gridHeight, py + TileMetrics.tileHalfLength); 

        float wallBaseOffset = wallHeight - (TileMetrics.wallHeight - TileMetrics.wallBaseHeight);

        Vector3 v1d = new Vector3(v1.x, wallBaseOffset, v1.z);
        Vector3 v2d = new Vector3(v2.x, wallBaseOffset, v2.z);
        Vector3 v3d = new Vector3(v3.x, wallBaseOffset, v3.z);
        Vector3 v4d = new Vector3(v4.x, wallBaseOffset, v4.z);

        Vector3 v15x = new Vector3(px - TileMetrics.tileLength, TileMetrics.gridHeight, py + TileMetrics.innerWallTopWidth);
        Vector3 v37x = new Vector3(px - TileMetrics.tileLength, TileMetrics.gridHeight, py - TileMetrics.innerWallTopWidth);

        Vector3 v15y = new Vector3(px, TileMetrics.gridHeight, py);
        Vector3 v26y = new Vector3(px, TileMetrics.gridHeight, py);

        Vector3 v26x = new Vector3(px, TileMetrics.gridHeight, py);
        Vector3 v48x = new Vector3(px, TileMetrics.gridHeight, py);

        Vector3 v37y = new Vector3(px, TileMetrics.gridHeight, py);
        Vector3 v48y = new Vector3(px, TileMetrics.gridHeight, py);

        //, new Vec2i(0, -1)
        triangulizeWallside(v1, v3, v1d, v3d);
        triangulizeBase(v7, v5, v3d, v1d);

        //, new Vec2i(0, 1)
        triangulizeWallside(v4, v2, v4d, v2d);
        triangulizeBase(v6, v8, v2d, v4d);

        //, new Vec2i(1, 0)
        triangulizeWallside(v2, v1, v2d, v1d);
        triangulizeBase(v5, v6, v1d, v2d);

        //, new Vec2i(-1, 0)
        triangulizeWallside(v3, v4, v3d, v4d);
        triangulizeBase(v8, v7, v4d, v3d);

    }

    void triangulizeWallside(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        addQuad(v2, v1, v4, v3, 1);
        AddQuadUV(new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 0), new Vector2(1, 0));
    }

    void triangulizeBase(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {

        addQuad(v2, v1, v4, v3, 1);
       
        AddQuadUV(new Vector2(0, 0.2f), new Vector2(1, 0.2f), new Vector2(0, 0), new Vector2(1, 0));
    }

    void addQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, int listIndex)
    {
        int indiceCount = vertices.Count;
        vertices.Add(Perturb(v1));
        vertices.Add(Perturb(v2));
        vertices.Add(Perturb(v3));
        vertices.Add(Perturb(v4));
        indices[listIndex].Add(indiceCount);
        indices[listIndex].Add(indiceCount + 2);
        indices[listIndex].Add(indiceCount + 1);
        indices[listIndex].Add(indiceCount + 1);
        indices[listIndex].Add(indiceCount + 2);
        indices[listIndex].Add(indiceCount + 3);
    }

    void addTriangle(Vector3 v1, Vector3 v2, Vector3 v3, int listIndex)
    {
        int indiceCount = vertices.Count;
        vertices.Add(Perturb(v1));
        vertices.Add(Perturb(v2));
        vertices.Add(Perturb(v3));
        indices[listIndex].Add(indiceCount);
        indices[listIndex].Add(indiceCount + 1);
        indices[listIndex].Add(indiceCount + 2);
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

    Vector3 Perturb(Vector3 position)
    {
        Vector4 sample = TileMetrics.SampleNoise(position);
        position.x += (sample.x * 2f - 1f) * TileMetrics.tilePerturbStrength;
        position.y += (sample.y * 2f - 1f) * TileMetrics.tilePerturbStrength;
        position.z += (sample.z * 2f - 1f) * TileMetrics.tilePerturbStrength;
        return position;
    }
}
