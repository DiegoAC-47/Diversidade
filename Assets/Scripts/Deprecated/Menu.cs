using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    [SerializeField]
    private ItemButton itemDefault;
    [SerializeField]
    private int lin, col;
    [SerializeField]
    private Text size;
    private ItemButton[] itens;

	private void Start ()
    {
        this.itens = new ItemButton[this.lin * this.col];
	    for(int i = 0; i < this.lin; i++)
        {
            for(int j = 0; j < this.col; j++)
            {
                this.itens[i * this.col + j] = (ItemButton)Instantiate(this.itemDefault, this.GetComponent<RectTransform>());
                this.itens[i * this.col + j].GetComponent<RectTransform>().anchoredPosition = new Vector3(this.itemDefault.GetComponent<RectTransform>().sizeDelta.x / 2 + 10 + (10 + this.itemDefault.GetComponent<RectTransform>().sizeDelta.x) * j, -this.itemDefault.GetComponent<RectTransform>().sizeDelta.y / 2 - 10 + (10 + this.itemDefault.GetComponent<RectTransform>().sizeDelta.y) * -i, 0);
            }
        }
        this.RefreshText();
    }

    public void AddItem(Item i)
    {
        for(int j = 0; j < this.itens.Length; j++)
        {
            if(this.itens[j].Item == null)
            {
                this.itens[j].Item = i;
                this.RefreshText(); return;
            }
        }
    }

    public void RemoveItem(Item i)
    {
        for (int j = 0; j < this.itens.Length; j++)
        {
            if (this.itens[j].Item == i)
            {
                this.itens[j].Item = null;
                this.RefreshText();
                return;
            }
        }
    }

    private void Update ()
    {
	
	}

    private void RefreshText()
    {
        this.size.text = "Ocupado/Total \n" + FindObjectOfType<Inventory>().Occuped + "/" + FindObjectOfType<Inventory>().Max;
    }
}
