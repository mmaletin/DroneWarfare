
using System;

public interface ISignal
{
    bool state { get; }
    event Action<bool> onStateChanged;
}