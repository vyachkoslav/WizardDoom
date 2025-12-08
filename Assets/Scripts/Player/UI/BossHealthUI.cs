using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Player.UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        [SerializeField] private TMPro.TMP_Text nameLabel;

        private Entity boss;

        public void SetBoss(Entity entity)
        {
            Assert.IsNotNull(entity);
            if (entity.Health <= 0) return;
            
            boss = entity;
            nameLabel.text = entity.transform.name;
            gameObject.SetActive(true);
        }

        public void ResetBoss(Entity entity)
        {
            Assert.IsNotNull(entity);
            if (boss != entity) return;
            
            gameObject.SetActive(false);
            boss = null;
            nameLabel.text = "Null";
        }

        private void Update()
        {
            healthBar.value = boss.Health / boss.MaxHealth;
            if (boss.Health <= 0) ResetBoss(boss);
        }
    }
}