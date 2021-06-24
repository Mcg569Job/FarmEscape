using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheObject : MonoBehaviour
{
    [SerializeField] private string TargetTag;
    [SerializeField] private Vector3 FollowDistance;
    private Transform Target;

    private void Start()
    {
        StartCoroutine(Find());
    }

    private IEnumerator Find() {
        yield return new WaitForSeconds(2);
        Target = GameObject.FindGameObjectWithTag(TargetTag).transform;
    }

    void Update()
    {
        if(Target!=null)
        transform.position = Vector3.Lerp(transform.position,Target.position + FollowDistance,Time.deltaTime * 3);
    }
}
