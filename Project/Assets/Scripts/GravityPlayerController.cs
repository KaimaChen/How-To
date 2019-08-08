using UnityEngine;

public class GravityPlayerController : MonoBehaviour
{
    public Transform mGround;

    public float mSpeed = 5f;
    public float mGravity = -9.8f;
    public float mJumpSpeed = 7f;
    public float mGroundedDistance = 0.6f;

    private float mSpeedY;

    void Update()
    {
        //x-z move
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float x = horizontal * mSpeed * Time.deltaTime;
        float z = vertical * mSpeed * Time.deltaTime;

        transform.Translate(new Vector3(x, 0, z), Space.Self);

        //y move
        Vector3 toGround = mGround.position - transform.position;

        if (Physics.Raycast(transform.position, toGround, out RaycastHit hitInfo, LayerMask.GetMask("Ground")))
        {
            float distance = (hitInfo.point - transform.position).magnitude;
            if(distance > mGroundedDistance) // in air
            {
                mSpeedY += mGravity * Time.deltaTime;
            }
            else // grounded
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    mSpeedY = mJumpSpeed;
                else
                    mSpeedY = 0;
            }

            Vector3 moveY = hitInfo.normal * mSpeedY * Time.deltaTime;
            transform.Translate(moveY, Space.World);
        }

        //rotate
        Vector3 forward = Vector3.Cross(toGround, transform.right);
        transform.rotation = Quaternion.LookRotation(forward, -toGround);
    }
}
