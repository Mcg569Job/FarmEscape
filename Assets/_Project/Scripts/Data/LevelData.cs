using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Create Level Data")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class Level
    {
        [Header("-PERSONS-")]
        [Range(1, 50)] public int PlayerCount;
        [Range(.1f, 5)] public float PlayerSpeed = 2;

        [Range(1, 50)] public int EnemyCount;
        [Range(.1f, 5)] public float EnemySpeed = 2.1f;


        [Header("-GAME-")]
        [Range(1, 50)] public int BulletCountInScene;
    }

    [SerializeField] private Level[] levels;


    public Level GetCurrentlevel()
    {
        int level = LevelManager.Instance.GetLevelIndex() -1;        
        return levels[level];
    }
}
