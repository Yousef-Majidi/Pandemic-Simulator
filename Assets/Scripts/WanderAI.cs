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

    private Animator _animator;

    public bool IsWalking()
    {
        return _isWalking;
    }
    // Called when object is activated
    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
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
            _animator.SetBool("isWalking", _isWalking);
            transform.Rotate(transform.up * Time.deltaTime * _rotSpeed);
        }
        if (_isRotatingLeft == true)
        {
            _animator.SetBool("isWalking", _isWalking);
            transform.Rotate(transform.up * Time.deltaTime * -(_rotSpeed));
        }
        if (_isWalking == true)
        {
            _animator.SetBool("isWalking", _isWalking);
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

/*public class Mover : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    private Transform targetWaypoint;

    private int targetIndex = 0;

    private float minDist = 0.5f;

    private int lastIndex;

    private float moveSpeed = 5.0f;
    private float rotateSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        lastIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetIndex];
    }

    // Update is called once per frame
    void Update()
    {


        Quaternion rotationTarget = Quaternion.LookRotation(targetWaypoint.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, rotateSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        checkDistance(distance);
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
    }

    void checkDistance(float currDist)
    {
        if(currDist < minDist)
        {
            targetIndex++;
            UpdateTarget();
        }
    }

    void UpdateTarget()
    {
        if(targetIndex > lastIndex)
        {
            targetIndex = 0;
        }
        targetWaypoint = waypoints[targetIndex];
    }

}
*/