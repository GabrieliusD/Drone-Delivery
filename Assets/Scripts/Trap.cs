using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mirror;
public class Trap : NetworkBehaviour
{
    public enum TrapType{ Slide, Spin, Other};
    public enum SlideDirection{Up, Down, Left, Right, None};
    public TrapType mTrapType;
    public float mSlideDistance;
    public SlideDirection mSlideDirection;
    public float mSpeed;
    void Update()
    {

        if(isServer)
        {
            switch(mTrapType)
            {
                case TrapType.Slide: SlideTrap(); break;
                case TrapType.Spin: RotateTrap(); break;
                default: break;
            }
        }
    }

    public Vector3 GetSlideDirectionVector()
    {
        switch(mSlideDirection)
        {
            case SlideDirection.Up: return Vector3.up; 
            case SlideDirection.Down: return Vector3.down; 
            case SlideDirection.Left: return Vector3.left;
            case SlideDirection.Right: return Vector3.right;
            default: return Vector3.zero;
        }
    }

    public void SlideTrap()
    {
        float value = Mathf.Lerp(-mSlideDistance, mSlideDistance, Mathf.PingPong(Time.time, 1));
        if(mSlideDirection == SlideDirection.Up)
            transform.position = new Vector3(transform.position.x, value, transform.position.z);
        if (mSlideDirection == SlideDirection.Down)
            transform.position = new Vector3(transform.position.x, -value, transform.position.z);
        if (mSlideDirection == SlideDirection.Left)
            transform.position = new Vector3(value, transform.position.y, transform.position.z);
    }
    public void RotateTrap()
    {
        transform.Rotate(GetSlideDirectionVector() *mSpeed * Time.deltaTime);
    }
}
