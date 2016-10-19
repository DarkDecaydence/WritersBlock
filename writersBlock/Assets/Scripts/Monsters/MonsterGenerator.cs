using UnityEngine;
using System.Collections.Generic;

public class MonsterGenerator : MonoBehaviour {

    public GameObject[] monsterPrefabs;
    List<Monster> monsters;

    void Awake()
    {

        GameData.monsterGenerator = this;

    }

    public void GenerateMonsters(int num)
    {
        GenerateMonstersAtRandom(num);
        MonsterAggroCheck();
    }

    private void GenerateMonstersAtRandom(int v)
    {
        clearChildren();
        monsters = new List<Monster>();
        int spawned = 0;

        while(spawned < v)
        {
            int x = Random.Range(0, GameData.grid.GetTotalSizeX);
            int y = Random.Range(0, GameData.grid.GetTotalSizeY);
            Tile t = GameData.grid.getTile(new Vec2i(x, y));
            if ( t != null && t.isWalkAble() && !t.isTileOccupied())
            {
                spawnMonster(0, new Vec2i(x, y));
                spawned++;
            }
        }

    }

    void spawnMonster(int index, Vec2i pos)
    {

        GameObject monster = Instantiate(monsterPrefabs[0]);
        monster.transform.SetParent(this.transform);
        Monster monsterScript = monster.GetComponent<Monster>();
        monsters.Add(monsterScript);
        monsterScript.Init(pos);

    }

    public void MonsterAggroCheck()
    {
        for(int i = 0; i < monsters.Count; i++)
        {
            monsterDistCheck( monsters[i] );
        }

    }

    private void monsterDistCheck(Monster m)
    {
        if( m.state == Monster.State.Patroling &&
            m.aggroRange > Vector3.Distance(m.GetPosition().ToVec3(), GameData.playerCharacter.Pos.ToVec3()))
        {
            m.aggro();
        }
    }

    public void killMonster(Monster m)
    {
        monsters.Remove(m);
        if(monsters.Count <= 0)
        {
            win();
        }
    }

    void win()
    {
        Debug.Log("You win, wha? you want a medal?");
    }

    void clearChildren()
    {
        foreach (Transform child in transform)
        {
            HealthScript health = child.transform.GetComponent<HealthScript>();

            if(health != null)
                health.killObj();
        }
    }

}
