using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 endPos;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;

    private void Start()
    {
        startPos = transform.position;
        StartCoroutine(MoveToEnd());
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            collision.transform.position = transform.position;
        }

    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == ("Player"))
    //    {
    //        collision.transform.parent = null;
    //    }
    //}



    IEnumerator MoveToEnd()
    {
        transform.position = Vector3.Lerp(transform.position, endPos, speed * Time.deltaTime);
        bool move;
        if (transform.position != startPos)
        {
            move = false;
        }
        else
        {
            move = true;
        }
        if (move == true)
        {
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(MoveToStart());
        }
    }

    IEnumerator MoveToStart()
    {
        transform.position = Vector3.Lerp(transform.position, startPos, speed * Time.deltaTime);
        bool move;
        if (transform.position != startPos)
        {
            move = false;
        }
        else
        {
            move = true;
        }
        if (move == true)
        {
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(MoveToEnd());
        }
        
    }
}
