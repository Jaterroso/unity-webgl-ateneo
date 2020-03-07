using UnityEngine;

namespace Ateneo {
    public class Respawn : MonoBehaviour
    {
        private UIManager m_uiMan;

        void Awake()
        {
            m_uiMan = GameObject.FindWithTag("CanvasUI").GetComponent<UIManager>();
        }

        void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player")
            {
                m_uiMan.InvokeRespawn();
            }
        }
    }
}