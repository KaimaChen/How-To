using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleController : MonoBehaviour
{
    public float mSpeed = 10;

    [SerializeField] private Camera mCamera;
    private readonly MouseLook mMouseLook = new MouseLook();

    void Start()
    {
        mMouseLook.Init(transform, mCamera.transform);
    }

    void Update()
    {
        RotateView();

        //x-z move
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if(horizontal != 0 || vertical != 0)
        {
            float x = horizontal * mSpeed * Time.deltaTime;
            float z = vertical * mSpeed * Time.deltaTime;
            Vector3 motion = transform.right * x + transform.forward * z;

            transform.Translate(motion, Space.World);
        }
    }

    private void RotateView()
    {
        mMouseLook.LookRotation(transform, mCamera.transform);
    }
}
