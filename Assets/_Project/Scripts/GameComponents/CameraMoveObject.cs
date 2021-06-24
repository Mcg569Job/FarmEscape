using PathCreation;
using System.Collections;
using UnityEngine;

public class CameraMoveObject : PersonBase
{
    private Transform _finishObject;

    private void Start()
    {
        _finishObject = GameObject.FindGameObjectWithTag("Finish").transform;
        SetPath(GameObject.FindGameObjectWithTag("MainPath").GetComponent<PathCreator>());
       
        StartCoroutine(StartMove());
       
        UIManager.Instance.SetDistanceBarValue(pathCreator.path.length);
        speed= Data.Get.LevelData.GetCurrentlevel().PlayerSpeed; 
    }

    private IEnumerator StartMove()
    {
        yield return new WaitForSeconds(0.1f);
        isMove = true;
    }

    private void Update()
    {

        if (pathCreator.path.isFinish && GameManager.Instance.GameStarted()) 
        {
            GameManager.Instance.FinishGame();
            transform.position = Vector3.Lerp(transform.position,_finishObject.position,Time.deltaTime*speed);
            return;
        }

        if (!GameManager.Instance.GameStarted()) return;

        if (isMove)
        {
            UIManager.Instance.UpdateDistanceBar(distanceTravelled);
            Move();
        }
    }
}
