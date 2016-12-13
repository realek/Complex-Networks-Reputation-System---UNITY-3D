using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// UI class to be extended if needed
/// </summary>
public class QuestUI : MonoBehaviour {

    public Text questText;

    public void DisplayQuest(Quest q)
    {
        if(questText!=null)
        {

            questText.text = "Quest Description:" + q.description;

            if (q.QuestGiver != null)
            {
                questText.text += System.Environment.NewLine;
                questText.text += "Quest Giver: " + q.QuestGiver.fName + " " + q.QuestGiver.lName;
            }

            if(q.QuestEnder != null)
            {
                questText.text += System.Environment.NewLine;
                questText.text += "Quest Turnin: " + q.QuestEnder.fName + " " + q.QuestEnder.lName;
            }

        }


    }
}
