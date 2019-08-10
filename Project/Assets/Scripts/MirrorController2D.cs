using UnityEngine;

public class MirrorController2D : MonoBehaviour
{
    public float mSpeed = 5;
    public Vector2 mSize = new Vector2(0.5f, 0.5f);
    public Transform mMirrorObj;

    Vector2 Pos
    {
        get { return transform.position; }
    }

    Vector2 MirrorPos
    {
        get { return mMirrorObj.transform.position; }
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal == 0 && vertical == 0)
            return;

        float x = horizontal * mSpeed * Time.deltaTime;
        float y = vertical * mSpeed * Time.deltaTime;

        Vector2 offset1 = new Vector2(x, y);
        Vector2 offset2 = new Vector2(-x, y);
        Vector2 dir1 = offset1.normalized;
        Vector2 dir2 = offset2.normalized;

        float finalX = offset1.x;
        float finalY = offset1.y;

        RaycastHit2D hitInfo1 = Physics2D.BoxCast(Pos + offset1 * 0.1f, mSize, 0, offset1, offset1.magnitude);
        if (hitInfo1.transform != null)
        {
            Vector2 offset = dir1 * hitInfo1.distance;
            finalX = offset1.x > 0 ? Mathf.Min(offset1.x, offset.x) : Mathf.Max(offset1.x, offset.x);
            finalY = offset1.y > 0 ? Mathf.Min(offset1.y, offset.y) : Mathf.Max(offset1.y, offset.y);
        }

        RaycastHit2D hitInfo2 = Physics2D.BoxCast(MirrorPos + offset2 * 0.1f, mSize, 0, offset2, offset2.magnitude);
        if(hitInfo2.transform != null)
        {
            Vector2 offset = dir2 * hitInfo2.distance;
            finalX = offset2.x > 0 ? Mathf.Min(offset2.x, offset.x) : Mathf.Max(offset2.x, offset.x);
            finalY = offset2.y > 0 ? Mathf.Min(offset2.y, offset.y) : Mathf.Max(offset2.y, offset.y);
        }

        transform.Translate(new Vector2(finalX, finalY), Space.World);
        mMirrorObj.position = new Vector2(-Pos.x, Pos.y);
    }
}
