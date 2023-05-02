using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGLoseZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            BulletMS bms = other.GetComponent<BulletMS>();
            if (!bms.targetHit)
            {
                var evt = GameEventsHandler.TargetDestroyedEvent;
                if (VarSaver.CoinMultiplier <= 1)
                    evt.Multiplier = 1;
                EventManager.Broadcast(evt);
                bms.targetHit = true;
            }
        }
    }
}
