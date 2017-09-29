using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StatusPlate
{
    HAND,
    DROP,
    TABLE,
}

public class cPlateQuest : PlayerControllerDefault
{
    private GameObject plateObj;

    private Animator animator;

    private qPlateTable table;

    [SerializeField]
    private PlateType plateType;

    [SerializeField]
    private StatusPlate statusPlate;

    [SerializeField]
    private string layerName, aniDeliveryName, ExitTriggerName,TableTriggerName;

    public override void Setup()
    {
        base.Setup();
        this.JumpEnabled = false;
        animator = quest.Player.GetComponentInChildren<Animator>();
        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 0);
    }

    public override void _Update()
    {
        base._Update();
        if(Input.GetAxis(Inputs.DropPlate.ToString()) > 0)
        {
            DropPlate();
        }

        ControllAnimation();
    }

    public override void _FixedUpdate()
    {
        if (statusPlate != StatusPlate.TABLE & !(Input.GetAxisRaw(Inputs.Interact.ToString()) > 0))
        {
            base._FixedUpdate();
        }
        else
        {
            this.StopPlayer();
        }
    }    

    public void GetPlate(PlateType plateType, GameObject plateObj)
    {
        this.plateType = plateType;
        DestroyPlate();
        this.plateObj = Instantiate(plateObj, quest.Player.HandLeft.position, quest.Player.HandLeft.rotation);
        this.plateObj.transform.parent = quest.Player.HandLeft;
        this.plateObj.transform.Rotate(new Vector3(180, 0, 0));
        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
    }

    public void DeliveryPlate(qPlateTable table)
    {
        this.table = table;
        LeavePlate(StatusPlate.TABLE);
        this.plateType = PlateType.NULL;
    }

    public void DropPlate()
    {
        if (this.plateType != PlateType.NULL)
        {
            this.plateType = PlateType.NULL;
            if (this.plateObj != null)
            {
                DestroyPlate();
                GameObject g = Instantiate(this.plateObj);
                g.AddComponent<qPlateDestroyPlate>();
                g.GetComponent<qPlateDestroyPlate>().Setup();
                g.GetComponent<qPlateDestroyPlate>().enabled = true;
                g.transform.position = this.plateObj.transform.position;
                g.transform.localScale = this.plateObj.transform.lossyScale;
                g.transform.rotation = this.plateObj.transform.rotation;
                LeavePlate(StatusPlate.DROP);
                if (g.GetComponent<Rigidbody>())
                {
                    g.GetComponent<Rigidbody>().useGravity = true;
                    g.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    g.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    g.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
                    g.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
                    g.transform.parent = null;
                }
            }
        }
    }

    private void LeavePlate(StatusPlate statusPlate)
    {
        this.statusPlate = statusPlate;

        if(this.statusPlate == StatusPlate.TABLE)
        {
            animator.SetTrigger(TableTriggerName);
        }
    }

    private void DestroyPlate()
    {
        Destroy(plateObj);

        if (table != null)
        {
            table.DropPlate();
            table = null;
        }
        
        statusPlate = StatusPlate.HAND;
        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 0);
        animator.SetTrigger(ExitTriggerName);
    }

    private void ControllAnimation()
    {
        if (statusPlate == StatusPlate.TABLE)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(aniDeliveryName) & animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                DestroyPlate();
            }
        }
        else if (statusPlate == StatusPlate.DROP)
        {
           
        }
    }

    public PlateType PlateType
    {
        get
        {
            return plateType;
        }
    }    
}
