using System.Collections.Generic;
using UnityEngine;

namespace PetWorld
{
    public class ListenerRegistry : MonoBehaviour
    {
        private readonly HashSet<ListenerSubscriber> _subscribers = new HashSet<ListenerSubscriber>();

        private void OnEnable()
        {
            foreach (var subscriber in _subscribers)
                subscriber.AddListeners();
        }

        private void OnDestroy()
        {
            foreach (var subscriber in _subscribers)
                subscriber.RemoveListeners();

            _subscribers.Clear();
        }
        
        public void Register(ListenerSubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }
    }   
}