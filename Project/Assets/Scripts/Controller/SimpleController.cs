using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleController : MonoBehaviour
{
    public float m_speed = 10;
    
    [SerializeField] private Camera m_camera;
    private readonly MouseLook m_mouseLook = new MouseLook();
    private CharacterController m_controller;

    void Start()
    {
        m_mouseLook.Init(transform, m_camera.transform);
        m_controller = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
        RotateView();

        //x-z move
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if(horizontal != 0 || vertical != 0)
        {
            float x = horizontal * m_speed * Time.deltaTime;
            float z = vertical * m_speed * Time.deltaTime;
            Vector3 motion = transform.right * x + transform.forward * z;

            m_controller.Move(motion);
        }
    }

    private void RotateView()
    {
        m_mouseLook.LookRotation(transform, m_camera.transform);
    }
}
