using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public interface IObserver<T>
    {
        public void OnNotify(T data);
    }
}