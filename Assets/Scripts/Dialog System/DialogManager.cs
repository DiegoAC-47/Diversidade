using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DialogSystem;
using System;

public enum DialogBehaviour
{
    QUEST,
    STATIC_QUEST,
    SIMPLE,
    CINEMATIC
}

public enum DialogBoxPosition
{
    TOP,
    BOTTOM
}

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private DialogBehaviour behaviour;
    [SerializeField]
    private Canvas npcFeedbackTalk;
    [SerializeField]
    private Color backgroundDialogColor = Color.grey;
    [SerializeField]
    private Quest quest;
    [SerializeField]
    private CutScene cutSceneBeforeQuest, cutSceneAfterQuest;
    [SerializeField]
    private string dialogFileName;
    [SerializeField]
    private string folderPath = "/Resources/Dialogs/", fileFormat = ".dialogtree";
    [SerializeField]
    private DialogManager nextDialog;
    [SerializeField]
    private RuntimeAnimatorController specificControllerPlayer;
    [SerializeField]
    private DialogBoxPosition boxPosition = DialogBoxPosition.BOTTOM;

    private List<string> animationsPlayedPlayer = new List<string>(), animationsPlayedNPC = new List<string>();

    private GlobalOptions globalOptions;
    private InterfaceManager interfaceManager;
    private RuntimeAnimatorController originalController;
    private Animator animPlayer, animNPC;
    private DialogTree tree;
    private bool play, postPlay;
    private int actualDialog = 0, actualSpeech = 0;

    private Canvas playerFeedbackTalk;
    private Color playerBackgroundColor;

    private bool completeText;
    private int typeIndex;
    private string printText;

    public event Predicate<DialogManager> Finish;

    /// <summary>
    /// Carrega na memória todo o texto do personagem
    /// </summary>
    private void Start ()
    {
        IO.GetInstance().Setup(Application.dataPath + this.folderPath + this.dialogFileName + this.fileFormat);
        this.tree = IO.GetInstance().GetDataFile();
        this.animPlayer = FindObjectOfType<Player>().GetComponentInChildren<Animator>();
        this.animNPC = this.GetComponentInChildren<Animator>();
        this.interfaceManager = FindObjectOfType<InterfaceManager>();
        this.globalOptions = FindObjectOfType<GlobalOptions>();
    }
	
    /// <summary>
    /// Inicia o dialogo
    /// </summary>
    public void SetDialog()
    {
        this.originalController = this.animPlayer.runtimeAnimatorController;

        if (this.specificControllerPlayer != null && this.originalController != this.specificControllerPlayer)
        {
            this.animPlayer.runtimeAnimatorController = this.specificControllerPlayer;
        }

        FindObjectOfType<Game>().InDialog = true;
        Player player = FindObjectOfType<Player>();
        this.playerFeedbackTalk = player.FeedbackTalk;
        this.playerBackgroundColor = player.BackgroundColor;

        if (this.tree.Dialogs.Count > 0)
        {
            if (this.tree.Dialogs[this.actualDialog].Speeches.Count > 0)
            {
                this.play = true;

                this.PlayAnimation();

                switch (this.behaviour)
                {
                    case DialogBehaviour.STATIC_QUEST:
                    case DialogBehaviour.QUEST:
                        if (this.actualDialog == 1)
                        {
                            Quest q = FindObjectOfType<QuestSystem>().FindQuest(this.quest.Title);
                            if (q != null && q.State == QuestState.DONE)
                            {
                                this.actualDialog++;
                                q.State = QuestState.COMPLETED;
                            }
                        }
                        break;
                }
                StartCoroutine(this.StartDialog());
            }
            else
                this.End();
        }
        else
            this.End();
    }

    /// <summary>
    /// Muda a fala/balão do dialogo
    /// </summary>
    private void NextSpeech()
    {
        this.SetAllAnimationOff(this.animPlayer);
        this.SetAllAnimationOff(this.animNPC);

        this.typeIndex = 0;
        this.printText = "";
        this.completeText = false;

        this.actualSpeech++;

        if (this.actualSpeech >= this.tree.Dialogs[this.actualDialog].Speeches.Count)
        {
            this.End();
            switch (this.behaviour)
            {
                case DialogBehaviour.STATIC_QUEST:
                case DialogBehaviour.QUEST:
                    if (this.actualDialog == 0)
                    {
                        if (this.cutSceneBeforeQuest != null)
                        {
                            this.cutSceneBeforeQuest.StartCinematic();
                            StartCoroutine(this.WaitCutSceneEnd());
                        }
                        else
                        {
                            FindObjectOfType<QuestSystem>().AddQuest(this.quest);
                            this.actualDialog++;
                        }
                    }
                    else if (this.actualDialog == 2)
                    {
                        this.actualDialog++;
                        if (this.cutSceneAfterQuest != null)
                            this.cutSceneAfterQuest.StartCinematic();
                    }
                    break;

                case DialogBehaviour.SIMPLE:
                    if (this.actualDialog == 0)
                        this.actualDialog++;
                    break;
            }
        }
        else
        {
            this.PlayAnimation();
        }
    }

    /// <summary>
    /// Finaliza o dialogo e inicia o nextDialog se ele não for nulo
    /// </summary>
    public void End()
    {
        try
        {
            this.SetAllAnimationOff(this.animPlayer);
            this.SetAllAnimationOff(this.animNPC);

            if (this.specificControllerPlayer != null && this.animPlayer.runtimeAnimatorController != this.originalController)
                this.animPlayer.runtimeAnimatorController = this.originalController;

            if (this.nextDialog != null)
            {
                if (this.npcFeedbackTalk != null)
                    this.npcFeedbackTalk.GetComponentInChildren<SpriteRenderer>().enabled = false;
                this.npcFeedbackTalk.GetComponent<Billboard>().enabled = false;
                this.playerFeedbackTalk.GetComponentInChildren<SpriteRenderer>().enabled = false;
                this.playerFeedbackTalk.GetComponent<Billboard>().enabled = false;
                this.nextDialog.Finish += NextDialog_Finish;
                this.postPlay = true;
                this.nextDialog.SetDialog();
            }
            else
            {
                this.play = false;
                this.postPlay = false;
                this.actualSpeech = 0;
                if (this.npcFeedbackTalk != null)
                {
                    this.npcFeedbackTalk.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    this.npcFeedbackTalk.GetComponent<Billboard>().enabled = false;
                }

                if (this.playerFeedbackTalk != null)
                {
                    this.playerFeedbackTalk.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    this.playerFeedbackTalk.GetComponent<Billboard>().enabled = false;
                }

                if (this.boxPosition == DialogBoxPosition.BOTTOM)
                    this.interfaceManager.DialogBoxBottom.enabled = false;
                else
                    this.interfaceManager.DialogBoxTop.enabled = false;

                this.typeIndex = 0;
                this.printText = "";
                this.completeText = false;

                if (this.nextDialog != null)
                    this.nextDialog.Finish -= NextDialog_Finish;

                if (this.Finish != null)
                    this.Finish(this);

                FindObjectOfType<Game>().InDialog = false;
            }
        }
        catch (Exception e)
        {
            ///coloquei isso aqui pq estava dando erro ao iniciar a quest no bar da denise
            print(e.Message);
        }
    }

    /// <summary>
    /// Chamado quando o NextDialog acaba
    /// </summary>
    /// <param name="obj">O dialogo que acabou</param>
    /// <returns></returns>
    private bool NextDialog_Finish(DialogManager obj)
    {
        this.play = false;
        this.postPlay = false;
        this.actualSpeech = 0;
        this.npcFeedbackTalk.GetComponentInChildren<SpriteRenderer>().enabled = false;
        this.npcFeedbackTalk.GetComponent<Billboard>().enabled = false;
        this.typeIndex = 0;
        this.printText = "";
        this.completeText = false;

        this.nextDialog.Finish -= NextDialog_Finish;

        if (this.Finish != null)
            this.Finish(this);
        return true;
    }

    /// <summary>
    /// Corrotine que aplica o efeito de digitação e imprime o resultado no balão
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartDialog()
    {
        while (this.play && !this.postPlay)
        {
            this.TypeText();
            this.Print();

            yield return new WaitForSeconds(FindObjectOfType<GlobalOptions>().DialogTypeInterval);
        }
    }

    /// <summary>
    /// Escreve texto letra por letra
    /// </summary>
    private void TypeText()
    {
        if (this.typeIndex < this.tree.Dialogs[this.actualDialog].Speeches[this.actualSpeech].Text.Length && !this.completeText)
            this.printText += this.tree.Dialogs[this.actualDialog].Speeches[this.actualSpeech].Text[this.typeIndex++];
        else
            this.completeText = true;
    }

    /// <summary>
    /// Chama a próxima fala se a atual já estiver finalizada, ou finaliza a atual
    /// </summary>
    public void Next()
    {
        if (this.postPlay)
        {
            this.nextDialog.Next();
        }
        else
        {
            if (this.completeText)
            {
                this.NextSpeech();
            }
            else
            {
                this.completeText = true;
                this.printText = this.tree.Dialogs[this.actualDialog].Speeches[this.actualSpeech].Text;
            }
        }
    }

    /// <summary>
    /// Imprime nos balões o texto
    /// </summary>
    private void Print()
    {
        Text text;
        if (this.boxPosition == DialogBoxPosition.BOTTOM)
        {
            this.interfaceManager.DialogBoxBottom.enabled = true;
            text = this.interfaceManager.DialogBoxBottom.GetComponentInChildren<Text>();
        }
        else
        {
            this.interfaceManager.DialogBoxTop.enabled = true;
            text = this.interfaceManager.DialogBoxTop.GetComponentInChildren<Text>();
        }

        text.fontSize = this.globalOptions.DialogTextSize;
        text.text = this.printText;

        switch (this.tree.Dialogs[this.actualDialog].Speeches[this.actualSpeech].Actor)
        {
            case Actor.NPC:
                this.npcFeedbackTalk.GetComponentInChildren<SpriteRenderer>().enabled = true;
                this.npcFeedbackTalk.GetComponent<Billboard>().enabled = true;
                this.playerFeedbackTalk.GetComponentInChildren<SpriteRenderer>().enabled = false;
                this.playerFeedbackTalk.GetComponent<Billboard>().enabled = false;
                text.GetComponentInParent<Image>().color = this.backgroundDialogColor;
                break;
            case Actor.PLAYER:
                if (this.npcFeedbackTalk != null)
                    this.npcFeedbackTalk.GetComponentInChildren<SpriteRenderer>().enabled = false;
                this.playerFeedbackTalk.GetComponentInChildren<SpriteRenderer>().enabled = true;
                this.playerFeedbackTalk.GetComponent<Billboard>().enabled = true;
                text.GetComponentInParent<Image>().color = this.playerBackgroundColor;
                break;
        }
    }

    private IEnumerator WaitCutSceneEnd()
    {
        while (FindObjectOfType<CutSceneSystem>().ActualScene != null)
            yield return new WaitForEndOfFrame();
        FindObjectOfType<QuestSystem>().AddQuest(this.quest);
        this.actualDialog++;
    }

    /// <summary>
    /// Inicia a animação da fala atual
    /// </summary>
    private void PlayAnimation()
    {
        Speech speech = this.tree.Dialogs[this.actualDialog].Speeches[this.actualSpeech];

        if (speech.Anim.Length > 0)
            this.SetAnimation(speech.Anim, speech.AnimLayer, true, speech.AnimIsTrigger);
        if (speech.FacialExpression.Length > 0)
        {
            this.SetAnimation(speech.FacialExpression, speech.FacialExpressionLayer, true, speech.FacilExpressionIsTrigger);

        }
    }

    /// <summary>
    /// Seta uma animação em um animator seja ela trigger ou bool
    /// </summary>
    /// <param name="anim">Nome da parâmetro</param>
    /// <param name="value">Valor da bool</param>
    /// <param name="isTrigger">É um trigger?</param>
    private void SetAnimation(string anim, int layer, bool value, bool isTrigger)
    {
        Animator animator = null;

        if (this.tree.Dialogs[this.actualDialog].Speeches[this.actualSpeech].Actor == Actor.PLAYER)
            animator = this.animPlayer;
        else
            animator = this.animNPC;

        if (animator && anim.Length > 0)
        {
            //this.SetAllAnimationOff(animator);

            if (anim == "MoveRun")
            {
                anim = "Move";
                animator.SetFloat("Speed", 1f);
            }
            else if (anim == "Move")
            {
                animator.SetFloat("Speed", 0.5f);
            }

            animator.SetLayerWeight(layer, value ? 1f : 0f);

            if (isTrigger)
            {
                if (value)
                    animator.SetTrigger(anim);
                else
                    animator.SetTrigger("Exit");
            }
            else
            {
                animator.SetBool(anim, value);
                if(value)
                {
                    if (animator == this.animPlayer)
                        this.animationsPlayedPlayer.Add(anim);
                    else
                        this.animationsPlayedNPC.Add(anim);
                }
            }
        }
    }

    /// <summary>
    /// Desliga todas as animações do tipo bool em um animator
    /// </summary>
    /// <param name="animator">Animator que será afetado</param>
    private void SetAllAnimationOff(Animator animator)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);

            foreach (string s in animator == this.animPlayer ? this.animationsPlayedPlayer : this.animationsPlayedNPC)
            {
                animator.SetBool(s, false);
            }

            if (animator == this.animPlayer)
                this.animationsPlayedPlayer.Clear();
            else
                this.animationsPlayedNPC.Clear();

            animator.SetBool("Exit", true);
            animator.SetBool("Exit", false);
        }
    }

    /// <summary>
    /// Inicia a quest e avança para o próximo dialogo. Útil para iniciar quests sem precisar de um dialogo inicial
    /// </summary>
    public void ShortCutStartQuest()
    {
        FindObjectOfType<QuestSystem>().AddQuest(this.quest);
        this.actualDialog++;
    }

    /// <summary>
    /// Finaliza a quest alterando seu state para Complete e avança o dialogo. Útil para terminar uma quest sem um dialogo
    /// </summary>
    /// <param name="quest">Quest a ser finalizada</param>
    public void ShortCutFinishQuest(Quest quest)
    {
        this.actualDialog++;
        quest.State = QuestState.COMPLETED;
    }

    public bool Play
    {
        get
        {
            return play;
        }
    }

    public bool CompleteText
    {
        get
        {
            return completeText;
        }
    }

    public DialogManager NextDialog
    {
        get
        {
            return nextDialog;
        }
    }

    public int ActualDialog
    {
        get
        {
            return actualDialog;
        }

        set
        {
            actualDialog = value;
        }
    }

    public int ActualSpeech
    {
        get
        {
            return actualSpeech;
        }

        set
        {
            actualSpeech = value;
        }
    }

    public string DialogFileName
    {
        get
        {
            return dialogFileName;
        }
    }

    public CutScene CutSceneBeforeQuest
    {
        get
        {
            return cutSceneBeforeQuest;
        }
    }

    public CutScene CutSceneAfterQuest
    {
        get
        {
            return cutSceneAfterQuest;
        }
    }
}
