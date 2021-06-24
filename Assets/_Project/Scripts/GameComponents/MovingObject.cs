using UnityEngine;

public class MovingObject : MonoBehaviour
{
    #region variables
  
    [Range(0, 5)] public float speedY = 1.25f;
    [Range(0, 5)] public float motionRangeY = .5f;

    private Vector2 _start;
    private Vector2 _pos;
    private Vector3 _vec;
    private int  _dirY = 1;

    #endregion

    #region Start
    private void Start()=>  _start = transform.position;    
    #endregion

    #region Move
    public void Update() => UpdatePosition();
    private void UpdatePosition()
    {
        _pos = transform.position;
       
        if (_pos.y > _start.y + motionRangeY) _dirY = -1;
        else if (_pos.y <= _start.y - motionRangeY) _dirY = 1;
        
        _vec.y = speedY * _dirY * Time.deltaTime;

        transform.position += _vec;
    }
    #endregion
}
