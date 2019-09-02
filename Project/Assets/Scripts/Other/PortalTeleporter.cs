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

            // If this is true: The player has moved across the portal
            if (dotProduct < 0f)
            {
                // Teleport him!
                float rotationDiff = -Quaternion.Angle(transform.rotation, mExit.rotation);
                rotationDiff += 180;
                mPlayer.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                mPlayer.position = mExit.position + positionOffset;

                mIsInTeleporter = false;
            }
        }
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
