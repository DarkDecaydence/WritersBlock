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
        "ball", "blast", "wall", "shield"
    };

    public static List<string> LatinTypes = new List<string>() {
        "globus", "flatus", "murus", "arma"
    };

    public static List<string> DraconicTypes = new List<string>() {
        // Draconic words for "Line" and "Nova" doesn't exist.
        // Words here are "Ball", "Blast", "Wall" and "Shield" 
        "garmth", "drevab", "dos", "fethos"
    };
    #endregion

    public static IEnumerable<string> AllTypes { get { return EnglishTypes.Concat(LatinTypes.Concat(DraconicTypes)); } }

    #endregion

    // Expression describing how charged the spell is.
    // Implement if time allows it.
    //public int ChargeLevel { get; private set; }

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
    // NOTE: Recursive architecture is better suited for this.
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
                if (EnglishTypes.Contains(s)) {
                    attemptedTypes.Add(ToType(s));
                    attemptedLanguages.Add(IncantationLanguage.English);
                }
                else if (LatinTypes.Contains(s)) {
                    attemptedTypes.Add(ToType(s));
                    attemptedLanguages.Add(IncantationLanguage.Latin);
                }
                else {
                    attemptedTypes.Add(ToType(s));
                    attemptedLanguages.Add(IncantationLanguage.Draconic);
                }
            }
            else {
                Ramblings.Add(s);
            }
        }
    }

    public Incantation ToIncantation()
    {
        // Checks whether the incantation is correctly constructed.
        if (IsRambling || !HasValidElement || !HasValidType || !IsValidLanguage)
            return Incantation.MisfireIncantation;

        SpellData incantationData = new SpellData(attemptedElements.FirstOrDefault(), attemptedTypes.FirstOrDefault(), PowerFromLanguage(attemptedLanguages.FirstOrDefault()));
        return new Incantation(incantationData);
    }

    public override string ToString()
    {
        if (IsRambling)
            return "Rambling Incantation";

        if (!IsValidLanguage)
            return "Mispronounced Incantation";

        if (!HasValidElement)
            return "Multi-Elemental Incantation";

        if (!HasValidType)
            return "Mistyped Incantation";
        
        string element, type, language;
        switch (attemptedLanguages.First()) {
            case IncantationLanguage.English:
                language = "english"; break;
            case IncantationLanguage.Latin:
                language = "latin"; break;
            case IncantationLanguage.Draconic:
                language = "draconic"; break;
            default: throw new Exception("Enum had unidentified type or 'Invalid' when impossible; Correct switch cases.");

        }
        switch (attemptedElements.First()) {
            case SpellElement.Arcane:   element = "arcane"; break;
            case SpellElement.Earth:    element = "earth"; break;
            case SpellElement.Water:    element = "water"; break;
            case SpellElement.Fire:     element = "fire"; break;
            case SpellElement.Air:      element = "air"; break;
            case SpellElement.Void:     element = "void"; break;
            default: throw new Exception("Enum had unidentified type or 'Invalid' when impossible; Correct switch cases.");
        }
        switch (attemptedTypes.First()) {
            case SpellType.Ball:    type = "ball"; break;
            case SpellType.Blast:   type = "blast"; break;
            case SpellType.Wall:    type = "wall"; break;
            case SpellType.Shield:  type = "shield"; break;
            default: throw new Exception("Enum had unidentified type or 'Invalid' when impossible; Correct switch cases.");
        }

        return string.Format("({0}) {1}{2}", language, element, type);
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
                UnityEngine.Debug.Log("Didn't recognize SpellElement. Coercing to 'Invalid'");
                return SpellElement.Invalid;
        }
    }

    private SpellType ToType(string s)
    {
        switch (s) {
            case "ball":
            case "globus":
            case "garmth":
                return SpellType.Ball;
            case "blast":
            case "flatus":
            case "drevab":
                return SpellType.Blast;
            case "wall":
            case "murus":
            case "dos":
                return SpellType.Wall;
            case "shield":
            case "arma":
            case "fethos":
                return SpellType.Shield;
            default:
                UnityEngine.Debug.Log("Didn't recognize SpellType. Coercing to 'Invalid'");
                return SpellType.Invalid;
        }
    }

    private int PowerFromLanguage(IncantationLanguage l)
    {
        switch (l) {
            case IncantationLanguage.English:
                return 2;
            case IncantationLanguage.Latin:
                return 4;
            case IncantationLanguage.Draconic:
                return 7;
            default:
                return 0;
        }
    }

    private string[] SerializeIncantation(string rawIncantation)
    {
        return rawIncantation
            .Split(' ')
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => s.ToLower())
            .ToArray();
    }
    #endregion
}
