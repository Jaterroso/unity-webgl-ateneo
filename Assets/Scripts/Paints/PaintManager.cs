using UnityEngine;

namespace Ateneo
{    
    [RequireComponent(typeof(Animator))]
    public class PaintManager : MonoBehaviour
    {
        public bool IsReady { get { return m_curImage != null; }}
        public Sprite Sprite { get { return m_curImage; }}
        
        private Sprite m_curImage;
        private Animator m_canvasAnim;

        void Awake()
        {
            m_canvasAnim = GetComponent<Animator>();
        }

        public void SetSprite(Sprite image)
        {
            m_curImage = image;
            
            if(IsReady)
            {
                m_canvasAnim.SetTrigger("PaintToastVisible");
            }
        }
    }
}