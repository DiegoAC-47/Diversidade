using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IACarFront : MonoBehaviour {

	private void crash()
    {
        GetComponentInParent<IACar>().closeCar();
    }

    private void crashOut()
    {
        GetComponentInParent<IACar>().awayCar();
    }

    private void OnTriggerStay(Collider collision)
	{
		if (collision.gameObject.GetInstanceID () != gameObject.GetInstanceID ()) {
			if (collision.gameObject.GetComponentInParent<IACar> () || collision.gameObject.GetComponent<Player> ()) {
				crash ();
			} else if (collision.gameObject.GetComponent<PersonSignal> ()) {
				if (collision.gameObject.GetComponent<PersonSignal> ().NeedPass) {
					crash ();
				} else {
					crashOut ();
				}
			}
		}
	}

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<IACar>() || collision.gameObject.GetComponent<Player>())
        {
            crashOut();
        }
    }



}
