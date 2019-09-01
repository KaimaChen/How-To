using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform mPlayer;
    public Transform mTargetPortal;

    private bool mIsInTeleporter;

    void Update()
    {
        if (mIsInTeleporter)
        {
            Vector3 portalToPlayer = mPlayer.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // If this is true: The player has moved across the portal
            if (dotProduct < 0f)
            {
                // Teleport him!
                float rotationDiff = -Quaternion.Angle(transform.rotation, mTargetPortal.rotation);
                rotationDiff += 180;
                mPlayer.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                mPlayer.position = mTargetPortal.position + positionOffset;

                mIsInTeleporter = false;
            }
        }
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
