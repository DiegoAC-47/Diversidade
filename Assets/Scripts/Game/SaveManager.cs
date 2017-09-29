using UnityEngine;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private Canvas feedback;
    [SerializeField]
    private float feedbackAditionalTime = 1f;

    private bool io;
    private CutScene cutSceneToPlay;

    private void Awake()
    {
        if (FindObjectsOfType<SaveManager>().Length > 1)
            Destroy(this.gameObject);

        this.feedback.sortingOrder = 999;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        this.feedback.enabled = false;
    }

    public void Save()
    {
        if(!this.io)
        {
            this.io = true;
            StartCoroutine(this.SaveFeedback());

            Quest[] quests = FindObjectOfType<QuestSystem>().Quests;
            foreach(Quest q in quests)
            {
                PlayerPrefs.SetInt(q.Title, (int)q.State);
                PlayerPrefs.SetInt(q.Title + "Score", q.Score);
            }

            DialogManager[] dialogs = FindObjectsOfType<DialogManager>();
            foreach(DialogManager d in dialogs)
            {
                PlayerPrefs.SetInt(d.DialogFileName + "ADialog", d.ActualDialog);
            }

            PlayerPrefs.SetInt("StartCgPlayed", FindObjectOfType<Game>().StartcgPlayed ? 1 : 0);
            Vector3 position = FindObjectOfType<Player>().transform.position;
            PlayerPrefs.SetFloat("PlayerX", position.x);
            PlayerPrefs.SetFloat("PlayerY", position.y);
            PlayerPrefs.SetFloat("PlayerZ", position.z);

            this.io = false;
        }
    }

    public void Load()
    {
        short index = 0;

        this.io = true;
        StartCoroutine(this.SaveFeedback());

        FindObjectOfType<Game>().StartcgPlayed = PlayerPrefs.GetInt("StartCgPlayed") == 0 ? false : true;

        Quest[] quests = FindObjectOfType<QuestSystem>().Quests;
        for(short i = 0; i < quests.Length; i ++)
        {
            if (PlayerPrefs.GetInt(quests[i].Title) == (int)QuestState.COMPLETED)
            {
                quests[i].LoadState();
                quests[i].ResetScore();
                quests[i].AddScore(PlayerPrefs.GetInt(quests[i].Title + "Score"));

                if(quests[i].Npc.Dialog.CutSceneBeforeQuest != null)
                    this.CutSceneToPlay = quests[i].Npc.Dialog.CutSceneBeforeQuest;
                if (quests[i].AfterQuestScene != null)
                    this.CutSceneToPlay = quests[i].AfterQuestScene;
                if (quests[i].Npc.Dialog.CutSceneAfterQuest != null)
                    this.CutSceneToPlay = quests[i].Npc.Dialog.CutSceneAfterQuest;

                index = i;
            }
            else
                break;
        }

        DialogManager[] dialogs = FindObjectsOfType<DialogManager>();
        foreach (DialogManager d in dialogs)
        {
            d.ActualDialog = PlayerPrefs.GetInt(d.DialogFileName + "ADialog");
        }

        this.io = false;

        if (this.cutSceneToPlay != null)
        {
            this.cutSceneToPlay.StartCinematic();
        }
        else
        {
            //FindObjectOfType<Player>().transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), 
            //                                                            PlayerPrefs.GetFloat("PlayerY"), 
            //                                                            PlayerPrefs.GetFloat("PlayerZ"));
            if (quests[index].NpcInteractOnDone)
                quests[index].Npc.OnInteract();
        }
    }

    private IEnumerator SaveFeedback()
    {
        this.feedback.enabled = true;

        while(this.io)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(this.feedbackAditionalTime);

        this.feedback.enabled = false;
    }

    public void CleanSave()
    {
        PlayerPrefs.DeleteAll();
    }

    public CutScene CutSceneToPlay
    {
        get
        {
            return cutSceneToPlay;
        }

        set
        {
            if (this.cutSceneToPlay != null)
            {
                this.cutSceneToPlay.StartCinematic();
                this.cutSceneToPlay.Finish();
            }
            this.cutSceneToPlay = value;
        }
    }
}
