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
        foreach (GameObject bound in GameManager.instance.skillet)
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

            GameManager.instance.audioPlayer.PlayOneShot(magicClip);
            GameManager.instance.catchedBibs += (GameManager.instance.carma / 7 + 1);
            GameManager.instance.carma++;
            GameManager.instance.UpdateScore();
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "ground")
        {
            KillMe();

            GameManager.instance.audioPlayer.PlayOneShot(deadClip);
            GameManager.instance.carma -= 7;
            GameManager.instance.UpdateScore();

            if (GameManager.instance.carma < 0)
                GameManager.instance.carma = 0;
            if (GameManager.instance.carma <= 0 && GameManager.isGameGoing)
            {
                GameManager.instance.LoseGame();
            }
        }
    }
}
