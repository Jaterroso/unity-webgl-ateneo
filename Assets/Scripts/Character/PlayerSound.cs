using UnityEngine;

namespace Ateneo
{    
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(AudioReverbFilter))]
    [RequireComponent(typeof(Animator))]
    public class PlayerSound : MonoBehaviour
    {
        public SingleUnityLayer dirtyLayer;
        public AudioClip DefaultWalk;
        public AudioClip DirtWalk;

        private AudioSource m_audio;
        private AudioListener m_listener;
        private AudioReverbFilter m_audioFilter;
        private Animator m_anim;
        private bool m_audioActive;
        
        void Awake()
        {        
            m_audio = GetComponent<AudioSource>();
            m_listener = GetComponent<AudioListener>();
            m_audioFilter = GetComponent<AudioReverbFilter>();
            m_anim = GetComponent<Animator>();

            m_audio.clip = DefaultWalk;

            ZoneManager zoneMan = GameObject.FindWithTag("CanvasUI").GetComponent<ZoneManager>();
            zoneMan.Transition += OnZoneTransition;
        }

        void OnTriggerEnter(Collider col)
        {
            EvaluateDirty(col);
        }

        void OnTriggerExit(Collider col)
        {
            EvaluateDirty(col, true);
        }

        public void SetActive(bool val)
        {
            if(m_audioActive == val) return;

            if(val)
                m_audio.Play();
            else
                m_audio.Pause();
            
            m_anim.SetBool("Walking", val);
            m_audioActive = val;
        }        

        private void EvaluateDirty(Collider col, bool exiting = false)
        {
            if(col.gameObject.layer == dirtyLayer.LayerIndex)
            {
                m_audio.clip = exiting ? DefaultWalk : DirtWalk;
                m_audio.Play();
            }
        }

        private void OnZoneTransition(object sender, bool isInside)
        {
            m_audioFilter.enabled = isInside;
        }        
    }
}