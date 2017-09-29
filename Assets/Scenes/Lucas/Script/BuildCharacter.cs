using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildCharacter : MonoBehaviour
{
    [SerializeField]
    private GameObject begin, end;

    [SerializeField]
    private Transform model;

    [SerializeField]
    private Button[] choices;

    [SerializeField]
    private Renderer[] hairRend, faceRend, clothingRend, accessoryRend;

    [SerializeField]
    private Texture[] hair, face, clothing, accessory;

    [SerializeField]
    private Sprite[] hairSprite, faceSprite, clothingSprite, accssorySprite;

    private float speed;
    [SerializeField]
    private int category, page;

    private bool click = false;

    private void Start()
    {
        begin.SetActive(true);
        end.SetActive(false);
        setCategory(UnityEngine.Random.Range(0, 5));
    }

    private void Update()
    {
        if (click)
        {
            model.Rotate(0, speed * Time.deltaTime, 0);
        }
    }

    public void setChoice(int i)
    {
        i = i + (choices.Length * (page - 1));
        if (category == 0)
        {
            setNewTexture(hairRend, hair[i]);
        }
        else if (category == 1)
        {
            setNewTexture(faceRend, face[i]);
        }
        else if (category == 2)
        {
            setNewTexture(clothingRend, clothing[i]);
        }
        else if (category == 3)
        {
            setNewTexture(accessoryRend, accessory[i]);
        }
    }

    private void setNewTexture(Renderer[] rend, Texture newtexture)
    {
        foreach (Renderer r in rend)
        {
            r.material.SetTexture("_MainTex", newtexture);
        }

    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public void onClick()
    {
        click = true;
    }

    public void exit()
    {
        this.speed = 0;
        click = false;
    }


    public void setCategory(int category)
    {
        setCategory(category, 1);
    }
    public void setCategory(int category, int page)
    {
        this.category = category;
        this.page = page;
        if (category == 0)
        {
            setSprite(hair, hairSprite);
        }
        else if (category == 1)
        {
            setSprite(face, faceSprite);
        }
        else if (category == 2)
        {
            setSprite(clothing, clothingSprite);
        }
        else if (category == 3)
        {
            setSprite(accessory, accssorySprite);
        }
    }

    private void setSprite(Texture[] t, Sprite[] s)
    {
        if (t.Length > 0 & (t.Length == s.Length))
        {
            for (int i = 0; i < choices.Length; i++)
            {
                if (i + (choices.Length * (page - 1)) < t.Length)
                {
                    choices[i].gameObject.SetActive(true);
                    choices[i].image.sprite = s[i + (choices.Length * (page - 1))];
                }
                else
                {
                    choices[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void save()
    {
        begin.SetActive(false);
        end.SetActive(true);
    }

    public void yes()
    {
        print("salvou");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void no()
    {
        begin.SetActive(true);
        end.SetActive(false);
    }

    public void setPage(int i)
    {
        page = i > 0 ? page + 1 : page - 1;
        int aux = -1;
        if (category == 0)
        {
            aux = hairSprite.Length;
        }
        else if (category == 1)
        {
            aux = faceSprite.Length;
        }
        else if (category == 2)
        {
            aux = clothingSprite.Length;
        }
        else if (category == 3)
        {
            aux = accssorySprite.Length;
        }

        float c = (float)aux / (float)choices.Length;

        if (c - (int)c > 0.5f)
        {
            c = ((int)c) + 1;
        }
        else
        {
            c = (int)c;
        }


        if (page > c)
        {
            page = (int)c;
        }
        else if (page < 1)
        {
            page = 1;
        }

        setCategory(this.category, this.page);

    }
}