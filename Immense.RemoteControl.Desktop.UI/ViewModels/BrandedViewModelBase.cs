﻿using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Immense.RemoteControl.Desktop.Shared.Abstractions;
using Immense.RemoteControl.Desktop.Shared.Reactive;
using Immense.RemoteControl.Shared.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace Immense.RemoteControl.Desktop.UI.ViewModels;

public interface IBrandedViewModelBase
{
    Bitmap? Icon { get; set; }
    string ProductName { get; set; }
    WindowIcon? WindowIcon { get; set; }
}

public class BrandedViewModelBase : ObservableObject, IBrandedViewModelBase
{
    private static BrandingInfoBase? _brandingInfo;
    protected readonly ILogger<BrandedViewModelBase> _logger;
    protected readonly IUiDispatcher _dispatcher;
    private readonly IBrandingProvider _brandingProvider;


    public BrandedViewModelBase(
        IBrandingProvider brandingProvider,
        IUiDispatcher dispatcher,
        ILogger<BrandedViewModelBase> logger)
    {
        _brandingProvider = brandingProvider;
        _dispatcher = dispatcher;
        _logger = logger;

        ApplyBrandingImpl();
    }

    public Bitmap? Icon
    {
        get => Get<Bitmap?>();
        set => Set(value);
    }

    public string ProductName
    {
        get => Get<string?>() ?? "Remote Control";
        set => Set(value ?? "Remote Control");
    }

    public WindowIcon? WindowIcon
    {
        get => Get<WindowIcon?>();
        set => Set(value);
    }

    private void ApplyBrandingImpl()
    {
        _dispatcher.Invoke(() =>
        {
            try
            {
                _brandingInfo ??= _brandingProvider.CurrentBranding;

                ProductName = _brandingInfo.Product;

                if (_brandingInfo.Icon?.Any() == true)
                {
                    using var imageStream = new MemoryStream(_brandingInfo.Icon);
                    Icon = new Bitmap(imageStream);
                }
                else
                {
                    using var imageStream =
                        Assembly
                            .GetExecutingAssembly()
                            .GetManifestResourceStream("Immense.RemoteControl.Desktop.Shared.Assets.DefaultIcon.png") ?? new MemoryStream();

                    Icon = new Bitmap(imageStream);
                }

                WindowIcon = new WindowIcon(Icon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying branding.");
            }
        });
    }
}
