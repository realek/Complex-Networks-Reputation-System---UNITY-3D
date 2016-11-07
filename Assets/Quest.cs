using System.Collections.Generic;
using UnityEngine;


public enum QuestType
{
    Main,
    Side
}

public enum QuestChoice
{
    None,
    Choiceless,
    Evil,
    Good,
    Neutral
}

public enum QuestState
{
    Refused,
    Accepted,
    Completed
}
[System.Serializable]
struct Quest {


    //Quest data
    public string name;
    public string description;
    public QuestChoice takenChoice;
    public QuestType type;
    public QuestState state;
    public Location questLocation;
    // Use this for initialization

    public void GoodChoice()
    {
        takenChoice = QuestChoice.Good;
        Complete();
    }

    public void EvilChoice()
    {
        takenChoice = QuestChoice.Evil;
        Complete();
    }

    public void NeutralChoice()
    {
        takenChoice = QuestChoice.Neutral;
        Complete();
    }

    public void NoChoice()
    {
        takenChoice = QuestChoice.Choiceless;
        Complete();
    }

    public void Refused()
    {
        state = QuestState.Refused;
        takenChoice = QuestChoice.None;
    }

    public void Accepted()
    {
        state = QuestState.Accepted;
    }

    private void Complete()
    {
        state = QuestState.Completed;
    }
}
