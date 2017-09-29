using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cHideQuest : PlayerControllerDefault
{
    

    public override void _Update()
    {
        if(Input.GetAxis(Inputs.Interact.ToString()) > 0)
        {
            callGirl();
        }
    }


    /// <summary>
    /// chama a função da garota de ir para o próximo ponto
    /// </summary>
    private void callGirl()
    {
        FindObjectOfType<sHideQuest>().Agent.GoTo();
    }
}
