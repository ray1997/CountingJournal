using CountingJournal.Model;
using CountingJournal.Model.Messages;
using CountingJournal.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Messenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace CountingJournal.Views;

[CommunityToolkit.Mvvm.ComponentModel.INotifyPropertyChanged]
public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
        Messenger.Default.Register<HomePage, FocusOnInputBoxMessage, string>(this, Token.FocusOnInputTextBox,
            (me, msg) => FocusOnTextBoxOnShow(me, msg));
        Messenger.Default.Register<HomePage, ScrollToCurrentItemMessage, string>(this, Token.PleaseScrollToCurrentItem,
            (me, msg) => ScrollMessageToCurrent(me, msg));
    }

    private void FocusOnTextBoxOnShow(HomePage me, FocusOnInputBoxMessage msg)
    {
        gotoInput.Focus(FocusState.Keyboard);
    }

    private void ScrollMessageToCurrent(HomePage me, ScrollToCurrentItemMessage msg)
    {
        try
        {
            mainListView.ScrollIntoView(ViewModel.CountingMessages[ViewModel.SelectedMessage]);
        }
        catch { }
    }

    private void SetJumpMode(object sender, RoutedEventArgs e)
    {
        if (sender is RadioMenuFlyoutItem item)
        {
            switch (item.Tag.ToString())
            {
                case "filler": ViewModel.SelectedJump = JumpMode.Filler; break;
                case "number": ViewModel.SelectedJump = JumpMode.Message; break;
                case "confirm": ViewModel.SelectedJump = JumpMode.ConfirmedFiller; break;
            }
        }
    }

    private void SubmitAttempt(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Escape)
        {
            e.Handled = true;
            ViewModel.GotoInput = string.Empty;
            ViewModel.ShowGoToInput = false;
            return;
        }
        if (e.Key != Windows.System.VirtualKey.Enter)
        {
            return;
        }
        e.Handled = true;
        ViewModel.GoToThisMessageCommand.Execute(null);
    }
}

public class MessageTemplate : DataTemplateSelector
{
    public DataTemplate PlainText
    {
        get;set;
    }

    public DataTemplate WithPics
    {
        get;set;
    }
    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is not Message || item is not MessageViewModel)
            return base.SelectTemplate(item);
        if (item is MessageViewModel msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Attachments))
                return WithPics;
            return PlainText;
        }
        return base.SelectTemplate(item);
    }
}
