using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestList", menuName = "Quests/Quest List")]
public class QuestList : ScriptableObject
{
    public List<QuestObject> Quests;
}
