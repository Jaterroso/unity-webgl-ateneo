using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Ateneo
{
    public class ZonePanelController : MonoBehaviour
    {
        private Text m_zoneTitle;
        private TextMeshProUGUI m_zoneDescription;
        private RawImage m_zoneImage;
        private ZoneManager m_zoneMan;
        private ZoneDetails m_curZone;
        
        void Awake()
        {
            m_zoneMan = gameObject.GetComponentInParent<ZoneManager>();
            m_zoneTitle = transform.Find("Content/TextContent/Title").GetComponent<Text>();
            m_zoneDescription = transform.Find("Content/TextContent/Description").GetComponent<TextMeshProUGUI>();
            m_zoneImage = transform.Find("Content/Image").GetComponent<RawImage>();
        }

        void OnEnable()
        {
            if(m_curZone != m_zoneMan.CurrentZone)
            {
                m_curZone = m_zoneMan.CurrentZone;
                Texture tex = Resources.Load<Texture>(m_curZone.imageUrl);
                m_zoneTitle.text = m_curZone.name;
                m_zoneDescription.text = m_curZone.description;
                m_zoneImage.texture = tex;
            }
        }
    }    
}
