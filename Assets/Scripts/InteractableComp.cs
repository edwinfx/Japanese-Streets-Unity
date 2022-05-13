using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPickupType{
    EPT_Can1,
    EPT_Can2,
    EPT_Can3,
    EPC_VendingMachine,
    EPC_SushiMachine
} 

public class InteractableComp : MonoBehaviour
{
    public EPickupType pickupType = EPickupType.EPT_Can1;
    public float amount = 10;
}
