using SNGames.CommonModule;
using SNGames.JonnyTriggerProto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class WeaponsManager : MonoBehaviour, IObserverOfSubject<InputData>
    {
        [SerializeField] private string initialGunToSpawn;
        [SerializeField] private LineRenderer aimLineRenderer;
        [SerializeField] private List<GunSpawnPoints> gunSpawnPoints;

        private BaseGun currentGun_LH;
        private BaseGun currentGun_RH;

        private bool isShootFunctionalityEnabled;
        private Vector3 gunAimStartPoint;
        private Vector3 gunAimTargetPoint;
        private ZoneManager currentZoneCharacterIsIn;

        private void Start()
        {
            CharacterState_BattleMode_WithJumpCurve.OnDecideCharacterShootFunctionality += OnDecideCharacterShootFunctionality;
            CharacterState_BattleMode_WithoutJumpCurve.OnDecideCharacterShootFunctionality += OnDecideCharacterShootFunctionality;
            InputController.AddOberver(this);

            SpawnGun(initialGunToSpawn);
        }


        private void OnDecideCharacterShootFunctionality(bool shoudEnable, Vector3 startingPosition,ZoneManager currentZone)
        {
            isShootFunctionalityEnabled = shoudEnable;
            gunAimStartPoint = startingPosition;
            gunAimTargetPoint = currentZone.GetCurrentAimAtPointOnAimAtCurve();
            currentZoneCharacterIsIn = currentZone;

            if (shoudEnable)
            {
                if (!aimLineRenderer.enabled)
                    aimLineRenderer.enabled = true;

                aimLineRenderer.SetPosition(0, startingPosition);
                aimLineRenderer.SetPosition(1, currentZone.GetCurrentAimAtPointOnAimAtCurve());
            }
            else
            {
                if (aimLineRenderer.enabled)
                    aimLineRenderer.enabled = false;
            }
        }

        private void LateUpdate()
        {
            RotateGunTowardsTheAimTargetCurvePoint();
        }

        private void SpawnGun(string gunID)
        {
            Transform gunToSpawnTransform_LH = gunSpawnPoints.Find(t => t.gunID == gunID).gunSpawnPoint_LH;
            Transform gunToSpawnTransform_RH = gunSpawnPoints.Find(t => t.gunID == gunID).gunSpawnPoint_RH;
            BaseGun gunToSpawn = GunDataConfig.GunData[gunID].gunPrefab;

            currentGun_LH = Instantiate(gunToSpawn);
            currentGun_LH.transform.parent = gunToSpawnTransform_LH;
            currentGun_LH.transform.localPosition = Vector3.zero;
            currentGun_LH.transform.localScale = Vector3.one;
            currentGun_LH.transform.localRotation = Quaternion.identity;

            currentGun_RH = Instantiate(gunToSpawn);
            currentGun_RH.transform.parent = gunToSpawnTransform_RH;
            currentGun_RH.transform.localPosition = Vector3.zero;
            currentGun_RH.transform.localScale = Vector3.one;
            currentGun_RH.transform.localRotation = Quaternion.identity;
        }

        private void RotateGunTowardsTheAimTargetCurvePoint()
        {
            if (isShootFunctionalityEnabled)
            {
                Vector3 gunDirectionToFace = (gunAimTargetPoint - gunAimStartPoint).normalized;

                if (currentZoneCharacterIsIn.GetCurrentZoneData().enableRightHandIKToAimCurveTarget)
                    currentGun_RH.transform.rotation = Quaternion.LookRotation(gunDirectionToFace, Vector3.forward);

                if (currentZoneCharacterIsIn.GetCurrentZoneData().enableLeftHandIKToAimCurveTarget)
                    currentGun_LH.transform.rotation = Quaternion.LookRotation(gunDirectionToFace, Vector3.forward);
            }
        }

        private void OnDestroy()
        {
            CharacterState_BattleMode_WithJumpCurve.OnDecideCharacterShootFunctionality -= OnDecideCharacterShootFunctionality;
            CharacterState_BattleMode_WithoutJumpCurve.OnDecideCharacterShootFunctionality -= OnDecideCharacterShootFunctionality;
        }

        public void OnNotify(InputData data)
        {
            ZoneDataOutput zoneData = currentZoneCharacterIsIn.GetCurrentZoneData();

            if (isShootFunctionalityEnabled
                && zoneData.currentBulletCountInZone > 0)
            {
                Vector3 gunDirectionToFace = (gunAimTargetPoint - gunAimStartPoint).normalized;
                gunDirectionToFace.x = 0;

                currentZoneCharacterIsIn.DecreaseZoneCurrentBulletCount();

                if (zoneData.shootFromLeftHand)
                    currentGun_LH.OnShoot(gunDirectionToFace);

                if (zoneData.shootFromRightHand)
                    currentGun_RH.OnShoot(gunDirectionToFace);
            }
        }
    }

    [System.Serializable]
    public struct GunSpawnPoints
    {
        public string gunID;
        public Transform gunSpawnPoint_LH;
        public Transform gunSpawnPoint_RH;
    }
}