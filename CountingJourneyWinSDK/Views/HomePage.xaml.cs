using CountingJournal.Model;
using CountingJournal.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
        if (item is Message msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Attachments))
                return WithPics;
            return PlainText;
        }
        return base.SelectTemplate(item);
    }
}
