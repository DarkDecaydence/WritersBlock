using UnityEngine;

public class Incantation : ScriptableObject
{
    public static Incantation MisfireIncantation
    {
        get
        {
            var defaultSpellData = new SpellData(SpellElement.Invalid, SpellType.Invalid, 0);
            return new Incantation(defaultSpellData);
        }
    }

    private SpellData data;

    public Incantation(SpellData data)
    {
        this.data = data;
    }
}
