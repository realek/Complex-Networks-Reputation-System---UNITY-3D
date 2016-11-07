using System.Collections.Generic;

public enum PlayerRep
{
    Neutral,
    Bad,
    Good
}
[System.Serializable]
public class NPCReps {
        
    Dictionary<NpcEntry, PlayerRep> m_nPCplayerRep;

    public NPCReps()
    {
        m_nPCplayerRep = new Dictionary<NpcEntry, PlayerRep>();
    }

    //Adds npc on player meet
    public void AddNpc(NpcEntry npc)
    {
        m_nPCplayerRep.Add(npc, PlayerRep.Neutral);
    }

    //set npc rep after event/action
    public void SetRep(NpcEntry npc, PlayerRep rep)
    {
        m_nPCplayerRep[npc] = rep;
    }

}
