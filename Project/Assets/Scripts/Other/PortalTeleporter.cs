using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform mPlayer;
    public Transform mExit;

    private bool mIsInTeleporter;

    void Update()
    {
        if (mIsInTeleporter)
        {
            Vector3 portalToPlayer = mPlayer.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer); //这里假设传送门的Y轴朝前

            //>0表示在门前，所以这里表示穿过后才传送
            //传送后会在另一个门的前面，所以不会来回传
            if (dotProduct < 0f)
            {
                MakeTeleport(transform, mExit);

                mIsInTeleporter = false;
            }
        }
    }

    void MakeTeleport(Transform from, Transform to)
    {
        Quaternion relativeDiff = to.rotation * Quaternion.Inverse(from.rotation);
        relativeDiff *= Quaternion.Euler(0, 180, 0);

        Vector3 positionOffset = mPlayer.position - from.position;
        positionOffset = relativeDiff * positionOffset;

        mPlayer.position = to.position + positionOffset;
        mPlayer.rotation *= relativeDiff;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            mIsInTeleporter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            mIsInTeleporter = false;
    }
}
