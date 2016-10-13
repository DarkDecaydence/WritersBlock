using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum IncantationLanguage
{
    Invalid, English, Latin, Draconic
}

public class IncantationBuilder
{
    #region Static fields
    #region Spell elements
    // All elements are listed in enumration order.
    public static List<string> EnglishElements = new List<string>() {
        "arcane", "earth", "water", "fire", "air", "void"
    };

    public static List<string> LatinElements = new List<string>() {
        "procul", "terra", "aqua", "ignis", "aether", "vacuum"
    };

    public static List<string> DraconicElements = new List<string>() {
        "suiaerl", "edar", "pab", "ixen", "thrae", "amuul"
    };
    #endregion

    public static IEnumerable<string> AllElements { get { return EnglishElements.Concat(LatinElements.Concat(DraconicElements)); } }

    #region Spell types
    public static List<string> EnglishTypes = new List<string>() {

    };

    public static List<string> LatinTypes = new List<string>() {

    };

    public static List<string> DraconicTypes = new List<string>() {
        // Draconic words for "Line" and "Nova" doesn't exist.
        // Words here are "Ball", "Wall" and "Shield" 
        "garmth", "dos", "fethos"
    };

    public static IEnumerable<string> AllTypes { get { return EnglishTypes.Concat(LatinTypes.Concat(DraconicTypes)); } }
    #endregion

    #endregion

    public int ChargeLevel { get; private set; }

    private readonly List<IncantationLanguage> attemptedLanguages = new List<IncantationLanguage>();
    public bool IsValidLanguage
    {
        get {
            return attemptedLanguages.Aggregate((currentGuess, next) => currentGuess == next ? currentGuess : IncantationLanguage.Invalid) != IncantationLanguage.Invalid;
        }
    }

    private readonly List<SpellElement> attemptedElements = new List<SpellElement>();
    public bool HasValidElement { get { return attemptedElements.Count == 1; } }

    private readonly List<SpellType> attemptedTypes = new List<SpellType>();
    public bool HasValidType { get { return attemptedTypes.Count == 1; } }

    public readonly List<string> Ramblings = new List<string>();
    public bool IsRambling { get { return Ramblings.Count > 0; } }

    // Should be used for more complex calculation of spell power.
    // Add if time allows.
    //public float Precision { get; private set; }

    public void Expand(string input)
    {
        foreach (string s in SerializeIncantation(input)) {
            if (AllElements.Contains(s)) { 
                if (EnglishElements.Contains(s)) {
                    attemptedElements.Add(ToElement(s));
                    attemptedLanguages.Add(IncantationLanguage.English);
                }
                else if (LatinElements.Contains(s)) {
                    attemptedElements.Add(ToElement(s));
                    attemptedLanguages.Add(IncantationLanguage.Latin);
                }
                else {
                    attemptedElements.Add(ToElement(s));
                    attemptedLanguages.Add(IncantationLanguage.Draconic);
                }
            }
            else if (AllTypes.Contains(s)) {

            }
            else {
                Ramblings.Add(s);
            }
        }
    }

    public Incantation ToIncantation()
    {
        return null;
    }

    #region Private helper methods
    private SpellElement ToElement(string s)
    {
        switch (s) {
            case "arcane":
            case "procul":
            case "suiaerl":
                return SpellElement.Arcane;
            case "earth":
            case "terra":
            case "edar":
                return SpellElement.Earth;
            case "water":
            case "aqua":
            case "pab":
                return SpellElement.Water;
            case "fire":
            case "ignis":
            case "ixen":
                return SpellElement.Fire;
            case "air":
            case "aether":
            case "thrae":
                return SpellElement.Air;
            case "void":
            case "vacuum":
            case "amuul":
                return SpellElement.Void;
            default:
                return SpellElement.Invalid;
        }
    }

    private SpellType ToType(string s)
    {
        return SpellType.Invalid;
    }

    private string[] SerializeIncantation(string rawIncantation)
    {
        return rawIncantation
            .Split(' ')
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => s.ToLower())
            .ToArray();
    }

    private void UpdateSpellLanguage()
    {

    }
    #endregion
}
