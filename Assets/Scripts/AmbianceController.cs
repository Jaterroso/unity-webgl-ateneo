using UnityEngine;
using UnityEngine.Audio;

namespace Ateneo
{
    public class AmbianceController : MonoBehaviour
    {
        public AudioMixerSnapshot insideState;
        public AudioMixerSnapshot outsideState;
        public AudioMixerSnapshot pauseState;
        public AudioMixerGroup pauseGroup;
        public AudioClip pauseClip;        

        private AudioMixerSnapshot m_curSnapshot;
        private AudioSource m_pauseSource;

        void Awake()
        {
            GameObject go = GameObject.FindWithTag("CanvasUI");
            ZoneManager zoneMan = go.GetComponent<ZoneManager>();
            UIManager uiMan = go.GetComponent<UIManager>();

            // Subscribe to events
            uiMan.Begin += OnBegin;
            uiMan.PauseRequest += onPauseRequest;
            uiMan.PauseExit += onPauseExit;
            zoneMan.Transition += OnZoneTransition;
            
            // Create an audio source for the pause menu
            m_pauseSource = gameObject.AddComponent<AudioSource>();
            m_pauseSource.ignoreListenerPause = true; // Ignore Listener Pause
            m_pauseSource.loop = true;
            m_pauseSource.volume = 1;
            m_pauseSource.clip = pauseClip;
            m_pauseSource.outputAudioMixerGroup = pauseGroup;

            m_curSnapshot = outsideState;
        }

        private void onPauseRequest(object sender, bool active)
        {
            if(active)
            {
                m_pauseSource.Play();
                pauseState.TransitionTo(1f);
            }
            else
            {
                m_curSnapshot.TransitionTo(1f);
            }
        }

        private void onPauseExit(object sender, EventArgs e)
        {
            m_pauseSource.Pause();
        }

        private void OnZoneTransition(object sender, bool isInside)
        {
            m_curSnapshot = isInside ? insideState : outsideState;
            m_curSnapshot.TransitionTo(2f);
        }

        private void OnBegin(object sender, EventArgs e)
        {
            m_curSnapshot.TransitionTo(2f);
        }
    } 
}