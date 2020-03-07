using UnityEngine;

namespace Ateneo
{
	public class FogController : MonoBehaviour
	{
		public float TargetDensity;
		public float RecoveryRate;
		public bool IsActive { get { return m_active; } }
		
		private bool m_active;
		private float m_fogDensity;
		private ZoneManager m_zoneMan;

		void Awake()
		{
			m_fogDensity = RenderSettings.fogDensity;
			m_zoneMan = GetComponent<ZoneManager>();
			m_zoneMan.Transition += OnZoneTransition;
		}

		public void SetActive(bool val)
		{    	
			m_fogDensity = RenderSettings.fogDensity;
			m_active = val;
			StartCoroutine(StartFade());
		}

		private IEnumerator StartFade()
        {
            while (m_active ? (m_fogDensity < TargetDensity) : (m_fogDensity > 0))
            {
				m_fogDensity = m_active ? 
					Mathf.MoveTowards(m_fogDensity, TargetDensity, RecoveryRate * Time.deltaTime) : 
					Mathf.MoveTowards(m_fogDensity, 0f, RecoveryRate * Time.deltaTime);
				
				RenderSettings.fogDensity = m_fogDensity;
                yield return null;
            }
            yield break;
        }

		private void OnZoneTransition(object sender, bool isInside)
        {
            if (m_active == isInside)
                SetActive(!m_active);
        }
	}
}
