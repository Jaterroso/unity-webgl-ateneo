using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ateneo
{
    public class ZonePortal : MonoBehaviour
    {
        [Serializable]
        public struct Detector {
            public Vector3 position;
            public Zone zone;
        }

        public List<Detector> detectors;
        private List<Collider> m_colliders;
        
        void Awake()
        {
            if(detectors.Count > 0)
            {
                m_colliders = new List<Collider>();
                
                foreach (var item in detectors)
                {
                    BoxCollider coll = gameObject.AddComponent<BoxCollider>();
                    coll.center = item.position;
                    coll.isTrigger = true;
                    m_colliders.Add(coll);
                }
            }
        }

        public Zone GetZoneByColl(Collider coll)
        {
            Detector det = detectors[m_colliders.IndexOf(coll)];
            return det.zone;
        }
    }    
}