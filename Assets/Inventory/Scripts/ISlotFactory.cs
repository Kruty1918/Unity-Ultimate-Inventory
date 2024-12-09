namespace cdvproject.UI
{
    public interface ISlotFactory<T> where T : class
    {
        T CreateSlot();
    }
}