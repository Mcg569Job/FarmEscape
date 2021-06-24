using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image Image;
    private BulletData.Bullet _bullet;

    private Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    public void Set(BulletData.Bullet bullet)
    {
        if (btn == null)
            btn = GetComponent<Button>();            
        _bullet = bullet;

        if(_bullet.IsPurchased)
        Image.sprite = _bullet.Icon;

        btn.onClick.AddListener(SelectOrPurchaseMe);
    }

    private void SelectOrPurchaseMe()
    {
        if (_bullet.IsPurchased)
        {
            UIManager.Instance.SelectBullet(_bullet.Id); 
        }
        else
        {
            //purchase
        }
    }
}
