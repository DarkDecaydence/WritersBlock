using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SpellDatabase : MonoBehaviour {

    Dictionary<SpellType, Dictionary<SpellElement, GameObject>> spellDatabase;

	void Awake()
    {

        

    }
	
	public void insertSpellsIntoDatabase(string type)
    {
        GameObject[] list = Resources.LoadAll<GameObject>("Spells/Ball/");
        for (int i = 0; i < list.Length; i++)
        {

        }
    }
}
