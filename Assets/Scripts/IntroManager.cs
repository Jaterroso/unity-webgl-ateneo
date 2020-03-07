using UnityEngine;
using UnityEngine.Video;

namespace Ateneo
{
	public class IntroManager : MonoBehaviour
	{
		//
		// Skip Intro Modes
		// None: Normal
		// Intro: Skip the intro video
		// All: Skip the intro video and the welcome panel.
		//
		public enum SkipModes {None, Intro, All}

		public SkipModes Skip;
		public bool IsFinished { get { return m_introFinished; } }
		
		private GameObject m_introPanel;
		private VideoPlayer m_videoPlayer;
		private UIManager m_uiMan;
		private Animator m_canvasAnim;
		private bool m_introFinished;

		void Awake() 
		{
			m_canvasAnim = GetComponent<Animator>();			
			m_uiMan = GetComponent<UIManager>();			
			m_introPanel = transform.Find("IntroPanel").gameObject;
			m_videoPlayer = m_introPanel.GetComponentInChildren<VideoPlayer>();
			m_videoPlayer.loopPointReached += onVideoPlaybackEnd;

			if(Skip >= SkipModes.Intro) 
				onVideoPlaybackEnd(null);
			else 
				m_introPanel.SetActive(true);
		}

		void Start()
		{			
			m_videoPlayer.Prepare();
		}

		private void onVideoPlaybackEnd(VideoPlayer vp)
		{
			m_canvasAnim.SetTrigger("PlaybackEnd");
			m_introFinished = true;
		}

		public void onPlaybackStateExit()
		{			
			// No queremos que IntroPanel.isActive pase a la cadena de Defaults del Animator
			m_introPanel.SetActive(false);
        	SetWelcomePhase(1);
			
			if(Skip == SkipModes.All)
				m_uiMan.TogglePanel(UIManager.Panels.WelcomeClose);
		}

		public void SkipIntro()
		{
			if(!m_introFinished) onVideoPlaybackEnd(null);
		}

		public void SetWelcomePhase(int phase)
		{
			m_canvasAnim.SetInteger("WelcomePhase", phase);
		}
	}
}