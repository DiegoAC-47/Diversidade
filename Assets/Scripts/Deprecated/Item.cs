using UnityEngine;
using System.Collections;
using System;

public class Item : Interactible
{
    [SerializeField]
    private int size;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private string title, info;
    [SerializeField]
    private bool visible;
    [SerializeField]
    private Card card;

    private bool interactible;

    private void Start()
    {
        this.card = (Card)Instantiate(this.card, this.transform);
    }

    private void Update()
    {
        if (this.visible)
        {
            if (this.interactible)
            {
                this.card.GetComponentInChildren<Canvas>().enabled = true;
                this.card.transform.position = new Vector3(this.transform.position.x, this.GetComponent<MeshRenderer>().bounds.max.y, this.transform.position.z) + Vector3.up;
            }
            else
                this.card.GetComponentInChildren<Canvas>().enabled = false;
        }
    }

    public override void IsInteractible(bool b)
    {
        this.GetComponentInChildren<Card>().Interact(b);
        this.interactible = b;
    }

    public override void OnInteract()
    {
        //if (FindObjectOfType<Player>().AddItem(this))
        //{
        //    FindObjectOfType<Player>().SetAnimationTrigger("Pick");
        //    this.GetComponentInChildren<Card>().Interact(false);
        //}
    }

    public bool Visible
    {
        get
        {
            return visible;
        }

        set
        {
            this.GetComponent<MeshRenderer>().enabled = value;
            this.GetComponent<Collider>().enabled = value;
            if (value == false)
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            else
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            this.card.GetComponentInChildren<Canvas>().enabled = value;
            visible = value;
        }
    }

    public int Size
    {
        get
        {
            return size;
        }
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    public string Title
    {
        get
        {
            return title;
        }
    }

    public string Info
    {
        get
        {
            return info;
        }
    }
}
