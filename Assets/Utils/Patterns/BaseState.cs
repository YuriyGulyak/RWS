public abstract class BaseState<T>
{
    public T owner;

    public virtual void OnEnableState() { }

    public virtual void OnDisableState() { }
    
    public virtual void OnUpdateState() { }
}
