using UnityEngine;
using UnityEngine.UI;

namespace Ateneo 
{
    public class ZoneToastController : MonoBehaviour
    {
        private Text m_title;
        private Image m_image;

        void Awake()
        {
            m_title = transform.Find("Content/Right/Name").GetComponent<Text>();
            m_image = transform.Find("Content/Image").GetComponent<Image>();
        }

        public void SetZone(ZoneDetails zone)
        {
            Sprite sprite = Resources.Load<Sprite>(zone.iconUrl);
            m_title.text = zone.name;
            m_image.sprite = sprite;
        }
    }
}
