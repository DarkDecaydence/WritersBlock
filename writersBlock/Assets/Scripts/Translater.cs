using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Translater : MonoBehaviour
{
    void Start()
    {
        SpellDatabase.LoadDataBase();
    }

    public Message interpretMessage(string msg)
    {

        msg = msg.ToLower();

        string[] partMessages = msg.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();

        string command = partMessages[0];

        switch (command) {
            case "m":
            case "move":
                return interpretMove(partMessages);
            case "attack":
                return interpretAttack(partMessages);
            case "cast":
                return interpretSpell(partMessages);
            default:
                return new Message(false, "You tripped on a smell pebble!");
        }
    }

    private Message interpretAttack(string[] partMessages)
    {
        return new Message(false, "Attack not implemented yet");
    }

    private Message interpretMove(string[] partMessages)
    {
        if (partMessages.Length <= 1)
            return new Message(false, "Specify a direction you dumbnut");

        string direction = partMessages[1];
        bool moveValidity = false;
        try { 
            moveValidity = GameData.playerCharacter.move(translateDirection(direction));
        } catch (InvalidDirectionException ex) {
            return new Message(false, ex.Message);
        }

        return new Message(moveValidity, "You slammed your face into the wall!");
    }

    private Message interpretSpell(string[] partMessages)
    {
        if (partMessages.Length < 2)
            return new Message(false, "Specify a direction!");
        else if (partMessages.Length < 3)
            return new Message(false, "Specify an incantation!");

        Vec2i direction;
        try {
            direction = translateDirection(partMessages.Last()); 
        } catch (InvalidDirectionException ex) {
            return new Message(false, ex.Message);
        }

        var restIncantation = partMessages.Skip(1);
        string incantation = string.Join(" ", restIncantation.Take(restIncantation.Count() - 1).ToArray());

        IncantationBuilder ib = new IncantationBuilder();
        ib.Expand(incantation);

        var isValid = ib.HasValidElement && ib.HasValidType && ib.IsValidLanguage && !ib.IsRambling;
        if (isValid) {
            var data = ib.ToIncantation();
            var spellCast = SpellDatabase.GetSpellGameObject(data.SpellType, data.SpellElement);
            Incantation.SpawnIncantation(GameData.playerCharacter.gameObject, spellCast, data, new Vector2(direction.x, direction.y));
            //switch (data.SpellType) {
            //    case SpellType.Ball:
            //        Incantation.SpawnIncantation(GameData.playerCharacter.gameObject, spellCast, data, new Vector2(direction.x, direction.y));
            //        break;
            //    case SpellType.Blast:
            //        BurstSpell.SpawnIncantation(GameData.playerCharacter.gameObject, spellCast, data, new Vector2(direction.x, direction.y));
            //        break;
            //    default:
            //        Debug.Log("*fart*");
            //        break;
            //}
        }

        return new Message(isValid, ib.ToString());
    }

    private Vec2i translateDirection(string dir)
    {
        var dirVec = new Vec2i();
        switch (dir) {
            case "l":
            case "left":
                dirVec = new Vec2i(-1, 0); break;
            case "r":
            case "right":
                dirVec = new Vec2i(1, 0); break;
            case "u":
            case "up":
                dirVec = new Vec2i(0, 1); break;
            case "d":
            case "down":
                dirVec = new Vec2i(0, -1); break;
            default:
                throw new InvalidDirectionException(string.Format("Cannot move in direction '{0}'!", dir));
        }

        return dirVec;
    }
}

public class InvalidDirectionException : Exception
{
    public InvalidDirectionException() : base() { }
    public InvalidDirectionException(string message) : base(message) { }
    public InvalidDirectionException(string message, Exception innerException) : base(message, innerException) { }
    public InvalidDirectionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public struct Message
{
    public bool valid;
    public string errMessage;

    public Message(bool valid)
    {
        this.valid = valid;
        errMessage = "";
    }

    public Message(bool valid, string message)
    {
        this.valid = valid;
        this.errMessage = message;
    }
}
