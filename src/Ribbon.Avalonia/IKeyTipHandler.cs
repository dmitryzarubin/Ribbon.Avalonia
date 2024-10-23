using Avalonia.Input;

namespace Ribbon.Avalonia;

public interface IKeyTipHandler
{
    void ActivateKeyTips(Ribbon ribbon, IKeyTipHandler prev);

    bool HandleKeyTipKeyPress(Key key);
}