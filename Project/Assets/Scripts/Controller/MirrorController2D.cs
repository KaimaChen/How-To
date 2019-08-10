using UnityEngine;

public class MirrorController2D : MonoBehaviour
{
    private const float kSkin = 0.01f;

    public float mSpeed = 5;
    public Vector2 mSize = new Vector2(0.5f, 0.5f);
    public Transform mMirrorObj;

    private int mTrapLayer;

    Vector2 Pos
    {
        get { return transform.position; }
    }

    Vector2 MirrorPos
    {
        get { return mMirrorObj.transform.position; }
    }

    void Start()
    {
        mTrapLayer = LayerMask.NameToLayer("Trap");
        mMirrorObj.position = new Vector2(-Pos.x, Pos.y);
    }

    void Update()
    {
        if (LevelManager.Instance.IsEnd)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal == 0 && vertical == 0)
            return;

        float x = horizontal * mSpeed * Time.deltaTime;
        float y = vertical * mSpeed * Time.deltaTime;

        //check move origin obj
        float absX = MoveHorizontal(x, Pos);
        float absY = MoveVertical(y, Pos);

        //check move mirror obj
        absX = Mathf.Min(absX, MoveHorizontal(-x, MirrorPos));
        absY = Mathf.Min(absY, MoveVertical(y, MirrorPos));
        
        //both move min distance
        transform.Translate(new Vector2(absX * Mathf.Sign(x), absY * Mathf.Sign(y)), Space.World);
        mMirrorObj.position = new Vector2(-Pos.x, Pos.y);
    }

    private float MoveHorizontal(float moveX, Vector2 pos)
    {
        if (moveX == 0)
            return moveX;

        float sign = Mathf.Sign(moveX);
        float offsetX = sign * mSize.x / 2;
        float offsetY = mSize.y / 2 - kSkin;
        float raycastDist = Mathf.Abs(moveX);
        Vector2 dir = Vector2.right * sign;

        RaycastHit2D topHitInfo = Physics2D.Raycast(pos + new Vector2(offsetX, offsetY), dir, raycastDist);
        RaycastHit2D bottomHitInfo = Physics2D.Raycast(pos + new Vector2(offsetX, -offsetY), dir, raycastDist);

        if (topHitInfo.transform != null)
        {
            moveX = GetCloserValue(moveX, topHitInfo.distance * sign);

            if (IsTouchTrap(topHitInfo)) LevelManager.Instance.SetEnd(); //for simple, touch trap make game over
        }
        if (bottomHitInfo.transform != null)
        {
            moveX = GetCloserValue(moveX, bottomHitInfo.distance * sign);

            if (IsTouchTrap(bottomHitInfo)) LevelManager.Instance.SetEnd(); //for simple, touch trap make game over
        }

        return Mathf.Abs(moveX);
    }

    private float MoveVertical(float moveY, Vector2 pos)
    {
        if (moveY == 0)
            return moveY;

        float sign = Mathf.Sign(moveY);
        float x = mSize.x / 2 - kSkin;
        float y = sign * mSize.y / 2;
        float raycastDist = Mathf.Abs(moveY);
        Vector2 dir = Vector2.up * sign;

        RaycastHit2D leftHitInfo = Physics2D.Raycast(pos + new Vector2(-x, y), dir, raycastDist);
        RaycastHit2D rightHitInfo = Physics2D.Raycast(pos + new Vector2(x, y), dir, raycastDist);

        if (leftHitInfo.transform != null)
        {
            moveY = GetCloserValue(moveY, leftHitInfo.distance * sign);

            if (IsTouchTrap(leftHitInfo)) LevelManager.Instance.SetEnd(); //for simple, touch trap make game over
        }
        if (rightHitInfo.transform != null)
        {
            moveY = GetCloserValue(moveY, rightHitInfo.distance * sign);

            if (IsTouchTrap(rightHitInfo)) LevelManager.Instance.SetEnd(); //for simple, touch trap make game over
        }

        return Mathf.Abs(moveY);
    }

    private float GetCloserValue(float value, float compareValue)
    {
        return (value > 0 ? Mathf.Min(value, compareValue) : Mathf.Max(value, compareValue));
    }

    private bool IsTouchTrap(RaycastHit2D hitInfo)
    {
        Transform trans = hitInfo.transform;
        if (trans != null)
            return trans.gameObject.layer == mTrapLayer;
        else
            return false;
    }
}
