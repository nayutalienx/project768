namespace project768.scripts.common.interaction;

public abstract class Interaction<T>
{
    public T Entity { get; set; }

    public Interaction(T entity)
    {
        Entity = entity;
    }

    public virtual void Interact()
    {
    }
}