using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalOptions : MonoBehaviour {

    [SerializeField]
    private float dialogTypeInterval = 0.05f, objectDistanceInteract = 2f, achievementPopupDuration = 3f, dialogInterval = 2f, changeModelDuration = 2f;
    [SerializeField]
    private Color questActive = Color.yellow, questUnactive = Color.gray, questDone = Color.green, questCompleted = Color.blue;
    [SerializeField]
    private int dialogTextSize = 50;
    [SerializeField]
    private bool playStartCutScene = true, jumpCG = false;
    [SerializeField]
    private int targetFrameRate = 60;

    public float AchievementPopupDuration
    {
        get
        {
            return achievementPopupDuration;
        }

        set
        {
            achievementPopupDuration = value;
        }
    }

    public float DialogInterval
    {
        get
        {
            return dialogInterval;
        }

        set
        {
            dialogInterval = value;
        }
    }

    public float DialogTypeInterval
    {
        get
        {
            return dialogTypeInterval;
        }

        set
        {
            dialogTypeInterval = value;
        }
    }

    public float ObjectDistanceInteract
    {
        get
        {
            return objectDistanceInteract;
        }

        set
        {
            objectDistanceInteract = value;
        }
    }

    public Color QuestActive
    {
        get
        {
            return questActive;
        }

        set
        {
            questActive = value;
        }
    }

    public Color QuestCompleted
    {
        get
        {
            return questCompleted;
        }

        set
        {
            questCompleted = value;
        }
    }

    public Color QuestDone
    {
        get
        {
            return questDone;
        }

        set
        {
            questDone = value;
        }
    }

    public Color QuestUnactive
    {
        get
        {
            return questUnactive;
        }

        set
        {
            questUnactive = value;
        }
    }

    public int DialogTextSize
    {
        get
        {
            return dialogTextSize;
        }

        set
        {
            dialogTextSize = value;
        }
    }

    public float ChangeModelDuration
    {
        get
        {
            return changeModelDuration;
        }

        set
        {
            changeModelDuration = value;
        }
    }

    public bool PlayStartCutScene
    {
        get
        {
            return playStartCutScene;
        }

        set
        {
            playStartCutScene = value;
        }
    }

    public int TargetFrameRate
    {
        get
        {
            return targetFrameRate;
        }

        set
        {
            targetFrameRate = value;
        }
    }

    public bool JumpCG
    {
        get
        {
            return jumpCG;
        }

        set
        {
            jumpCG = value;
        }
    }
}
