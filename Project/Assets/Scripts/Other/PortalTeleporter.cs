using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    private static PortalTeleporter sMovedTeleporter;

    public Transform mPlayer;
    public Transform mExit;

    private bool mIsInTeleporter;

    void Update()
    {
        if (mIsInTeleporter)
        {
            Vector3 portalToPlayer = mPlayer.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            if (dotProduct < 0f) //穿过后才传送
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
        //if (other.tag != "Player" || this == sMovedTeleporter)
        //{
        //    Debug.Log(other.tag + ", " + this.GetInstanceID() + ", " + (sMovedTeleporter == null ? "null" : sMovedTeleporter.GetInstanceID().ToString()));
        //    return;
        //}

        //Debug.Log("Teleport" + mPlayer.position + ", " + mTargetTeleporter.transform.position);
        //mPlayer.position = mTargetTeleporter.transform.position;

        if (other.tag == "Player")
            mIsInTeleporter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            mIsInTeleporter = false;
    }
}
