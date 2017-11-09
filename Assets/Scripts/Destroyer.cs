using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == GameController.Tags.Player.ToString())
        {
            GameController.instance.character.GetComponent<Runner>().Die();
            Supporting.Log("Player died from collision with: " + gameObject.name);
        }
    }
}
