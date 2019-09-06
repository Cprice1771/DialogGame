using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private const string game_file_path = "Assets/GameGraphs/ch1.json";
    private GameMap _game;
    public GameNode CurrentNode;

    private int textCounter = 1;

    public TextMeshProUGUI Dialoge;

    public List<Button> Actions;

    // Start is called before the first frame update
    void Start()
    {
        StreamReader reader = new StreamReader(game_file_path);
        _game = GameParser.ParseFile(reader.ReadToEnd());
        _game.SetDefaultValues();
        Debug.Log($"nodes: {_game.nodes.Count}");
        LoadNode(_game.nodes.First(x => x.NodeType == NodeTypes.START));
    }

    public void ApplyConsequnces(List<Consequence> toapply) {
        foreach (var consequnce in toapply) {
            var stateToUpdate = _game.gameState.FirstOrDefault(x => x.id == consequnce.State.id);
            switch (stateToUpdate.type) {
                case "enum":
                    stateToUpdate.value = consequnce.Value;
                    break;
                case "num":

                    if (consequnce.Operator == Consequence.ADD) {
                        stateToUpdate.value = (decimal.Parse(stateToUpdate.value) + decimal.Parse(consequnce.Value)).ToString();
                    } else if (consequnce.Operator == Consequence.SET) {
                        stateToUpdate.value = consequnce.Value;
                    }

                    break;
                case "bool":
                    stateToUpdate.value = consequnce.Value;
                    break;
            }
        }
    }

    public void TakeAction(string id) {
        var actionToTake = CurrentNode.Actions.First(y => y.id == id);
        ApplyConsequnces(actionToTake.Consequences);
        var node = _game.nodes.FirstOrDefault(x => x.id == actionToTake.EndAt);
        LoadNode(node);
        textCounter = 1;
    }

    public void LoadNode(GameNode node) {
        CurrentNode = node;
        Dialoge.text = CurrentNode.Conversation.First().Text;
        foreach(var action in Actions) {
            action.onClick.RemoveAllListeners();
            action.gameObject.SetActive(false);
        }
    }

    public List<GameAction> GetAvailableActions() {
        var availableActions = new List<GameAction>();

        foreach(var action in CurrentNode.Actions) {
            var passedRequirements = true;
            foreach(var requirement in action.Requirements) {
                var stateToCheck = _game.gameState.FirstOrDefault(x => x.id == requirement.State.id);
                switch (stateToCheck.type) {
                    case "enum":
                        passedRequirements = stateToCheck.value == requirement.Value;
                        break;
                    case "num":

                        if (requirement.Operator == Requirement.EQUAL_TO) {
                            passedRequirements = stateToCheck.value == requirement.Value; 
                        } else if(requirement.Operator == Requirement.LESS_THAN) {
                            passedRequirements = decimal.Parse(stateToCheck.value) < decimal.Parse(requirement.Value);
                        } else if (requirement.Operator == Requirement.GREATER_THAN) {
                            passedRequirements = decimal.Parse(stateToCheck.value) > decimal.Parse(requirement.Value);
                        }

                        break;
                    case "bool":
                        passedRequirements = stateToCheck.value == requirement.Value;
                        break;
                }

                if(!passedRequirements) {
                    break;
                }
            }

            if(passedRequirements) {
                availableActions.Add(action);
            }
        }

        return availableActions;
    }

    public void OnTextClick() {
        if(textCounter < CurrentNode.Conversation.Count) {
            textCounter += 1;
        }

        Dialoge.text = string.Join("\n\n", CurrentNode.Conversation.Take(textCounter).Select(x => x.Text));
        if(textCounter >= CurrentNode.Conversation.Count) {

            var availableACtions = GetAvailableActions();


            if(availableACtions.Count == 1) {
                TakeAction(availableACtions.First().id);
            } else {
                for (var i = 0; i < availableACtions.Count; i++) {
                    Actions[i].gameObject.SetActive(true);
                    Actions[i].GetComponentInChildren<Text>().text = availableACtions[i].Name;
                    var id = availableACtions[i].id;
                    Actions[i].onClick.AddListener(delegate { TakeAction(id); });
                }
            }   
        }
    }
}
