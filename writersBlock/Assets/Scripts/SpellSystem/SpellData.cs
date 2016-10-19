using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum SpellType
{
    Invalid, Ball, Blast, Shield
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
    public int Speed;
    public float Size;

    public SpellData(SpellElement element, SpellType type, int power, int speed, float size)
    {
        this.SpellElement = element;
        this.SpellType = type;
        this.Power = power;
        this.Speed = speed;
        this.Size = size;
    }
}
