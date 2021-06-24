
using UnityEngine;
using PathCreation;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 followDistance;
    private Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("CameraMoveObject").transform;
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.GameStatus == GameStatus.FinishGame)
        {
            if (followDistance.y < 8)
            {
                followDistance.y += Time.deltaTime * 2;
            }           
        }
               
        transform.position = target.position + followDistance;
        transform.LookAt(target);
        
    }

}
