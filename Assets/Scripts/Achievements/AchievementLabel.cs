using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementLabel : MonoBehaviour {

    [SerializeField]
    private Text title, description;

    private bool test;

    private void Start()
    {
        StartCoroutine(this.Timer());
    }

    private IEnumerator Timer()
    {
        while (!this.test)
        {
            this.test = true;
            yield return new WaitForSeconds(FindObjectOfType<GlobalOptions>().AchievementPopupDuration);
        }
        Destroy(this.gameObject);
    }

    public string Description
    {
        get
        {
            return description.text;
        }

        set
        {
            description.text = value;
        }
    }

    public string Title
    {
        get
        {
            return title.text;
        }

        set
        {
            title.text = value;
        }
    }
}
