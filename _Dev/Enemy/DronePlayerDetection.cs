using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePlayerDetection : MonoBehaviour
{
   [SerializeField] private Drone _droneController;
   private void OnTriggerEnter(Collider other)
   {
      EventManager.Broadcast(GameEventsHandler.PlayerTakeDamageEvent);
      _droneController.Explode();
   }
}
