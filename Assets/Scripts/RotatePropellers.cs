using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePropellers : MonoBehaviour
{
    public List<GameObject> propellers;
    public int speed;
    void Update()
    {
        foreach (var item in propellers)
        {
            item.transform.Rotate(new Vector3(0,speed*Time.deltaTime,0));
        }
    }
}
