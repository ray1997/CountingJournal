using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MSG = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
using CountingJournal.Model;
using Microsoft.UI.Xaml;
using CountingJournal.Model.Messages;

namespace CountingJournal.ViewModels;
public partial class MessageViewModel : Message
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmThisMessageAsFillerCommand))]
    [NotifyCanExecuteChangedFor(nameof(RevertFillerConfirmationCommand))]
    [NotifyPropertyChangedFor(nameof(FillerIcon))]
    [NotifyPropertyChangedFor(nameof(FillerTooltip))]
    private bool confirmedFiller = false;

    public string FillerIcon
    {
        get
        {
            if (ConfirmedFiller)
                return "\uE107";
            else if (IsFiller)
                return "\uE10A";
            else
                return "\uE10B";
        }
    }

    public string FillerTooltip
    {
        get
        {
            if (ConfirmedFiller)
                return "100% filler";
            else if (IsFiller)
                return "Not sure if it's filler";
            else
                return "Not filler";
        }
    }

    public bool CanConfirmAsFiller => ConfirmedFiller == false;
    public bool CanConfirmAsNotFiller => ConfirmedFiller == true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowIfItIsFiller))]
    [NotifyPropertyChangedFor(nameof(FillerIcon))]
    [NotifyPropertyChangedFor(nameof(FillerTooltip))]
    private bool isFiller = false;

    public Visibility ShowIfItIsFiller => 
        IsFiller ? Visibility.Visible : Visibility.Collapsed;

    [ObservableProperty]
    private bool isChecked = false;

    [ObservableProperty]
    private int asNumber = -1;

    [ObservableProperty]
    private int expectNumber = -1;

    public MessageViewModel(Message source)
    {
        Sender = source.Sender;
        SendAt = source.SendAt;
        Content = source.Content;
        Attachments = source.Attachments;
    }

    public MessageViewModel(Message source, bool markedFiller)
    {
        Sender = source.Sender;
        SendAt = source.SendAt;
        Content = source.Content;
        Attachments = source.Attachments;
        ConfirmedFiller = IsFiller = markedFiller;

    }

    [RelayCommand(CanExecute = nameof(CanConfirmAsFiller))]
    private void ConfirmThisMessageAsFiller()
    {
        ConfirmedFiller = true;
        MSG.Default.Send(new ConfirmThisAsFillerMessage(this, true), Token.ConfirmFillerMSGToken);
    }

    [RelayCommand(CanExecute = nameof(CanConfirmAsNotFiller))]
    private void RevertFillerConfirmation()
    {
        ConfirmedFiller = false;
        MSG.Default.Send(new ConfirmThisAsFillerMessage(this, false), Token.ConfirmFillerMSGToken);
    }
}
