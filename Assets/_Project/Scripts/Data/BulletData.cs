using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletData", menuName = "Create Bullet Data")]
public class BulletData : ScriptableObject
{
    [System.Serializable]
    public struct Bullet
    {
        public string Name;
        public int Id;
        public GameObject Prefab;
        public Gradient Gradient;
        public Color Color;
        public Sprite Icon;
        public bool IsPurchased { get { return PlayerPrefs.HasKey("bullet_" + Id) ? PlayerPrefs.GetInt("bullet_" + Id) == 1 : false; } set { PlayerPrefs.SetInt("bullet_" + Id, value ? 1 : 0); } }
    }
    [SerializeField] private ParticleSystem Particle;
    [SerializeField] public Bullet[] bullets;

    public int SelectedBullet
    {
        get
        {
            return PlayerPrefs.HasKey("bullet") ? PlayerPrefs.GetInt("bullet") : 0;
        }
        set
        {
            PlayerPrefs.SetInt("bullet", value);
        }
    }

    public void SelectCurrentBullet()
    {
        UIManager.Instance.UpdateBullet(SelectedBullet);
        UpdateCurrentParticle();
    }

    public Bullet GetCurrentBullet()
    {
        return bullets[SelectedBullet];
    }

    public Sprite GetCurrentBulletIcon()
    {
        return bullets[SelectedBullet].Icon;
    }

    public GameObject GetCurrentBulletPrefab()
    {
        return bullets[SelectedBullet].Prefab;
    }

    public void UpdateCurrentParticle()
    {
        Gradient gr = bullets[SelectedBullet].Gradient;

        foreach (ParticleSystem particle in Particle.GetComponentsInChildren<ParticleSystem>())
        {
            var col = particle.colorOverLifetime;
            col.enabled = true;
            col.color = gr;
        }

    }
    public bool UnlockedAll()
    {
        bool ok = true;
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].IsPurchased)
            {
                ok = false;
            }
        }
        return ok;

    }
    public IEnumerator RandomUnlockBullet()
    {
        while (true)
        {
            int i = Random.Range(0, bullets.Length);
            if (bullets[i].IsPurchased == false)
            {
                bullets[i].IsPurchased = true;
                UIManager.Instance.SelectBullet(bullets[i].Id);
                UIManager.Instance.UpdateShop();
                break;
            }
            yield return null;
        }

    }
}
