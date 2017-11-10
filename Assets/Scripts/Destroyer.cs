using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == GameController.Tags.Player.ToString())
        {
            // Supporting.Log("Player died from collision with: " + gameObject.name);
            GameController.instance.character.GetComponent<Runner>().Die();
        }

        if (coll.gameObject.tag == GameController.Tags.SceneBlock.ToString())
        {
            // as a SceneBlock is left behind the player, generate a new one
            // Supporting.Log("Leaving " + coll.name + " behind");
            GameController.instance.SpawnNewBlockScene(coll.gameObject);
        }
    }
}
