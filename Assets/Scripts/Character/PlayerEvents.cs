using System;
using UnityEngine;

namespace Ateneo
{
    public class PlayerEvents : MonoBehaviour
    {
        private FirstPersonController m_character;
        private Camera m_camera;
        private Animator m_animPause;
        private Transform m_cameraPause;
        private Transform m_cameraParent;
        private Animator m_anim;

        void Awake()
        {
            m_character = GetComponent<FirstPersonController>();
            m_anim = GetComponent<Animator>();
            m_animPause = GameObject.FindWithTag("PauseCamera").GetComponent<Animator>();
            m_cameraPause = m_animPause.transform.Find("CameraContainer");

            UIManager uiMan = GameObject.FindWithTag("CanvasUI").GetComponent<UIManager>();

            // Subscribe to events
            uiMan.PauseRequest += OnPauseRequest;
            uiMan.PauseEnter += OnPauseEnter;
            uiMan.PauseExit += OnPauseExit;
            uiMan.Respawn += OnRespawn;
            uiMan.Overlay += OnOverlay;
        }

        void Start()
        {
            m_camera = m_character.CurrentCamera;
            m_cameraParent = m_camera.transform.parent;
        }

        private void OnPauseRequest(object sender, bool active)
        {
            if(active)
            {
                m_character.enabled = false;
                m_anim.SetTrigger("PauseIn");
            }
            else
            {
                // Resume the global audio listener
                AudioListener.pause = false;
            }
        }

        private void OnPauseEnter(object sender, EventArgs e)
        {
            m_camera.transform.SetParent(m_cameraPause);

            // After change the camera parent. Transform gets changed as trying to keep the original orientation.
            // So we reset those values
            resetCameraTransform();

            m_camera.farClipPlane = 12000f; // Magic Value
            m_animPause.enabled = true;

            // Pause the global audio listener
            AudioListener.pause = true;
        }

        private void OnPauseExit(object sender, EventArgs e)
        {
            m_camera.transform.SetParent(m_cameraParent);
            resetCameraTransform();
            m_camera.farClipPlane = 750f; // Magic Value
            m_animPause.enabled = false;
            m_character.enabled = true;
        }

        private void OnRespawn(object sender, bool isActive)
        {            
            if(isActive)
            {
                m_character.enabled = false;
            }
            else 
            {                
                transform.position = m_character.RespawnCoords;
                m_character.enabled = true;
            }
        }

        private void OnOverlay(object sender, bool isActive)
        {            
            m_character.enabled = !isActive;
        }

        private void resetCameraTransform()
        {
            m_camera.transform.localPosition = Vector3.zero;
            m_camera.transform.localRotation = new Quaternion(0, 0, 0, 0);
            m_camera.transform.localScale = Vector3.one;
        }
    }
}
