using System;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

namespace Ateneo
{
    public class PauseController : MonoBehaviour
    {
        public Text m_zoneName;
        public Text m_time;
        public ZoneManager m_zoneMan;

        [DllImport("__Internal")]
        private static extern void OpenBrowserNewTab(string url);

        void Awake()
        {
            m_zoneName = transform.Find("Content/Details/ZoneName/Text").GetComponent<Text>();
            m_time = transform.Find("Content/Details/PlayTime/Text").GetComponent<Text>();
            m_zoneMan = GameObject.FindWithTag("CanvasUI").GetComponent<ZoneManager>();
        }

        void OnEnable()
        {
            m_zoneName.text = m_zoneMan.CurrentZone.name;
            m_time.text = BeautifySecs(Time.time);
        }

        public void OpenBrowser(string url)
        {
            OpenBrowserNewTab(url);
        }

        private string BeautifySecs(float secs)
        {
            if (secs < 60)
                return "Justo Ahora";            
            if (secs < 120)
                return "1 Minuto";            
            if (secs < 3600)
                return string.Format("{0} Minutos", Math.Floor(secs / 60));            
            if (secs < 7200)
                return "1 Hora";
            if (secs < 86400)
                return string.Format("{0} Horas", Math.Floor(secs / 3600));

            return null;
        }
    }    
}
