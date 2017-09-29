using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    [SerializeField]
    private int occuped, max;
    private ArrayList itens;
    private Player player;

    private void Start ()
    {
        this.itens = new ArrayList();
        this.player = FindObjectOfType<Player>();
	}
	
	private void Update ()
    {
	    if(this.player == null)
        {
            this.player = FindObjectOfType<Player>();
        }
	}

    public bool Add
        (Item i)
    {
        if(this.max - this.occuped >= i.Size)
        {
            i.Visible = false;
            this.occuped += i.Size;
            this.itens.Add(i);
            i.transform.SetParent(this.player.transform);
            //i.transform.position = this.player.InventoryPosition.position;
            FindObjectOfType<Menu>().AddItem(i);
            return true;
        }
        return false;
    }

    public void RemoveItem(Item i)
    {
        i.Visible = true;
        this.occuped -= i.Size;
        this.itens.Remove(i);
        i.transform.SetParent(null);
        i.transform.position = this.player.transform.position + this.player.transform.forward * 2;
        FindObjectOfType<Menu>().RemoveItem(i);
    }

    public ArrayList Itens
    {
        get
        {
            return itens;
        }
    }

    public int Occuped
    {
        get
        {
            return occuped;
        }
    }

    public int Max
    {
        get
        {
            return max;
        }
    }
}
