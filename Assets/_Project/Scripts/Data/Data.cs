using UnityEngine;
public class Data : MonoBehaviour
{

    [ContextMenu("ResetPlayerPrefsData")]
    void ResetPlayerPrefsData()
    {
        PlayerPrefs.DeleteAll();
    }



    public static Data Get = null;
    private void Awake()
    {
        if (Get == null) Get = this;
    }

    public BulletData BulletData;
    public LevelData LevelData;
    public PoolData PoolData;

    public int Coin
    {
        get
        {
            return PlayerPrefs.GetInt("coin");
        }
        set
        {
            PlayerPrefs.SetInt("coin", value);
           UIManager.Instance.UpdateCoin();
        }
    }

    private void Start()
    {
        BulletData.SelectCurrentBullet();
        PoolData.SetParent(this.transform);
        PoolData.CreatePools();
    }

}
