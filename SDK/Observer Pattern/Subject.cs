using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public abstract class Subject<T> : MonoBehaviour
    {
        private static List<IObserver<T>> observers = new List<IObserver<T>>();

        public static void AddOberver(IObserver<T> observer)
        {
            observers.Add(observer);
        }

        public static void RemoveObserver(IObserver<T> observer)
        {
            observers.Remove(observer);
        }

        protected void NotifyAllObservers(T data)
        {
            observers.ForEach(observer => observer.OnNotify(data));
        }
    }
}
