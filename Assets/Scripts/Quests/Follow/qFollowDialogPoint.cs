using UnityEngine;
using System.Collections;
using System;

public class qFollowDialogPoint : MonoBehaviour
{
    [SerializeField]
    private DialogManager dialogManager;
    [SerializeField]
    private string descriptionUpdate;
    [SerializeField]
    private float speedToPoint = 3.5f;

    public event Predicate<Collider> OnTrigger;

    private void Awake()
    {
        BoxCollider collider = this.gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (this.OnTrigger != null)
            this.OnTrigger(other);
    }

    public void Expand(Vector3 size)
    {
        this.gameObject.GetComponent<BoxCollider>().size = size;
    }

    public DialogManager DialogManager
    {
        get
        {
            return dialogManager;
        }
    }

    public string DescriptionUpdate
    {
        get
        {
            return descriptionUpdate;
        }
    }

    public float SpeedToPoint
    {
        get
        {
            return speedToPoint;
        }

        set
        {
            speedToPoint = value;
        }
    }
}
