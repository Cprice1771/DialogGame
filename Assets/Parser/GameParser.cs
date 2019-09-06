using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameParser
{
    public static GameMap ParseFile(string jsonString)
    {
        return JsonUtility.FromJson<GameMap>(jsonString);
    }
}
