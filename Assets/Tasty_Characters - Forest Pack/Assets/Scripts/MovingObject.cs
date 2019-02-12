using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;          
    public LayerMask blockingLayer;         


    private BoxCollider2D boxCollider;      
    private Rigidbody2D rb2D;               
    private float inverseMoveTime;
    

    protected virtual void Start()
    {

        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        Move();
    }


    protected void Move()
    {
        Vector2 start = transform.position;

        Vector2 end = new Vector2(-Screen.width/2 + 100, -Screen.height/2);
        
        StartCoroutine(SmoothMovement(end));
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            
            yield return null;
        }
    }
}