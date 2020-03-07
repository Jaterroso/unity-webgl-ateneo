using UnityEngine;

namespace Ateneo
{
    public class ZoneDetector : MonoBehaviour
    {
        public SingleUnityLayer zoneLayer;
        public SingleUnityLayer paintLayer;
        
        private ZoneManager m_zoneMan;
        private PaintManager m_paintMan;

        void Awake() {   
            GameObject go = GameObject.FindWithTag("CanvasUI");         
            m_zoneMan = go.GetComponent<ZoneManager>();
            m_paintMan = go.GetComponent<PaintManager>();
        }

        void OnTriggerEnter(Collider coll)
        {
            if(coll.gameObject.layer == zoneLayer.LayerIndex)
            {
                Zone zone = coll.gameObject.GetComponent<ZonePortal>().GetZoneByColl(coll);
                m_zoneMan.SetCurrentZone(zone);
            }
            else if(coll.gameObject.layer == paintLayer.LayerIndex)
            {
                Sprite tex = coll.gameObject.GetComponent<Paint>().image;
                m_paintMan.SetSprite(tex);
            }
        }

        void OnTriggerExit(Collider coll)
        {
            if(coll.gameObject.layer == paintLayer.LayerIndex)
            {
                m_paintMan.SetSprite(null);
            }
        }
    }
}