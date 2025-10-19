using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        GameManager.singleton.AddScore(2);
        AudioManager.instance?.PlayCollectPoints();
        FindObjectOfType<BallController>().perfectPass++;
    }
}
