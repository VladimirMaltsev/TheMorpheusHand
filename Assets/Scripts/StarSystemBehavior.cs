using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemBehavior : MonoBehaviour
{
    public Transform topLeftPoint;
    public Transform downRightPoint;

    public GameObject starPrefab;
    private Queue<GameObject> stars = new Queue<GameObject>();
   

    void Start()
    {
        StartCoroutine(StarShining());
    }
    
    IEnumerator StarShining()
    {
        System.Random rand = new System.Random();
        while (true)
        {
            if (stars.Count > 7)
                Destroy(stars.Dequeue());

            
            float x = (float)rand.NextDouble() * (downRightPoint.position.x - topLeftPoint.position.x) + topLeftPoint.position.x;
            float y = (float)rand.NextDouble() * (topLeftPoint.position.y - downRightPoint.position.y) + downRightPoint.position.y;

            stars.Enqueue(Instantiate(starPrefab, new Vector3(x, y, 0), Quaternion.identity));
            

            yield return new WaitForSeconds(0.8f);
        }
    }
}
