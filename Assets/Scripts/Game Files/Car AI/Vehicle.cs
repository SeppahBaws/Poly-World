using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TestingDirection
{
    Left,
    Right,
    Forward
}

public class Vehicle : MonoBehaviour
{
    private Animator _animator;


    public List<Tile> Path;  // List of Tiles that hold the path that this vehicle is traveling
    [SerializeField]
    private int _currentWaypoint = -1;
    private int _nextWaypoint;
    private Tile _targetWaypoint;

    //TODO: different speeds on different roads -> read speed data from the current tile
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotateSpeed = 5f;

    private void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();

        if (Path != null)
        {
            Vector3 firstPos = Path[0].AdjustedWorldPosition();
            transform.rotation = Quaternion.LookRotation(firstPos - transform.position);
        }
    }

    private void Update()
    {
        if (Path == null)
            return;

        if (_currentWaypoint < Path.Count)
        {
            if (_targetWaypoint == null)
                _targetWaypoint = Path[0];
            ObsoleteWalk();
        }
    }

    private void Walk()
    {
        Vector3 nextWaypoint = _targetWaypoint.AdjustedWorldPosition();

        TestingDirection direction = FrontTest(transform, nextWaypoint, 0.1f);

        transform.position = Path[_currentWaypoint].AdjustedWorldPosition();
        transform.rotation = Quaternion.LookRotation(transform.position - nextWaypoint);

        switch (direction)
        {
            case TestingDirection.Forward:
                // Play forward animation
                _animator.SetTrigger("Forward");
                Debug.Log("Forward");

                // Set car position to the next tile
                //transform.position = nextWaypoint;
                break;

            case TestingDirection.Right:
                // Play corner right animation
                _animator.SetTrigger("CornerRight");
                Debug.Log("Right");

                //TODO: Skip car position by 1 tile, since the animation takes care of that.

                transform.LookAt(nextWaypoint);

                transform.rotation = Quaternion.LookRotation(nextWaypoint - transform.position);
                //transform.position = nextWaypoint;
                break;

            case TestingDirection.Left:
                // Play corner left animation
                //_animator.SetTrigger("CornerLeft");
                Debug.Log("Left");

                //TODO: Skip car position by 1 tile, since the animation takes care of that.

                transform.rotation = Quaternion.LookRotation(nextWaypoint - transform.position);
                //transform.position = nextWaypoint;
                break;
        }

        _currentWaypoint++;
        if (_currentWaypoint == Path.Count)
        {
            Path = null;
            Destroy(gameObject);
        }
        else
        {
            _targetWaypoint = Path[_currentWaypoint];
        }
    }
    
    /// <summary>
    /// Returns whether a position is in front, to the left or to the right of another position.
    /// </summary>
    private TestingDirection FrontTest(Transform obj1, Vector3 obj2, float fwdThres)
    {
        Vector3 vec = obj2 - obj1.position;
        vec.Normalize();
        float val = Vector3.Dot(obj1.right, vec);

        if (val > fwdThres)
            return TestingDirection.Right;

        if (val < -fwdThres)
            return TestingDirection.Left;

        return TestingDirection.Forward;
    }


    private void ObsoleteWalk()
    {
        Vector3 nextWaypoint = new Vector3(_targetWaypoint.X + 0.5f, 0, _targetWaypoint.Y + 0.5f);

        /**
         * TODO: replace the coded rotations by an animation
         * instead of smoothly interpolating the vehicle from position to position,
         * make it "hop" from tile to tile, and use animations to make it
         * drive and going around corners
         */

        // Rotate towards the waypoint
        transform.forward = Vector3.RotateTowards(transform.forward, nextWaypoint - transform.position,
            _rotateSpeed * Time.deltaTime, 0.0f);

        // move towards the targetWaypoint
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, _moveSpeed * Time.deltaTime);

        float distanceToTarget = Vector3.Distance(transform.position, _targetWaypoint.AdjustedWorldPosition());

        /**
         * if the car has to turn right, then we are supposed to start turning a bit earlier.
         * so we calculate the dot product of the waypoint's position with the right vector of the vehicle:
         * if dot is positive, the waypoint is on the right,
         * if dot is negative, waypoint is on the left
         *
         * QUICK MAFS
         */
        Vector3 vehicleRight = Vector3.Cross(Vector3.up, transform.forward);
        float dot = Vector3.Dot(vehicleRight, _targetWaypoint.AdjustedWorldPosition());

        //if ((dot <= 0 && distanceToTarget <= 0.01f) ||   // turn to left
        //    (dot > 0 && distanceToTarget <= 0.15f))       // turn to right
        if (distanceToTarget <= 0.05f)
        {
            _currentWaypoint++;
            if (_currentWaypoint == Path.Count)
            {
                Path = null;
                Destroy(gameObject);
            }
            else
            {
                _targetWaypoint = Path[_currentWaypoint];
            }
        }
    }
}