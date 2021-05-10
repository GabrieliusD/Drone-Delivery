using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;
public class DroneCameraController : NetworkBehaviour
{
public Vector2 maxFollowOffset = new Vector2(-1f, 6f);
    public Vector2 cameraVelocity = new Vector2(4f, 0.25f);
    public Transform playerTransform;
    public CinemachineVirtualCamera virtualCamera;

    private CinemachineTransposer transposer;

    Controls controls;
    private Controls Controls
    {
        get
        {
            if (controls != null) return controls;
            return controls = new Controls();
        }
    }

    public override void OnStartAuthority()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        virtualCamera.gameObject.SetActive(true);
        enabled = true;
        Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());


    }


    public void Look(Vector2 lookAxis)
    {
        float deltaTime = Time.deltaTime;
        float followOfset = Mathf.Clamp(
            transposer.m_FollowOffset.y - (lookAxis.y * cameraVelocity.y * deltaTime),
            maxFollowOffset.x, maxFollowOffset.y
        );

        transposer.m_FollowOffset.y = followOfset;

        playerTransform.Rotate(0f, lookAxis.x * cameraVelocity.x * deltaTime, 0f);
    }

}
