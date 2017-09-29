using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour {

    [SerializeField]
    private AchievementLabel label;
    [SerializeField]
    private Canvas canvas;

    private List<NPC> npcs;
    private int quests_num, npcs_num, itens_num;

	void Start ()
    {
        this.npcs = new List<NPC>();
	}
	
	void Update ()
    {
	}

    public bool Quest_Completed(Quest obj)
    {
        this.quests_num++;
        //if (this.quests_num == 2)
        //{
        //    AchievementLabel l = Instantiate(this.label, this.canvas.transform, false);
        //    l.Title = "Testador de demos";
        //    l.Description = "Completou as duas quests da demo";
        //}
        return true;
    }

    public bool Npc_Interact(NPC obj)
    {
        if (!this.npcs.Contains(obj))
        {
            this.npcs_num++;
            //if(this.npcs_num == 2)
            //{
            //    AchievementLabel l = Instantiate(this.label, this.canvas.transform, false);
            //    l.Title = "Extrovertido";
            //    l.Description = "Conversou com dois NPCs diferentes";
            //}
            this.npcs.Add(obj);
            return true;
        }
        return false;
    }

    public bool Collectible_Collected(Collectible obj)
    {
        this.itens_num++;
        //if (this.itens_num == 3)
        //{
        //    AchievementLabel l = Instantiate(this.label, this.canvas.transform, false);
        //    l.Title = "Colecinador";
        //    l.Description = "Pegou um total de três itens";
        //}
        return true;
    }

    public bool Item_Collected(QuestItem obj)
    {
        this.itens_num++;
        //if (this.itens_num == 3)
        //{
        //    AchievementLabel l = Instantiate(this.label, this.canvas.transform, false);
        //    l.Title = "Colecinador";
        //    l.Description = "Pegou um total de três itens";
        //}
        return true;
    }
}
