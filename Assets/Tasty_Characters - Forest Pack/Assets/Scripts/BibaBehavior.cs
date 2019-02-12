using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BibaBehavior : MonoBehaviour
{
    public GameObject biba;
    public GameObject catchEffect;
    public GameObject bloodEffect;
    public Transform pointOfCatchEffect;
    public GameObject tailSystem;
    
    public AudioClip deadClip;
    public AudioClip magicClip;

    void LateUpdate()
    {
        if (!GameManager.isGameGoing)
        {
            KillMe();
        }
    }

    void KillMe()
    {
        Vector3 coord = pointOfCatchEffect.position;
        Instantiate(bloodEffect, coord + new Vector3(0, 0.5f, 0), Quaternion.identity);

        System.Random rand = new System.Random();
        foreach (GameObject bound in GameManager.skillet)
        {
            Instantiate(bound, coord, Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector3(-2f + (float)rand.NextDouble() * 4f, 4f, 0), ForceMode2D.Impulse);
        }

        Destroy(biba);
    }

    void OnMouseDown ()
    {
        if (GameManager.isGameGoing)
        {
            Instantiate(tailSystem, pointOfCatchEffect.position, Quaternion.identity);
            DestroyImmediate(biba);
            GameManager.audioPlayer.PlayOneShot(magicClip);

            GameManager.catchedBibs += (GameManager.carma / 7 + 1);
            GameManager.carma++;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "ground")
        {
            KillMe();
            GameManager.audioPlayer.PlayOneShot(deadClip);
            GameManager.carma -= 7;
            if (GameManager.carma < 0)
                GameManager.carma = 0;
            if (GameManager.carma <= 0 && GameManager.isGameGoing)
            {
                GameManager.LoseGame();
            }
        }
    }
}
