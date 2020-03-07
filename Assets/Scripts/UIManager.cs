using System;
using System.Collections;
using UnityEngine;

namespace Ateneo
{
    [RequireComponent(typeof(IntroManager))]
    [RequireComponent(typeof(PaintManager))]
    [RequireComponent(typeof(FogController))]
    public class UIManager : MonoBehaviour
    {
        // UI Panels
        public enum Panels { None, Zone, Paint, WelcomeClose }

        public event EventHandler<Boolean> PauseRequest;
        public event EventHandler PauseEnter;
        public event EventHandler PauseExit;
        public event EventHandler Begin;
        public event EventHandler<Boolean> Respawn;
        public event EventHandler<Boolean> Overlay;

        private Animator m_canvasAnim;
        private IntroManager m_introMan;
        private PaintManager m_paintMan;
        private FogController m_fogCtrl;
        private bool m_onRespawn;
        private bool m_overlayEnabled;
        private bool m_welcomeFinished;
        private bool m_pause;
        private bool m_pauseLock;
        private bool m_retFogEnabled;
        private Panels m_currentPanel;

        void Awake()
        {
            m_introMan = GetComponent<IntroManager>();
            m_paintMan = GetComponent<PaintManager>();
            m_fogCtrl = GetComponent<FogController>();
            m_canvasAnim = GetComponent<Animator>();
            m_currentPanel = Panels.None;
            m_overlayEnabled = m_canvasAnim.GetBool("OverlayVisible");
        }

        void Start()
        {
            Overlay?.Invoke(this, m_overlayEnabled);
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.I))
                TogglePanel((m_paintMan.IsReady || m_currentPanel == Panels.Paint) ? Panels.Paint : Panels.Zone);
            if (Input.GetKeyUp(KeyCode.Escape))
                OnEscapeKey();
            if (Input.GetKeyUp(KeyCode.R))
                InvokeRespawn();
        }

        public void TogglePanel(Panels panel)
        {
            if (panel == Panels.None) return;

            if (panel == m_currentPanel || panel == Panels.WelcomeClose)
            {
                TogglePanel(panel, false);
                SetOverlay(false);
                m_currentPanel = Panels.None;
            }
            else
            {                
                if(m_currentPanel != Panels.None)
                    TogglePanel(m_currentPanel, false);
                else
                    SetOverlay(true);

                TogglePanel(panel, true);
                m_currentPanel = panel;
            }
        }

        public void ClosePanel()
        {
            TogglePanel(m_currentPanel);
        }

        public void CloseWelcome()
        {
            TogglePanel(Panels.WelcomeClose);
            Begin?.Invoke(this, null);
        }

        public void InvokeRespawn()
        {
            if (!m_onRespawn)
            {
                m_onRespawn = true;
                StartCoroutine(OnInvokeRespawn());
            }
        }
        
        private void OnEscapeKey()
        {
            if (!m_introMan.IsFinished)
                return m_introMan.SkipIntro();
            if (!m_welcomeFinished)
                return CloseWelcome();
            if (m_overlayEnabled)
                return TogglePanel(m_currentPanel);
            if (!m_pauseLock)
                return TogglePause();
        }

        private void TogglePanel(Panels panels, bool enter)
        {
            switch (panels)
            {
                case Panels.WelcomeClose:
                    // No importa el estado. Solo se llamara para el cierre
                    m_canvasAnim.SetTrigger("WelcomeClose");
                    m_canvasAnim.SetInteger("WelcomePhase", 0);
                    m_welcomeFinished = true;
                    break;

                case Panels.Zone:
                    m_canvasAnim.SetBool("ZoneVisible", enter);
                    break;

                case Panels.Paint:
                    m_canvasAnim.SetBool("PaintVisible", enter);
                    break;
            }
        }

        private void SetOverlay(bool val)
        {
            if (m_overlayEnabled != val)
            {
                m_overlayEnabled = val;
                m_canvasAnim.SetBool("OverlayVisible", m_overlayEnabled);
                Overlay?.Invoke(this, m_overlayEnabled);
            }
        }

        private void TogglePause()
        {
            StartCoroutine(m_pause ? PauseOut() : PauseIn());
            m_pause = !m_pause;
        }        

        private IEnumerator OnInvokeRespawn()
        {
            Respawn?.Invoke(this, true);
            m_canvasAnim.SetTrigger("OverlayRespawn");
            yield return new WaitForSeconds(1.1f);
            Respawn?.Invoke(this, false);
            m_onRespawn = false;
        }

        private IEnumerator PauseIn()
        {
            PauseRequest?.Invoke(this, true);
            m_pauseLock = true;
            m_retFogEnabled = m_fogCtrl.IsActive;
            m_fogCtrl.SetActive(false);
            yield return new WaitForSeconds(.35f);
            m_canvasAnim.SetBool("OverlayPause", true);
            yield return new WaitForSeconds(1f);
            PauseEnter?.Invoke(this, null);
            yield return new WaitForSeconds(1f);
            m_canvasAnim.SetBool("PauseVisible", true);
            m_pauseLock = false;
        }

        private IEnumerator PauseOut()
        {
            m_pauseLock = true;
            m_canvasAnim.SetBool("PauseVisible", false);
            yield return new WaitForSeconds(1f);
            PauseRequest?.Invoke(this, false);
            m_canvasAnim.SetBool("OverlayPause", false);
            yield return new WaitForSeconds(1f);
            PauseExit?.Invoke(this, null);
            m_fogCtrl.SetActive(m_retFogEnabled);
            m_pauseLock = false;
        }
    }
}
