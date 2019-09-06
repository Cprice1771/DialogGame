using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameMap
{
    public List<GameNode> nodes = new List<GameNode>();
    public List<Character> characters = new List<Character>();
    public List<GameState> gameState = new List<GameState>();

    internal void SetDefaultValues() {
        foreach(var state in gameState) {
            state.value = state.defaultValue;
        }
    }
}

[System.Serializable]
public class GameState {
    public string id;
    public string name;
    public string defaultValue;
    public string type;
    public string[] enumValues;
    public string value;
}



[System.Serializable]
public class Character {
    public string id;
    public string Name;
    public string Description;
}

[System.Serializable]
public class GameNode
{
    public string id;
    public string Name;
    public string NodeType;
    public string Description;
    public string title;
    public List<GameAction> Actions;
    public List<Dialogue> Conversation;
}

[System.Serializable]
public class Dialogue {
    public string id;
    public string Speaker;
    public string Text;
    public string CharacterId;
}

[System.Serializable]
public class GameAction
{
    public string id;
    public string EndAt;
    public string Name;
    public string Description;
    public List<Consequence> Consequences;
    public List<Requirement> Requirements;
}

[System.Serializable]
public class Consequence {

    public const string SET = "Set";
    public const string ADD = "Add";

    public string id;
    public GameState State;
    public string Operator;
    public string Value;
}

[System.Serializable]
public class Requirement {

    public const string EQUAL_TO = "EqualTo";
    public const string LESS_THAN = "LessThan";
    public const string GREATER_THAN = "GreaterThan";

    public string id;
    public GameState State;
    public string Operator;
    public string Value;
}