using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public static class SpellDatabase {

    public static Dictionary<SpellType, Dictionary<SpellElement, GameObject>> spellDatabase;

	public static void LoadDataBase()
    {
        spellDatabase = new Dictionary<SpellType, Dictionary<SpellElement, GameObject>>();
        List<string> enumValues = EnumUtil.GetValues<SpellType>();
        for (int i = 0; i < enumValues.Count; i++)
        {
            if (EnumUtil.Parse<SpellType>(enumValues[i]) == SpellType.Invalid)
                continue;

            InsertSpellsIntoDatabase(enumValues[i]);
        }    
    }
	
	public static void InsertSpellsIntoDatabase(string typeString)
    {
        SpellType type = EnumUtil.Parse<SpellType>(typeString);
        spellDatabase.Add(type, new Dictionary<SpellElement, GameObject>());
        GameObject[] ObjList = Resources.LoadAll<GameObject>("Spells/" + typeString);
        for (int i = 0; i < ObjList.Length; i++)
        {
            spellDatabase[type].Add(EnumUtil.Parse<SpellElement>(ObjList[i].name), ObjList[i]);
        }

        if(ObjList.Length == 0)
            Debug.LogError("The spell type of \"" + typeString + "\" Does not appear to exist, please update folder structure");

    }

    /// <summary>
    /// Returns the GameObjet of the given type and element if one exist if not it returns null
    /// </summary>
    /// <param name="type"></param>
    /// <param name="element"></param>
    /// <returns></returns>
    public static GameObject GetSpellGameObject(SpellType type, SpellElement element)
    {
        if (spellDatabase.ContainsKey(type) && spellDatabase[type].ContainsKey(element))
            return spellDatabase[type][element];
        else
            return null;
    }
}

public static class EnumUtil
{
    public static List<string> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>().Select(v => v.ToString()).ToList();
    }


    public static T Parse<T>(string s)
    {
        return (T)Enum.Parse(typeof(T), s);
    }
}
