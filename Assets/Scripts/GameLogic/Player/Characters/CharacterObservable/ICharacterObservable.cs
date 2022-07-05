using System.Collections.Generic;

public interface ICharacterObservable
{
    public void AddObserver(IObserveCharacters newObserver);
    public void RemoveObserver(IObserveCharacters observer);
}