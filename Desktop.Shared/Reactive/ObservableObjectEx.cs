﻿using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Immense.RemoteControl.Desktop.Shared.Reactive;

public class ObservableObjectEx : ObservableObject
{
    private readonly ConcurrentDictionary<string, object?> _backingFields = new();

    public void NotifyPropertyChanged(string propertyName)
    {
        OnPropertyChanged(propertyName);
    }

    protected T? Get<T>([CallerMemberName] string propertyName = "", T? defaultValue = default)
    {
        if (_backingFields.TryGetValue(propertyName, out var value) &&
            value is T typedValue)
        {
            return typedValue;
        }

        return defaultValue;
    }

    protected void Set<T>(T newValue, [CallerMemberName] string propertyName = "")
    {
        _backingFields.AddOrUpdate(propertyName, newValue, (k, v) => newValue);
        OnPropertyChanged(propertyName);
    }
}
