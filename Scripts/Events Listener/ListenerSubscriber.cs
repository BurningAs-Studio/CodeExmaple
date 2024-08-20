using Zenject;

namespace PetWorld
{
    public abstract class ListenerSubscriber
    {
        [Inject]
        public virtual void Construct(DiContainer diContainer)
        {
            Registration(diContainer.Resolve<ListenerRegistry>());
        }
        
        private void Registration(ListenerRegistry listenerRegistry)
        {
            listenerRegistry.Register(this);
        }
        
        public virtual void AddListeners(){}
        public virtual void RemoveListeners(){}
    }
}