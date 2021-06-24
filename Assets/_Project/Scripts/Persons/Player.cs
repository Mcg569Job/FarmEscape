
using UnityEngine;

public class Player : PersonBase
{


    [HideInInspector] public bool isTarget = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        speed = Data.Get.LevelData.GetCurrentlevel().PlayerSpeed;
        speed *= Random.Range(.95f, 1.05f);
        isMove = true;
        SetAnimation(PersonAnimations.Walk);
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
            isMove = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle")
        {          
            isMove = true;
        }
    }
    private void Update()
    {

        if (isMove)
            SetAnimation(PersonAnimations.Walk);
        else
            SetAnimation(PersonAnimations.Idle);

        if (GameManager.Instance.GameStarted())
        {
            Move();
        }

    }

    public void Dead()
    {
        isTarget = false;
        isMove = false;
        GameManager.Instance.RemovePerson(this);
        gameObject.SetActive(false);
    }

}
