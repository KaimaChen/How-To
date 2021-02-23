using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTunnel : MonoBehaviour
{
    [SerializeField] Vector3 mApex;
    [SerializeField] float mFov;
    [SerializeField] float mNear;
    [SerializeField] float mFar;
    [SerializeField] float mAspect;
    
    void Start()
    {
        
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
            Scale(player.transform);
    }

    private void Scale(Transform target)
    {
        //Vector3 smallToBig = mBigPos - mSmallPos;
        //float length = smallToBig.magnitude;

        //Vector3 smallToPos = target.position - mSmallPos;
        //float project = Vector3.Dot(smallToPos, smallToBig);
        //if (project < 0 || project > length)
        //    return;

        //float ratio = project / length;
        //float smallScale = mSmallRect.height / mBigRect.height;
        //float scale = Mathf.Lerp(smallScale, 1, ratio);
        //target.localScale = new Vector3(scale, scale, scale);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawFrustum(mApex, mFov, mFar, mNear, mAspect);
    }
}
