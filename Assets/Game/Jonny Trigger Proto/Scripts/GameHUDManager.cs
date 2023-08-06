using SNGames.CommonModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SNGames.JonnyTriggerProto
{
    public class GameHUDManager : MonoBehaviour
    {
        [SerializeField] private GameObject bulletsHolder;
        [SerializeField] private GameObject bulletIconPrefab;

        private List<GameObject> bulletImagesSpawnedInZone;
        private ZoneManager currentZoneCharacterIn;

        private void Awake()
        {
            CharacterPhysicsController.OnEnterZone += OnEnterZone;
            CharacterPhysicsController.OnExitZone += OnExitZone;
            WeaponsManager.OnBulletShot += OnBulletShot;
        }

        private void OnExitZone(ZoneManager zone)
        {
            currentZoneCharacterIn = null;
            bulletsHolder.SetActive(false);

            if (bulletImagesSpawnedInZone != null)
            {
                foreach (var item in bulletImagesSpawnedInZone)
                {
                    Destroy(item.gameObject);
                }
            }

            bulletImagesSpawnedInZone.Clear();
            bulletImagesSpawnedInZone = null;
        }

        private void OnEnterZone(ZoneManager zone)
        {
            currentZoneCharacterIn = zone;
            bulletImagesSpawnedInZone = new List<GameObject>();

            for (int i = 0; i < zone.GetCurrentZoneData().bulletCount; i++)
            {
                GameObject icon = Instantiate(bulletIconPrefab);
                icon.transform.SetParent(bulletsHolder.transform);
                bulletImagesSpawnedInZone.Add(icon);
            }

            bulletsHolder.SetActive(true);
        }

        private void OnBulletShot()
        {
            bulletImagesSpawnedInZone[bulletImagesSpawnedInZone.Count - 1].GetComponent<Image>().DOFade(0.08f, 0.1f);
            bulletImagesSpawnedInZone.RemoveAt(bulletImagesSpawnedInZone.Count - 1);
        }
    }
}