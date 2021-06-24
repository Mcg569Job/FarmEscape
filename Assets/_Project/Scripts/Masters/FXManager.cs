using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FX_Type { Null = 0, Blood = 1, BulletCollision = 2, Grass =3,Coin=4 }

public class FXManager : MonoBehaviour
{
    #region Singleton
    public static FXManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    #endregion
           
    public void ShowFX(FX_Type _type, Vector3 position)
    {
        Transform t = null;
        switch (_type)
        {
            case FX_Type.Blood:
                t = Data.Get.PoolData.GetItem(0).transform;
                t.position = position;
                break;
            case FX_Type.BulletCollision:
                t = Data.Get.PoolData.GetItem(1).transform;
                t.position = position;
                break;
            case FX_Type.Grass:
                t = Data.Get.PoolData.GetItem(2).transform;
                t.position = position;
                t = null;
                break;
            case FX_Type.Coin:
                t = Data.Get.PoolData.GetItem(3).transform;
                t.position = position;               
                break;
        }
        if (t != null)
            StartCoroutine(HideFX(t));
    }

    private IEnumerator HideFX(Transform fx)
    {
        yield return new WaitForSeconds(2.5f);
        fx.gameObject.SetActive(false);
    }
    private IEnumerator SpawnGrass(Vector3 position)
    {
        yield return new WaitForSeconds(.15f);
        GameObject g = Data.Get.PoolData.GetItem(2);
        g.transform.position = this.transform.position;
    }
}
