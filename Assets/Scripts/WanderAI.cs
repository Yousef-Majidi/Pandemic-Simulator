using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI
;

public class WanderAI : MonoBehaviour
{
    // TODO: Serialize the public objects
    public float _moveSpeed = 3f;
    public float _rotSpeed = 100f;

    private bool _isWandering = false;
    private bool _isRotatingLeft = false;
    private bool _isRotatingRight = false;
    private bool _isWalking = false;

    // Update is called once per frame
    void Update()
    {
        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if (isRotatingRight == true)
        {
            gameObject.GetComponent<Animator>().Play("idle");
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }
        if (isRotatingLeft == true)
        {
            gameObject.GetComponent<Animator>().Play("idle");
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
        if (isWalking == true)
        {
            gameObject.GetComponent<Animator>().Play("waalk");
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator Wander()
    {
        int rotTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 4);
        int rotateLorR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 5);
        int walkTime = Random.Range(1, 6);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        isWandering = false;
    }
}