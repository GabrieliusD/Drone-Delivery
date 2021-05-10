using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BoxData : NetworkBehaviour
{
    public string player = "hello";
    public int pointAmount = 10;
    public float boxHealth = 100;
    public float boxFragility = 0.2f;
    public Vector3 offset;
    [SyncVar] public GameObject Parent;
    Rigidbody boxBody;
    private void Start()
    {
        boxBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(Parent != null)
        {
            transform.position = Parent.transform.position - offset;
            transform.rotation = Parent.transform.rotation;
            boxBody.isKinematic = true;
        }else boxBody.isKinematic = false;
    }
 
}
