using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStatus { Null, Normal, GameOver, FinishGame, Win }

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        GameStatus = GameStatus.Null;
    }

    private int _earnedCoinInLevel;

    private List<Enemy> enemys = new List<Enemy>();
    private List<Player> players = new List<Player>();

    public void AddPerson(PersonBase person)
    {
        if (person.GetType() == typeof(Player))
            AddPlayer((Player)person);
        else
            AddEnemy((Enemy)person);
    }
    public void RemovePerson(PersonBase person)
    {
        if (person.GetType() == typeof(Player))
            RemovePlayer((Player)person);
        else
            RemoveEnemy((Enemy)person);
    }

    private void AddPlayer(Player player)
    {
        if (players.Contains(player)) return;
        players.Add(player);
        CheckPlayers();
    }
    private void RemovePlayer(Player player)
    {
        players.Remove(player);
        FXManager.Instance.ShowFX(FX_Type.Blood, player.transform.position);
        AudioManager.Instance.PlaySound(AudioType.PlayerDead);
        CheckPlayers();
    }
    private void AddEnemy(Enemy enemy)
    {
        if (enemys.Contains(enemy)) return;
        enemys.Add(enemy);
        CheckEnemys();
    }
    private void RemoveEnemy(Enemy enemy)
    {
        enemys.Remove(enemy);
        AudioManager.Instance.PlaySound(AudioType.EnemyDead);
        CheckEnemys();
    }
    private void CheckPlayers()
    {
        if (players.Count == 0)
        {
            GameOver();
        }
    }
    private void CheckEnemys()
    {
        if (enemys.Count == 0)
        {
          
        }
    }

    public GameStatus GameStatus;

    public bool GameStarted()
    {
        if (GameStatus == GameStatus.Normal ||
            GameStatus == GameStatus.FinishGame) return true;
        else return false;
    }

    public void Play()
    {
        if (GameStatus == GameStatus.Normal) return;
        GameStatus = GameStatus.Normal;
        UIManager.Instance.ActivateMenuPanel(false);
        UIManager.Instance.ActivateGamePanel(true);
        TinySauce.OnGameStarted(levelNumber:LevelManager.Instance.currentLevel.ToString());
    }
    public void FinishGame()
    {
        if (GameStatus == GameStatus.GameOver || GameStatus == GameStatus.FinishGame) return;
        GameStatus = GameStatus.FinishGame;
       
    }
    public void Win()
    {
        if (this.GameStatus == GameStatus.Win) return;

        if (this.GameStatus != GameStatus.GameOver)
        {
            if (enemys.Count > 0) return;

            AddCoin(players.Count * 10);

            UIManager.Instance.UpdateWinTexts(_earnedCoinInLevel, players.Count);
            UIManager.Instance.ActivateWinPanel(true);
            AudioManager.Instance.PlaySound(AudioType.Win);

            Data.Get.Coin += _earnedCoinInLevel;

            TinySauce.OnGameFinished(true, _earnedCoinInLevel, levelNumber: LevelManager.Instance.currentLevel.ToString());
            this.GameStatus = GameStatus.Win;
            _earnedCoinInLevel = 0;
        }
        print("win");
    }

    private void GameOver()
    {
        this.GameStatus = GameStatus.GameOver;
        
        UIManager.Instance.ActivateGameOverPanel(true);
        AudioManager.Instance.PlaySound(AudioType.GameOver);
        TinySauce.OnGameFinished(false, 0, levelNumber: LevelManager.Instance.currentLevel.ToString());
        _earnedCoinInLevel = 0;
    }


    public void AddCoin(int amount)
    {
        _earnedCoinInLevel += amount;
    }

}
