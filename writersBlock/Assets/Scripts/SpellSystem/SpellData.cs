using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum SpellType
{
    Invalid, Ball, Line, Wall, Shield, Nova
}

public enum SpellElement
{
    Invalid, Arcane, Earth, Water, Fire, Air, Void
}

public struct SpellData
{
    public SpellElement SpellElement;
    public SpellType SpellType;
    public int Power;

    public SpellData(SpellElement element, SpellType type, int Power)
    {
        this.SpellElement = element;
        this.SpellType = type;
        this.Power = Power;
    }
}
