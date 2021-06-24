using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shop : MonoBehaviour
{

    #region Singleton
    public static Shop instance = null;
    public void Awake()
    {
        instance = this;
    }
    #endregion


    private void Start()
    {
      
    }




}
