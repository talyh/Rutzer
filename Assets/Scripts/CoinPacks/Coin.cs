using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    [SerializeField]
    private int _points = 10;

    private void Collect()
    {
        GameController.instance.ScorePoints(_points);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == GameController.Tags.Player.ToString())
        {
            Collect();
        }
    }
}
