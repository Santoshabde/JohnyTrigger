using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using UnityEngine.UI;
using DG.Tweening;

namespace SNGames.JonnyTriggerProto
{
    public class RegionChangeUIDialog : BaseUIDialog
    {
        [SerializeField] private Image regioChangeImage;

        public override void OnOpenDialog()
        {
            base.OnOpenDialog();
            regioChangeImage.DOFillAmount(1f, 1f);
        }

        public override void OnCloseDialog()
        {
            regioChangeImage.DOFillAmount(0f, 1f).
                OnComplete(() => base.OnCloseDialog());
        }
    }
}
