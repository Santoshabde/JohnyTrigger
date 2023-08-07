using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;


namespace SNGames.JonnyTriggerProto
{
    public class InGameUIManager : UIManager
    {
        public static InGameUIManager Instance;

        protected override void Awake()
        {
            Instance = this;
            base.Awake();

            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.ON_REGION_COMPLETED, OnRegionCompleted);
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.ON_NEWREGION_LOADED, OnNewRegionLoaded);
        }

        private void OnRegionCompleted()
        {
            StartCoroutine(OnRegionCompleted_IEnum());

            IEnumerator OnRegionCompleted_IEnum()
            {
                yield return new WaitForSeconds(2f);
                OpenDialog<RegionChangeUIDialog>();
            }       
        }

        private void OnNewRegionLoaded()
        {
            CloseDialog<RegionChangeUIDialog>();
        }

        private void OnDestroy()
        {
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.ON_REGION_COMPLETED, OnRegionCompleted);
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.ON_NEWREGION_LOADED, OnNewRegionLoaded);
        }
    }
}