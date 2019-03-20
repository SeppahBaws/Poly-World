using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private GameObject _carPrefab;
    [SerializeField]
    private float _tilesPerSecond = 1f;

    private float _accumulatedTime = 0f;
    private GameObject _spawnedCarObject;
    private Animator _animator;
    
    private void Start()
    {
        // Instantiate
        _spawnedCarObject = Instantiate(_carPrefab, new Vector3(0.5f, 0, 0.5f), Quaternion.identity);

        // Fetch the Animator from the GameObject
        _animator = _spawnedCarObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        _accumulatedTime += Time.deltaTime;

        if (_accumulatedTime > 1 / _tilesPerSecond)
        {
            // do movement stuff here
            //m_SpawnedCarObject.transform.position += Vector3.right;
            //m_Animator.SetTrigger("CornerRight");

            _accumulatedTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _animator.SetBool("CornerLeft", true);
            return;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _animator.SetBool("CornerLeft", false);
            return;
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _animator.SetBool("CornerRight", true);
            return;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            _animator.SetBool("CornerRight", false);
            return;
        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _animator.SetBool("Forward", true);
            return;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            _animator.SetBool("Forward", false);
            return;
        }
    }
}
