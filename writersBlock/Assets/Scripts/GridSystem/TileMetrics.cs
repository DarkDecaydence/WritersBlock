﻿using UnityEngine;
using System.Collections;

public static class TileMetrics {

    public static float tileLength = 1f;

    public static float tileHalfLength = tileLength * 0.5f;

    public static float gridHeight = 0f;

    public static float wallHeight = 1f;

    public static float wallBaseHeight = 0.2f;

    public static float wallWidth = 0.8f;

    public static float innerWallRadius = wallWidth / 2f;

    public static float innterWallHalfRadius = innerWallRadius / 2f;

    public static Texture2D noiseSource;

    public const float tilePerturbStrength = 0.1f;

    public const float noiseScale = 0.003f;

    public static Vector4 SampleNoise(Vector3 position)
    {
        return noiseSource.GetPixelBilinear(
            position.x * noiseScale,
            position.z * noiseScale
        );
    }



}
