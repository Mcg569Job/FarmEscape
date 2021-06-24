using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType {Coin,Throw,PlayerDead,EnemyDead,Win,GameOver,Click,Purchase }

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance = null;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    [SerializeField] private AudioSource source;

    [SerializeField] private AudioClip Coin;
    [SerializeField] private AudioClip Throw;
    [SerializeField] private AudioClip PlayerDead;
    [SerializeField] private AudioClip EnemyDead;
    [SerializeField] private AudioClip Win;
    [SerializeField] private AudioClip GameOver;
    [SerializeField] private AudioClip Click;
    [SerializeField] private AudioClip Purchase;


    public void PlaySound(AudioType audioType)
    {
        if (PlayerPrefs.GetInt("sound") != 0) return;

        AudioClip clip = null;
        switch (audioType)
        {
            case AudioType.Coin: clip = Coin; break;           
            case AudioType.Throw: clip = Throw; break;
            case AudioType.PlayerDead: clip = PlayerDead; break;           
            case AudioType.EnemyDead: clip = EnemyDead; break;           
            case AudioType.Win: clip = Win; break;           
            case AudioType.GameOver: clip = GameOver; break;           
            case AudioType.Click: clip = Click; break;
            case AudioType.Purchase: clip = Purchase; break;
        }

        if (clip != null)
            source.PlayOneShot(clip);
    }

    public void Vibrate()
    {
        if (PlayerPrefs.GetInt("vibrate") == 0)
            Handheld.Vibrate();
    }
}
