using System;
using UniRx;

public interface IFactoryRequirement : IDisposable
{
    IReadOnlyReactiveProperty<bool> IsProductionAllowed { get; }
}
