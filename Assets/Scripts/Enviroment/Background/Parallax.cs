using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject cameraPlayer;
    private float _width, _startPos;
    public float parallax;
    

    void Start()
    {
        _startPos = transform.position.x;
        _width = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cameraPlayer.transform.position.x * (1 - parallax));
        float dist = (cameraPlayer.transform.position.x * parallax);

        transform.position = new Vector3(_startPos + dist, transform.position.y, transform.position.z);

        if (temp > _startPos + _width)
        {
            _startPos += _width;
        }
        else if (temp < _startPos - _width)
        {
            _startPos -= _width;
        }

    }
}
