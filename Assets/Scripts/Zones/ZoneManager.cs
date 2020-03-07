using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ateneo
{   
    // TODO: Move this to an external JSON
    public enum Zone { Principal, Frente, Estacionamiento, UsosMultiples, Jardin, Teatro, Antesala, Segundo }
    
    public class ZoneManager : MonoBehaviour
    {
        public event EventHandler<Zone> ZoneChanged;
        public event EventHandler<Boolean> Transition;

        // TODO: Move this to an external JSON
        public static List<ZoneDetails> zones = new List<ZoneDetails> {
            new ZoneDetails(
                "Pasillo Principal", 
                "Con un diseño amplio, moderno y relajante. Nuestra institucion cuenta con amplios espacios que le dan la bienvenida. Admire obras de arte de distinguidos miembros de la comunidad y tenga acceso a informacion de primera mano, nuestro equipo de trabajo estara encantado de atenderle.", 
                "Icons/hotel",
                "Panels/panel-principal"
            ),
            new ZoneDetails(
                "Jardín Frontal", 
                "La Fachada principal de nuestra aclamada institucion, con espacio de zonas verdes, vestido de gala y en constante transformacion. Dando imagen a mas que una Asociacion cultural, una comunidad, que promueve los conocimientos científicos, literarios y artísticos en nuestra region Paraguanera.", 
                "Icons/external",
                "Panels/panel-frente"
            ),
            new ZoneDetails(
                "Estacionamiento", 
                "Contamos con un amplio estacionamiento cercado para la ubicacion de su vehiculo. Permanezca tranquilo durante su estadia gracias al compromiso de nuestro personal de seguridad con la proteccion de sus bienes.", 
                "Icons/parking",
                "Panels/panel-estacionamiento"
            ),
            new ZoneDetails(
                "Sala de usos múltiples", 
                "Con capacidad para mas de 50 personas, esta sala ofrece múltiples posibilidades, ya que puede ser empleado como salón de clases, aula de capacitación, salón de juntas, sala de proyección de material y cualquier tipo de actividad cultural o académica. Además de contar con sistema de aire acondicionado y sonido 5.1 Surround.", 
                "Icons/external",
                "Panels/panel-multiples"
            ),
            new ZoneDetails(
                "Jardín Posterior", 
                "Conozca nuestra zona de Jardín, donde contamos con areas verdes, espaciosas para el disfrute de nuestros visitantes. Adicionalmente, se realizan practicas relacionadas a las actividades academicas impartidas en el recinto. Su mantenimiento y crecimiento se lleva a cabo gracias a la contribucion de entes regionales y colaboradores miembros de nuestra comunidad Ateneista.", 
                "Icons/tree",
                "Panels/panel-jardin"
            ),
            new ZoneDetails(
                "Sala de Teatro", 
                "El Ateneo de Punto Fijo se enorgullece de contar con la sala de teatro mas moderna de la region. Disfrute de asientos comodos, amplia capacidad de invitados, sistemas de aire acondicionado e iluminacion de calidad, camerinos, acustica perfecta, instalaciones para sistema de proyeccion y demas.", 
                "Icons/theatre",
                "Panels/panel-teatro"
            ),
            new ZoneDetails(
                "Antesala a Teatro", 
                "La sala previa a nuestro aclamado salon de teatro. Donde son expuestos los trabajos elaborados por nuestros destacados miembros Ateneistas. Sientase en casa, conozca de productos autoctonos a traves de comerciantes de la region y disfrute de los espacios que tenemos para usted.", 
                "Icons/convention",
                "Panels/panel-antesala"
            ),
            new ZoneDetails(
                "Segundo Piso", 
                "El segundo piso de nuestras instalaciones principalmente se enfocada a cumplir con nuestro compromiso educativo en la region. En estos espacios, distinguidos profesores imparten conocimientos a miembros de todas las edades en areas tales como Pintura, Danza, Musica, Literatura, Oratoria. Participe en actividades culturales, grupos de lectura, ensayos, entre otros.", 
                "Icons/stairs",
                "Panels/panel-segundo"
            )
        };

        public ZoneDetails CurrentZone { get { return m_currentZone; } }
        public Zone InitialZone;

        private Zone m_currentZoneId;
        private ZoneDetails m_currentZone;
        private Animator m_canvasAnim;
        private ZoneToast m_zoneToast;
        private bool m_debouncing;
        private bool m_isInside;
        
        void Awake()
        {
            m_canvasAnim = GetComponent<Animator>();
            m_zoneToast = transform.Find("ZoneToast").GetComponent<ZoneToast>();

            GetComponent<UIManager>().Respawn += OnRespawn;
        }

        void Start()
        {            
            SetCurrentZone(InitialZone, true);
        }

        public void SetCurrentZone(Zone zoneId, bool silent = false)
        {
            if(zoneId == m_currentZoneId) return;
            
            m_isInside = (m_isInside && (zoneId == Zone.Estacionamiento || zoneId == Zone.Frente)) ?
                false : (!m_isInside && zoneId == Zone.Principal);

            Transition?.Invoke(this, m_isInside);
            ZoneChanged?.Invoke(this, zoneId);

            m_currentZone = zones[(int) zoneId];
            m_currentZoneId = zoneId;
            m_zoneToast.SetZone(m_currentZone);

            if(!m_debouncing && !silent)
            {
                m_debouncing = true;
                StartCoroutine(ShowZoneToast());
            }
        }

        private IEnumerator ShowZoneToast()
        {            
            m_canvasAnim.SetTrigger("ZoneToastVisible");
            yield return new WaitForSeconds(5f);
            m_debouncing = false;
        }

        private void OnRespawn(object sender, bool isActive)
        {            
            if(!isActive)
                SetCurrentZone(InitialZone);
        }
    }    
}
