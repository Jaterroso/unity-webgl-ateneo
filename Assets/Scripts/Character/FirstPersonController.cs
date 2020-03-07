using UnityEngine;

namespace Ateneo
{
    //
    // First Person Controller
    // Source: https://github.com/jiankaiwang/FirstPersonController
    //
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerEvents))]
    [RequireComponent(typeof(PlayerSound))]
    public class FirstPersonController : MonoBehaviour
    {        
        public float speed = 10.0f;
        public float gravity = 0f;
        public Vector3 RespawnCoords;
        public Vector2 initialMouseLook;

        private CharacterController m_controller;
        private PlayerSound m_sound;
        
        private Camera m_camera;
        public Camera CurrentCamera { get { return m_camera; }}

        [SerializeField]
        public float sensitivity = 5.0f;
        [SerializeField]
        public float smoothing = 2.0f;   

        private float straffe;
        // get the incremental value of mouse moving
        private Vector2 mouseLook;
        // smooth the mouse moving
        private Vector2 smoothV;

        void Awake()
        {
            m_controller = GetComponent<CharacterController>();
            m_sound = GetComponent<PlayerSound>();
            m_camera = (Camera)FindObjectOfType(typeof(Camera));
            mouseLook = initialMouseLook;
        }
        
        void Update()
        {            
            float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            float straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;            
            Vector3 move = transform.TransformDirection(straffe, 0, translation);
            
            if(!m_controller.isGrounded) {
                move.y -= gravity * Time.deltaTime;
            }
            
            // Enable walk sound only when player is moving
            m_sound.SetActive(translation != 0);

            m_controller.Move(move);
            this.updateMouseCamera();
        }

        void OnDisable()
        {
            m_sound.SetActive(false);
        }

        private void updateMouseCamera() {
            var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
            mouseLook += smoothV;
            m_camera.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(mouseLook.x, transform.up);
        }
    }    
}