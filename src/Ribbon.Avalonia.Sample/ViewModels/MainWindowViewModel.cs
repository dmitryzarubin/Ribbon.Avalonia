using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace Ribbon.Avalonia.Sample.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly string _help = "Help requested!";

    private string _lastActionText = "none";

    private bool _showContextualGroup1 = true;

    private bool _showContextualGroup2;

    private bool _showContextualGroup3;
    private bool _isToggled;

    public string LastActionText
    {
        get => _lastActionText;
        set
        {
            _lastActionText = value;
            NotifyPropertyChanged();
        }
    }

    public bool IsToggled
    {
        get => _isToggled;
        set
        {
            _isToggled = value;
            NotifyPropertyChanged();
            LastActionText = value ? "Toggle onn" : "Toggle off";
        }
    }
    
    public bool ShowContextualGroup1
    {
        get => _showContextualGroup1;
        set
        {
            _showContextualGroup1 = value;
            NotifyPropertyChanged();
        }
    }

    public bool ShowContextualGroup2
    {
        get => _showContextualGroup2;
        set
        {
            _showContextualGroup2 = value;
            NotifyPropertyChanged();
        }
    }

    public bool ShowContextualGroup3
    {
        get => _showContextualGroup3;
        set
        {
            _showContextualGroup3 = value;
            NotifyPropertyChanged();
        }
    }


    public RecentDocumentListViewModel RecentDocumentList { get; } = new();
    
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnClickCommand(object parameter)
    {
        var paramString = "[NO CONTENT]";

        if (parameter != null)
        {
            if (parameter is string str)
                paramString = str;
            else
                paramString = parameter.ToString();
        }

        Console.WriteLine("OnClickCommand invoked: " + paramString);
        LastActionText = paramString;
    }
    
    public void OnExitCommand(object parameter)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.Shutdown();
        }
    }

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void HelpCommand(object parameter)
    {
        Console.WriteLine(_help);
        LastActionText = _help;
    }
}