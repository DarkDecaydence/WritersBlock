using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SpellTable
{
    private Dictionary<SpellElement, int> elementLevels = new Dictionary<SpellElement, int>();
    private IEnumerable<SpellElement> spellElements
    {
        get { return EnumUtil.GetValues<SpellElement>().Select(s => EnumUtil.Parse<SpellElement>(s)); }
    }

    public SpellTable()
    {
        foreach (SpellElement sElem in spellElements) { elementLevels.Add(sElem, 1); }
    }

    // Does not consider SpellElement.Invalid as a proper element.
    public void IncreaseElementLevel(SpellElement sElem, int levels) { elementLevels[sElem] += levels; }
    public void IncrementElementLevel(SpellElement sElem) { IncreaseElementLevel(sElem, 1); }

    public int GetElementLevel(SpellElement sElem) { return elementLevels[sElem]; }

    public bool IsValidSpell(SpellData sData)
    {
        var elemLvl = GetElementLevel(sData.SpellElement);
        var lang = LanguageFromSize(sData.Size);

        if (sData.SpellType == SpellType.Ball) {
            switch (lang) {
                case IncantationLanguage.English:
                    return true;
                case IncantationLanguage.Latin:
                    return (elemLvl >= 2);
                case IncantationLanguage.Draconic:
                    return (elemLvl >= 3);
            }
        } else if (sData.SpellType == SpellType.Blast) {
            switch (lang) {
                case IncantationLanguage.English:
                    return (elemLvl >= 2);
                case IncantationLanguage.Latin:
                    return (elemLvl >= 3);
                case IncantationLanguage.Draconic:
                    return (elemLvl >= 4);
            }
        }
        return false;
    }

    private IncantationLanguage LanguageFromSize(float size)
    {
        if (size <= 0.26f) {
            return IncantationLanguage.English;
        } else if (size <= 0.6f) {
            return IncantationLanguage.Latin;
        } else if (size <= 1.1f) {
            return IncantationLanguage.Draconic;
        } else {
            return IncantationLanguage.Invalid;
        }
    }
}
