using System.Collections;
using UnityEngine;

public class WanderAI : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Character Movement Speed")]
    private float _moveSpeed = 3f;

    [Space]

    [SerializeField]
    [Tooltip("Character Rotation Speed")]
    private float _rotSpeed = 100f;

    private bool _isWandering = false;
    private bool _isRotatingLeft = false;
    private bool _isRotatingRight = false;
    private bool _isWalking = false;

    public bool IsWalking()
    {
        return _isWalking;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if (_isRotatingRight == true)
        {
            gameObject.GetComponent<Animator>().Play("IdleNormal");
            transform.Rotate(transform.up * Time.deltaTime * _rotSpeed);
        }
        if (_isRotatingLeft == true)
        {
            gameObject.GetComponent<Animator>().Play("IdleNormal"); 
            transform.Rotate(transform.up * Time.deltaTime * -(_rotSpeed));
        }
        if (_isWalking == true)
        {
            gameObject.GetComponent<Animator>().Play("WalkFWD");
            transform.position += transform.forward * _moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator Wander()
    {
        int rotTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 4);
        int rotateLorR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 5);
        int walkTime = Random.Range(1, 6);

        _isWandering = true;

        yield return new WaitForSeconds(walkWait);
        _isWalking = true;
        yield return new WaitForSeconds(walkTime);
        _isWalking = false;
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1)
        {
            _isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            _isRotatingRight = false;
        }
        if (rotateLorR == 2)
        {
            _isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            _isRotatingLeft = false;
        }
        _isWandering = false;
    }
}