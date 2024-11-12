using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InGameCameraController : MonoBehaviour
{
    [Header("Instances")]
    [SerializeField] Transform m_CameraTargetTransform = null;
    
    [Header("Transform Settings")]
    [SerializeField] float m_StartingCameraTargetGroundDistance = 20.0f;
    [SerializeField] float m_CameraTargetAngle = 35.0f;
    
    [Header("Panning")]
    [SerializeField] float m_EdgePanDetectionRange = 10.0f;
    [SerializeField] float m_EdgePanSpeed = 2.0f;
    
    [Header("Debug")]
    [SerializeField] bool m_ShowEdgePanDebug = false;
    
    bool m_ShouldFollowChampion = false;
    float m_CameraTargetGroundDistance = 0.0f;
    
    void Start()
    {
        CheckTargetTransform();
        
        m_CameraTargetGroundDistance = m_StartingCameraTargetGroundDistance;

        ControlsManager controlsManager = ControlsManager.Get();
        if (controlsManager == null)
        {
            Debug.LogError("PlayerControlsManager is null");
            return;
        }

        controlsManager.GetInputMapGameplay().Default.ChampionCameraLock.performed += context => ToggleFollowChampion();
    }

    void Update()
    {
        UpdateCameraTargetPosition();
    }

    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    void CheckTargetTransform()
    {
        if (m_CameraTargetTransform != null)
        {
            //We already have target transform, no need to do anything
            return;
        }
        
        Debug.LogError("Cannot find Camera Target Transform child object. Maybe the object name is changed?" +
                       "A new object will be created as a target, but you have to find the cause of the error.");
            
        CreateAndUseTemporaryTargetGameObject();
    }

    void UpdateCameraTargetPosition()
    {
        if (m_CameraTargetTransform == null)
            return;
        
        UpdateCameraPanning();
        FollowOwnChampion();
        
        Vector3 targetPosition = m_CameraTargetTransform.position;
        targetPosition.y = 0;
        
        m_CameraTargetTransform.position = targetPosition;
    }

    void FollowOwnChampion()
    {
        if (m_ShouldFollowChampion)
        {
            //Cache ?
            MatchManager matchManager = MatchManager.Get();
            if (matchManager == null)
            {
                Debug.LogError("MatchManager is null");
            }
            else
            {
                m_CameraTargetTransform.position = matchManager.GetOwnChampionReference().transform.position;
            }
        }
    }
    
    void UpdateCameraPanning()
    {
        if (m_ShouldFollowChampion)
            return;

        ControlsManager controlsManager = ControlsManager.Get();
        if (controlsManager == null)
            return;

        Vector2 mousePosition = controlsManager.GetInputMapGameplay().Default.MousePosition.ReadValue<Vector2>();

        Vector2 panDirection = Vector2.zero;
        
        if (mousePosition.x < m_EdgePanDetectionRange)
        {
            panDirection.x = m_EdgePanSpeed;
        }

        if (mousePosition.x > Screen.width - m_EdgePanDetectionRange)
        {
            panDirection.x = -m_EdgePanSpeed;
        }

        if (mousePosition.y < m_EdgePanDetectionRange)
        {
            panDirection.y = m_EdgePanSpeed;
        }

        if (mousePosition.y > Screen.height - m_EdgePanDetectionRange)
        {
            panDirection.y = -m_EdgePanSpeed;
        }

        if (panDirection.x != 0 && panDirection.y != 0)
        {
            //Reduce diagonal speed
            panDirection *= 0.5f;
        }
        
        panDirection.x = Mathf.Clamp(panDirection.x, -m_EdgePanDetectionRange, m_EdgePanDetectionRange);
        panDirection.y = Mathf.Clamp(panDirection.y, -m_EdgePanDetectionRange, m_EdgePanDetectionRange);
        
        Vector3 currentCameraPosition = m_CameraTargetTransform.position;
        Vector3 destinationCameraPosition = currentCameraPosition + new Vector3(panDirection.x, 0.0f, panDirection.y);
        m_CameraTargetTransform.position = Vector3.MoveTowards(currentCameraPosition, destinationCameraPosition, m_EdgePanSpeed * Time.deltaTime);
    }

    void UpdateCameraPosition()
    {
        if (m_CameraTargetTransform == null)
        {
            Debug.LogError("Camera Target Transform child object is null");
            return;
        }
        
        //For the explanation of this logic, please take a look at the documentation
        
        float hypotenuse = m_CameraTargetGroundDistance / Mathf.Cos(m_CameraTargetAngle * Mathf.Deg2Rad);
        Vector3 hypotenuseVector = Quaternion.AngleAxis(-m_CameraTargetAngle, m_CameraTargetTransform.right) * m_CameraTargetTransform.forward * hypotenuse;
        transform.position = m_CameraTargetTransform.position + hypotenuseVector;
        transform.LookAt(m_CameraTargetTransform);
    }

    void CreateAndUseTemporaryTargetGameObject()
    {
        GameObject targetGameObject = new GameObject("Camera Target");
        Transform targetGOTransform = targetGameObject.transform;
        targetGOTransform.parent = transform;
        m_CameraTargetTransform = targetGOTransform;
    }

    void ToggleFollowChampion()
    {
        m_ShouldFollowChampion = !m_ShouldFollowChampion;
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (m_ShowEdgePanDebug == false)
            return;
        
        Vector2 topLeftCorner = new Vector2(0.0f, 0.0f);
        Vector2 bottomLeftCorner = new Vector2(0.0f, Screen.height - m_EdgePanDetectionRange);
        Vector2 topRightCorner = new Vector2(Screen.width - m_EdgePanDetectionRange, 0.0f);
        
        Vector2 sideEdgeSize = new Vector2(m_EdgePanDetectionRange, Screen.height);
        Vector2 centralEdgeSize = new Vector2(Screen.width, m_EdgePanDetectionRange);
        
        //Top Left -> Bottom left
        GUI.Box(new Rect(topLeftCorner, sideEdgeSize), "Left");
        GUI.Box(new Rect(bottomLeftCorner, centralEdgeSize), "Bottom");
        GUI.Box(new Rect(topRightCorner, sideEdgeSize), "Right");
        GUI.Box(new Rect(topLeftCorner, centralEdgeSize), "Top");
    }
#endif
}
