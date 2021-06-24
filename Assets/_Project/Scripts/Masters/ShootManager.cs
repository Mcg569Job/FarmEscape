using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    private int _bulletCount;
    private bool _isShooting = false;

    [Header("-BULLET POOL-")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] [Range(1, 30)] private int _bulletCountInPool = 10;
    private int _bulletId = 0;
    private Bullet[] _bulletsInPool;

    void Start()
    {
        _bulletCount = Data.Get.LevelData.GetCurrentlevel().BulletCountInScene;
        UIManager.Instance.SetBullets(_bulletCount);

        CreateBulletPool();
    }

    private void CreateBulletPool()
    {
        _bulletPrefab = Data.Get.BulletData.GetCurrentBulletPrefab();
        _bulletsInPool = new Bullet[_bulletCountInPool];
        for (int i = 0; i < _bulletCountInPool; i++)
        {
            GameObject g = Instantiate(_bulletPrefab, this.transform);
            g.transform.rotation = Random.rotation;
            _bulletsInPool[i] = g.GetComponent<Bullet>();
        }
    }

    void Update()
    {
        if (!GameManager.Instance.GameStarted()) return;

        if (Input.GetMouseButtonDown(0))
        {
            ClickToShoot();
        }
    }

    public void ClickToShoot()
    {
        if (_bulletCount > 0)
        {
            if (_isShooting) return;
            _isShooting = true;
            _bulletCount--;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, _layer))
            {
                Shoot(hit.point);
            }

            AudioManager.Instance.PlaySound(AudioType.Throw);
            AudioManager.Instance.Vibrate();

            UIManager.Instance.UpdateBullets(_bulletCount);
            StartCoroutine(ReadyToShoot());
        }

        
        if (_bulletCount <= 0)
        {
            if (GameManager.Instance.GameStatus == GameStatus.FinishGame)
                GameManager.Instance.Win();
        }

    }
    private void Shoot(Vector3 target)
    {
        Bullet currentBullet = _bulletsInPool[_bulletId];
        currentBullet.transform.position = Camera.main.transform.position;
        currentBullet.StartMove(target);
        _bulletId++;
        if (_bulletId > _bulletCountInPool - 1) _bulletId = 0;
    }

    private IEnumerator ReadyToShoot()
    {
        yield return new WaitForSeconds(.1f);
        _isShooting = false;
    }

}
