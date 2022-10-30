using System.ComponentModel;

namespace CountingJournal.Model.Messages;


public class Token
{
    public const string ConfirmFillerMSGToken = nameof(ConfirmFillerMSGToken);
    public const string PleaseScrollToCurrentItem = nameof(PleaseScrollToCurrentItem);
    public const string FocusOnInputTextBox = nameof(FocusOnInputTextBox);
}

public class ConfirmThisAsFillerMessage
{
    public Message ConfirmFiller { get; private set; }
    public bool ConfirmAsFiller { get;private set; }

    public ConfirmThisAsFillerMessage(Message confirmFiller, bool isFiller = true)
    {
        ConfirmFiller = confirmFiller;
        ConfirmAsFiller = isFiller;
    }
}

public class ScrollToCurrentItemMessage
{

}

public class FocusOnInputBoxMessage
{
}