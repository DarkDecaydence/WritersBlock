using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Translater : MonoBehaviour {

	public Message interpretMessage(string msg)
    {

        msg = msg.ToLower();

        string[] partMessages = msg.Split(' ');

        string command = partMessages[0];

        switch (command)
        {
            case "move":
                return interpretMove(partMessages); ;
            case "attack":            
                return interpretAttack(partMessages); ;
            default:
                return new Message(false, "*WinkyFace*");
        }
    }

    private Message interpretAttack(string[] partMessages)
    {
        return new Message(false, "Attack not implemented yet");
    }

    private Message interpretMove(string[] partMessages)
    {

        if (partMessages.Length <= 1)
            return new Message(false, "Specify a direction move you dumbnut");

        string direction = partMessages[1];
        bool moveValidity;
        switch (direction)
        {
            case "left":
                moveValidity = GameData.playerCharacter.move(new Vec2i(-1, 0));
                break;
            case "right":
                moveValidity = GameData.playerCharacter.move(new Vec2i(1, 0));
                break;
            case "up":
                moveValidity = GameData.playerCharacter.move(new Vec2i(0, 1));
                break;
            case "down":
                moveValidity = GameData.playerCharacter.move(new Vec2i(0, -1));
                break;
            default:
                moveValidity = false;
                break;
        }

        return new Message(moveValidity, "Slamming into a wall eh?");
    }
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
