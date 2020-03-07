using UnityEngine;
using UnityEngine.UI;

namespace Ateneo
{
    public class PaintPanelController : MonoBehaviour
    {
        private PaintManager m_paintMan;
        private Image m_image;
        
        void Awake()
        {
            m_paintMan = gameObject.GetComponentInParent<PaintManager>();
            m_image = GetComponent<Image>();
        }

        void OnEnable()
        {
            m_image.sprite = m_paintMan.Sprite;
        }
    }    
}
