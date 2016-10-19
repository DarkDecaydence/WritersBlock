using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    public Text text;
    public FadeScript levelFade;

    public FadeScript blackScreen;

    int lvl = 0;

    readonly float[] enemyCount = { 1, 1 };
    readonly int[] mapsize      = { 1, 1 };
    readonly int[] monsterDiff  = { 1, 1 };

    void Awake()
    {
        GameData.levelManager = this;
    }

    void Start()
    {
        loadLevel(0);
    }

    public void advanceLevel()
    {
        loadLevel(lvl + 1);
    }

    public void loadLevel(int lvlNum)
    {

        lvl = lvlNum;
        text.text = "Level " + lvl;

        blackScreen.startFadeIn();

        GameData.grid.CreateGridData();
        while (!validateGrid())
            GameData.grid.CreateGridData();

        GameData.grid.CreateGridMesh();
        GameData.playerCharacter.setPostion(new Vec2i(1, 1));
        GameData.monsterGenerator.GenerateMonsters(5);

        blackScreen.startFadeOut();

        levelFade.startFade();

    }

    bool validateGrid()
    {

        List<Vec2i> path = GameData.aStar.FindShortestPath(new Vec2i(1, 1), new Vec2i(GameData.grid.GetTotalSizeX - 2, GameData.grid.GetTotalSizeY - 2));

        if (path != null)
            return true;
        else
            return false;

    }


}
