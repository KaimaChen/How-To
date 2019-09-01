using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform mMainCamera;
    public Transform mMyPortal;
    public Transform mOtherPortal;
    
    void Update()
    {
        //update position
        Vector3 relativePos = mMainCamera.position - mOtherPortal.position;
        transform.position = mMyPortal.position + relativePos;

        //update rotation
        Vector3 relativeRot = mMainCamera.rotation.eulerAngles - mOtherPortal.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(mMyPortal.rotation.eulerAngles + relativeRot);
    }
}
