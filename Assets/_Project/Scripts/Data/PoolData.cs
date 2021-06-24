using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPoolData", menuName = "Create Pool Data")]
public class PoolData : ScriptableObject
{
    [System.Serializable]
    public class Pool
    {
        [SerializeField] private string PoolName;

        public GameObject Prefab;
        [Range(0, 30)] public int Count;

        [HideInInspector]
        public int Index;
        [HideInInspector]
        public GameObject[] objects;
    }


    public Pool[] pools;
    private Transform Parent;

    public void SetParent(Transform t) => Parent = t;

    public void CreatePools()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].objects = new GameObject[pools[i].Count];
            pools[i].Index = 0;
            for (int j = 0; j < pools[i].Count; j++)
            {
                GameObject g = Instantiate(pools[i].Prefab, Parent.transform);
                g.SetActive(false);
                pools[i].objects[j] = g;
            }
        }

    }

    public GameObject GetItem(int PoolIndex)
    {
        Pool p = pools[PoolIndex];

        GameObject g = p.objects[p.Index];
        g.SetActive(true);

        p.Index += 1;
        if (p.Index > p.Count - 1)
            p.Index = 0;
        return g;
    }
}
