using UnityEngine;
using PathCreation;


public abstract class PersonBase : MonoBehaviour
{
    protected PathCreator pathCreator=null;
    private Vector3 _startPos;
    protected float distanceTravelled;
    protected bool isMove;
    
    protected PersonAnimations _PersonAnimations;
    [SerializeField] protected Animator animator;

    [SerializeField] [Range(.1f, 10)] protected float speed = 2;
    public void SetSpeed(float value) => speed = value;

    public void SetStartPos(Vector3 pos)
    {
        this._startPos = pos;
        transform.position = pathCreator.path.GetPointAtDistance(0, EndOfPathInstruction.Stop) + _startPos;
    }
    public void SetPath(PathCreator creator) => this.pathCreator = creator;

    public void Move(bool move = true)
    {

        if (!isMove)
        {           
            return;
        }

        distanceTravelled += Time.deltaTime * speed;

        if (!move || pathCreator==null) return;

        Vector3 point = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
        transform.position = Vector3.Lerp(transform.position, point + _startPos, Time.deltaTime * .5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop), Time.deltaTime*10);

        if (pathCreator.path.isFinish) isMove = false;
    }

    public Vector3 GetCurrentPoint()
    {
        return pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
            isMove = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle")
            isMove = true;
    }

    #region Animations
    public void SetAnimation(PersonAnimations PersonAnimations)
    {
        if (animator == null) return;

        if (PersonAnimations == _PersonAnimations) return;
        _PersonAnimations = PersonAnimations;

        animator.SetBool("idle", false);
        animator.SetBool("walk", false);
        animator.SetBool("attack", false);
        animator.SetBool("dead", false);

        switch (PersonAnimations)
        {
            case PersonAnimations.Walk:
                animator.SetBool("walk", true);
                break;
            case PersonAnimations.Idle:
                animator.SetBool("idle", true);
                break;
            case PersonAnimations.Attack:
                animator.SetBool("attack", true);
                break;
            case PersonAnimations.Dead:
                animator.SetBool("dead", true);
                break;
        }
    }

    public enum PersonAnimations
    {
        Null,
        Idle,
        Walk,
        Attack,
        Dead
    }
    #endregion

}
