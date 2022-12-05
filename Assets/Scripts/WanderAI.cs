using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderAI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Character target waypoint index")]
    private int _targetIndex = 0;

    [SerializeField]
    [Tooltip("Minimum distance to waypoint before character moves to the next waypoint")]
    private float _minDist = 0.5f;

    [SerializeField]
    [Tooltip("Character Movement Speed")]
    private float _moveSpeed = 3f;

    [SerializeField]
    [Tooltip("Character Rotation Speed")]
    private float _rotSpeed = 100f;

    public List<Transform> _waypoints = new List<Transform>();
    private Transform _targetWaypoint;
    private int _lastIndex;

    private bool _isWalking = true;

    private Animator _animator;

    public bool IsWalking()
    {
        return _isWalking;
    }

    void checkDistance(float currDist)
    {
        if (currDist < _minDist)
        {
            
            _targetIndex++;
            UpdateTarget();
        }
    }

    void UpdateTarget()
    {
        if (_targetIndex > _lastIndex)
        {
            _targetIndex = 0;
        }
        _targetWaypoint = _waypoints[_targetIndex];
    }

    // Called when object is activated
    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _lastIndex = _waypoints.Count - 1;
        _targetWaypoint = _waypoints[_targetIndex];
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("IsWalking", _isWalking);
        // set the rotation target of the character
        Quaternion rotationTarget = Quaternion.LookRotation(_targetWaypoint.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, _rotSpeed * Time.deltaTime);

        // set the movement target of the character
        float distance = Vector3.Distance(transform.position, _targetWaypoint.position);
        checkDistance(distance);
     
        if (_isWalking == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetWaypoint.position, _moveSpeed * Time.deltaTime);
        }
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