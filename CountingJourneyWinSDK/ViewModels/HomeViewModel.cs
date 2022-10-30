using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CountingJournal.Helpers;
using CountingJournal.Helpers.Text;
using CountingJournal.Model;
using Microsoft.UI.Xaml;
using Windows.Storage;
using SAP = Windows.Storage.AccessCache.StorageApplicationPermissions;
using MSG = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
using CountingJournal.Model.Messages;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;

namespace CountingJournal.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<MessageViewModel>? messages;

    [ObservableProperty]
    private ObservableCollection<MessageViewModel>? countingMessages;

    [ObservableProperty]
    StorageFile? cSVFile = null;
    Settings? appConfig = null;

    [ObservableProperty]
    int latestCountNumber = 0;

    public HomeViewModel()
    {
        appConfig = App.GetService<Settings>();
        OnPropertyChanged(nameof(ShowCSVGather));
        MSG.Default.Register<HomeViewModel, ConfirmThisAsFillerMessage, string>(this, Token.ConfirmFillerMSGToken,
            (home, fillerMSG) => AddOrRemoveConfirmedFillers(fillerMSG));
        ConfirmedFillers = new();
        //Load filler list
        DirectoryInfo dir = new(@"D:\UserData\Desktop\Counting-20221026-1800\Fillers");
        if (!dir.Exists)
            dir.Create();
        var files = dir.GetFiles();
        foreach (var file in files)
        {
            var json = File.ReadAllText(file.FullName);
            var deserialized = JsonSerializer.Deserialize<Message?>(json);
            if (deserialized is null)
            {
                continue;
            }
            ConfirmedFillers.Add(deserialized);
        }
    }

    [ObservableProperty]
    ObservableCollection<Message> confirmedFillers = new();

    private void AddOrRemoveConfirmedFillers(ConfirmThisAsFillerMessage fillerMSG)
    {
        if (fillerMSG.ConfirmAsFiller)
        {
            //Add to filler list
            ConfirmedFillers.Add(fillerMSG.ConfirmFiller);
            //Save to json
            DirectoryInfo folder = new(@"D:\UserData\Desktop\Counting-20221026-1800\Fillers");
            if (!folder.Exists)
                folder.Create();
            string fileName = $"{fillerMSG.ConfirmFiller.SendAt.Ticks}.json";
            File.WriteAllTextAsync(Path.Join(folder.FullName, fileName), JsonSerializer.Serialize<Message>(fillerMSG.ConfirmFiller, 
                new JsonSerializerOptions() 
                { 
                    WriteIndented = true 
                }));
        }
        else
        {
            if (!ConfirmedFillers.Contains(fillerMSG.ConfirmFiller))
            {
                return;
            }
            ConfirmedFillers.Remove(fillerMSG.ConfirmFiller);
            string fileName = $"{fillerMSG.ConfirmFiller.SendAt.Ticks}.json";
            DirectoryInfo folder = new(@"D:\UserData\Desktop\Counting-20221026-1800\Fillers");
            if (!folder.Exists)
                return;
            FileInfo savedFiller = new(Path.Join(folder.FullName, fileName));
            if (!savedFiller.Exists)
                return;
            savedFiller.Delete();
        }
    }

    [RelayCommand]
    private async Task ListingMessages()
    {
        //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        //StorageFile storageFile = await storageFolder.GetFileAsync(filename);
        //var stream = System.IO.File.Open(storageFile.Path, FileMode.Open);
        //bool check = stream is FileStream;
        if (CSVFile is null)
            await GetCSVFile();

        var stream = File.Open(CSVFile.Path, FileMode.Open);
        if (stream is null)
            return;
        using var reader = new StreamReader(stream);
        using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<MessageMapper>();
        var items = csv.GetRecords<Message>().Select(msg => new MessageViewModel(msg, ConfirmedFillers.Any(confirm => msg.SendAt.Ticks == confirm.SendAt.Ticks)));
        Messages = new ObservableCollection<MessageViewModel>(items);
        //CountingMessages = new(Messages.Select(i => IsCounting.Decide(i)));
    }

    [ObservableProperty]
    private bool showCountingInfoBar = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HideLabelWhileCounting))]
    private bool isCountingRightNow = false;

    public CommandBarDefaultLabelPosition HideLabelWhileCounting => 
        IsCountingRightNow ? CommandBarDefaultLabelPosition.Collapsed : CommandBarDefaultLabelPosition.Right;

    [ObservableProperty]
    string latestMessageCount = "";

    [ObservableProperty]
    int totalParticipant = -1;

    [RelayCommand]
    private async Task Counting()
    {
        IsCountingRightNow = true;
        if (CountingMessages is null)
            CountingMessages = new();
        if (Messages is null)
            await ListingMessages();
        else
        {
            Messages.Clear();
            await ListingMessages();
        }
        if (TotalParticipant < 0)
        {
            TotalParticipant = Messages.Select(msg => msg.Sender.UserID).Distinct().Count();
        }
        IsCounting.ResetLastSender();
        LatestCountNumber = 0;
        TotalFiller = 0;
        CountingMessages.Clear();
        MessageViewModel? previousAnchor = null;
        int acceptableGapSize = 2;
        var previous = string.Empty;
        var next = string.Empty;
        foreach (var msg in Messages)
        {
            LatestMessageCount = $"Processing: {msg.Content}";
            if (msg.ConfirmedFiller)
            {
                TotalFiller++;
                continue;
            }            
            if (previousAnchor is null)
            {
                previous = "0";
            }
            else
            {
                previous = previousAnchor.AsNumber > 0 ?
                    previousAnchor.AsNumber.ToString() :
                    previousAnchor.Content;
            }

            next = msg.Content;

            bool shouldSwitchAnchor = false;

            //Attempted flags
            bool triedFilterOnlyNumber = false;
            bool presumeAllNumberSpread = false;
            bool triedConvertFromThaiNum = false;
            bool triedThaiText = false;
            bool triedYamokConvert = false;
            bool triedRoman = false;
            bool isDotNine = false;
            bool triedCalculate = false;
            bool triedConvert = false;
            bool isReferenceCheck = false;
            bool isPicCheck = false;
            await Task.Run(() =>
            {
                Debug.WriteLine($"Attemp parsing number: {msg.Content}" +
                $"\r\nNumber | Spread | TH Num | TH Txt |  Yamok |  Roman |     .9 |    Pow |  Weird |    Ref |    Pic" +
                $"\r\n{(triedFilterOnlyNumber ? "Y" : "n"),-6} | " +
                $"{(presumeAllNumberSpread ? "Y" : "n"),-6} | " +
                $"{(triedConvertFromThaiNum ? "Y" : "n"),-6} | " +
                $"{(triedThaiText ? "Y" : "n"),-6} | " +
                $"{(triedYamokConvert ? "Y" : "n"),-6} | " +
                $"{(triedRoman ? "Y" : "n"),-6} | " +
                $"{(isDotNine ? "Y" : "n"),-6} | " +
                $"{(triedCalculate ? "Y" : "n"),-6} | " +
                $"{(triedConvert ? "Y" : "n"),-6} | " +
                $"{(isReferenceCheck ? "Y" : "n"),-6} | " +
                $"{(isPicCheck ? "Y" : "n"),-6} | ");
            });

        retry:
            shouldSwitchAnchor = IsCountingDeciderV2.ShouldAddIn(previous, next, acceptableGapSize);
            if (string.IsNullOrEmpty(msg.Content))
            {
                goto nomsg;
            }
            if (shouldSwitchAnchor)
            {
                acceptableGapSize = 2;
                msg.IsFiller = false;
                msg.AsNumber = int.Parse(next);
                previousAnchor = msg;
                LatestCountNumber = int.Parse(next);
                continue;
            }
            else
                next = msg.Content;
            acceptableGapSize++;

            if (!triedFilterOnlyNumber)
            {
                next = IsCountingDeciderV2.PerhapsTheNumberIsMixedWithText(msg);
                triedFilterOnlyNumber = true;
                goto retry;
            }
            if (!presumeAllNumberSpread)
            {
                string testing = IsCountingDeciderV2.PerhapsTheNumberIsMixedWithTextAndNumber(msg, previous);
                next = !string.IsNullOrEmpty(testing) ? testing : msg.Content;
                presumeAllNumberSpread = true;
                goto retry;
            }
            if (!triedConvertFromThaiNum)
            {
                next = IsCountingDeciderV2.PerhapsThereIsThaiNumber(msg);
                triedConvertFromThaiNum = true;
                goto retry;
            }
            if (!triedThaiText)
            {
                try
                {
                    var test = ThaiTextNumber.ConvertToNumber(next);
                    if (test > 0)
                        next = test.ToString();
                    triedThaiText = true;
                    goto retry;
                }
                catch
                {
                    triedThaiText = true;
                }
            }
            if (next.Contains(RepeaterSymbol.Repeater) && !triedYamokConvert)
            {
                next = RepeaterSymbol.Translate(next, int.Parse(previous) + 1);
                triedYamokConvert = true;
                goto retry;
            }
            if (!triedRoman)
            {
                var test = Roman.ToNumber(next);
                if (test > 0)
                    next = test.ToString();
                triedRoman = true;
                goto retry;
            }
            if (!isDotNine && next.Contains('.') && next.Contains('9'))
            {
                bool result = float.TryParse(next, out float parsed);
                if (result)
                {
                    int converse = (int)parsed;
                    isDotNine = true;
                    next = converse.ToString();
                    goto retry;
                }
                isDotNine = true;
            }
            if (!triedCalculate)
            {
                try
                {
                    var result = BasicCalculation.Calculate(next, $"{int.Parse(previous) + 1}");
                    next = result == string.Empty ? next : result;
                    triedCalculate = true;
                    goto retry;
                }
                catch { }
                triedCalculate = true;
            }
            if (!triedConvert)
            {
                var result = WeirdNumber.ToNormal(next);
                result = TinyNumber.ToNormal(result);
                next = result;
                triedConvert = true;
                goto retry;
            }
            if (!isReferenceCheck)
            {
                if (Manual.MemeReference.ContainsKey(next))
                {
                    next = Manual.MemeReference[next].ToString();
                }
                isReferenceCheck = true;
                goto retry;
            }
            nomsg:
            if (!string.IsNullOrEmpty(msg.Attachments) && 
                string.IsNullOrEmpty(msg.Content) && !isPicCheck)
            {
                if (Manual.ImageToNumber.ContainsKey(msg.Attachments))
                {
                    next = Manual.ImageToNumber[msg.Attachments].ToString();
                }
                isPicCheck = true;
                goto retry;
            }

            //Finally give up
            msg.ExpectNumber = int.Parse(previous) + 1;
            msg.IsFiller = true;
            TotalFiller++;
        }
        CountingMessages = new(Messages.Where(msg => Consider(msg)));
        TotalFiller = Messages.Where(msg => msg.IsFiller).Count();
        LatestMessageCount = "";
        IsCountingRightNow = false;
    }

    private bool Consider(MessageViewModel msg)
    {
        if (ShowFillers && msg.IsFiller && !HideOnlyConfirmedFiller)
            return true;
        else if (!ShowFillers && HideOnlyConfirmedFiller)
        {
            if (msg.ConfirmedFiller)
                return false;
            else if (msg.IsFiller && !msg.ConfirmedFiller)
                return true;
            else
                return true;
        }
        else if (ShowConfirmed && !msg.IsFiller)
            return true;
        return false;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSelectConfirmFiller))]
    bool showFillers = true;

    public bool CanSelectConfirmFiller => ShowFillers == false;

    [ObservableProperty]
    bool hideOnlyConfirmedFiller = false;

    [ObservableProperty]
    bool showConfirmed = true;

    [ObservableProperty]
    int totalFiller = -1;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMessageSelected))]
    [NotifyCanExecuteChangedFor(nameof(SkipToNextFillerCommand))]
    [NotifyCanExecuteChangedFor(nameof(SkipToNextFillerNotCommand))]
    int selectedMessage = -1;

    public bool IsMessageSelected
        => SelectedMessage >= 0;

    [RelayCommand(CanExecute = nameof(IsMessageSelected))]
    private void SkipToNextFiller()
    {
        try
        {
            var next = SelectedMessage;
            do
            {
                next++;
            }
            while (!CountingMessages[next].IsFiller);
            SelectedMessage = next;
        }
        catch { }
    }

    [RelayCommand(CanExecute = nameof(IsMessageSelected))]
    private void SkipToNextFillerNot()
    {
        try
        {
            var next = SelectedMessage;
            do
            {
                next++;
            }
            while (CountingMessages[next].IsFiller);
            SelectedMessage = next;
        }
        catch { }
    }

    [RelayCommand]
    private void JumpToLatestConfirmedFiller()
    {
        var lastConfirmed = CountingMessages.Last(msg => msg.ConfirmedFiller);
        if (lastConfirmed is null)
            return;
        SelectedMessage = CountingMessages.IndexOf(lastConfirmed);
    }


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsJumpThroughFiller))]
    [NotifyPropertyChangedFor(nameof(IsJumpThroughNumber))]
    [NotifyPropertyChangedFor(nameof(IsJumpThroughConfirmedFiller))]
    private JumpMode selectedJump = JumpMode.Filler;

    public bool IsJumpThroughFiller => SelectedJump == JumpMode.Filler;
    public bool IsJumpThroughNumber => SelectedJump == JumpMode.Message;
    public bool IsJumpThroughConfirmedFiller => SelectedJump == JumpMode.ConfirmedFiller;

    [RelayCommand]
    private void JumpPrevious()
    {
        if (SelectedMessage < 0)
            return;
        try
        {
            var next = SelectedMessage;
            do
            {
                next--;
                if (next < 0)
                {
                    SelectedMessage = -1;
                    return;
                }
            }
            while (ConditionStillMet(next));

            SelectedMessage = next;
        }
        catch { }
    }

    [RelayCommand]
    private void JumpNext()
    {
        if (SelectedMessage >= CountingMessages.Count - 1)
            return;
        try
        {
            var next = SelectedMessage;
            do
            {
                next++;
                if (next >= CountingMessages.Count)
                {
                    SelectedMessage = CountingMessages.Count - 1;
                    return;
                }
            }
            while (ConditionStillMet(next));

            SelectedMessage = next;
        }
        catch { }
    }

    public bool ConditionStillMet(int index)
    {
        try
        {
            switch (SelectedJump)
            {
                case JumpMode.Filler:
                    return !CountingMessages[index].IsFiller;
                case JumpMode.Message:
                    return CountingMessages[index].IsFiller;
                case JumpMode.ConfirmedFiller:
                    return !CountingMessages[index].ConfirmedFiller;
            }
        }
        catch {  }
        return false;
    }

    [RelayCommand]
    private void FirstOfJump()
    {
        switch (SelectedJump)
        {
            case JumpMode.Filler:
                var firstFiller = CountingMessages.FirstOrDefault(msg => msg.IsFiller);
                if (firstFiller is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstFiller);
                break;
            case JumpMode.Message:
                var firstCount = CountingMessages.FirstOrDefault(msg => !msg.IsFiller);
                if (firstCount is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstCount);
                break;
            case JumpMode.ConfirmedFiller:
                var firstVerifiedFiller = CountingMessages.FirstOrDefault(msg => msg.ConfirmedFiller);
                if (firstVerifiedFiller is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstVerifiedFiller);
                break;
        }
    }

    [RelayCommand]
    private void LastOfJump()
    {
        switch (SelectedJump)
        {
            case JumpMode.Filler:
                var firstFiller = CountingMessages.LastOrDefault(msg => msg.IsFiller);
                if (firstFiller is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstFiller);
                break;
            case JumpMode.Message:
                var firstCount = CountingMessages.LastOrDefault(msg => !msg.IsFiller);
                if (firstCount is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstCount);
                break;
            case JumpMode.ConfirmedFiller:
                var firstVerifiedFiller = CountingMessages.LastOrDefault(msg => msg.ConfirmedFiller);
                if (firstVerifiedFiller is null)
                    return;
                SelectedMessage = CountingMessages.IndexOf(firstVerifiedFiller);
                break;
        }
    }
    public Visibility ShowCSVGather
    {
        get
        {
            if (appConfig is null)
                return Visibility.Visible;
            return string.IsNullOrEmpty(appConfig.CSVID)
            ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    [RelayCommand]
    private async Task GetCSVFile()
    {
        if (appConfig is not null &&
            !string.IsNullOrEmpty(appConfig.CSVID) &&
            !string.IsNullOrWhiteSpace(appConfig.CSVID))
        {
            CSVFile = await SAP.FutureAccessList.GetFileAsync(appConfig.CSVID);
            return;
        }

        StorageFile file = await StorageFile.GetFileFromPathAsync(@"D:\UserData\Desktop\Counting-20221026-1800\countTo10k.csv");
        appConfig.CSVID = SAP.FutureAccessList.Add(file);
        CSVFile = file;
    }

    [RelayCommand]
    private void ExportJSON()
    {
        var path = @"D:\UserData\Desktop\Counting-20221026-1800\final.json";
        if (File.Exists(path))
            File.Delete(path);
        var json = JsonSerializer.Serialize<ObservableCollection<Message>>(
            new(CountingMessages.Where(i => !i.ConfirmedFiller)), 
            new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });
        File.WriteAllText(path, json);
    }

    [RelayCommand]
    private void ShowGoToPrompt()
    {
        if (SelectedMessage < 0)
            SelectedMessage = 0;
        ShowGoToInput = true;
        MSG.Default.Send(new FocusOnInputBoxMessage(), Token.FocusOnInputTextBox);
    }

    [RelayCommand]
    private async Task GoToThisMessage()
    {
        if (string.IsNullOrEmpty(GotoInput))
        {
            ShowGoToInput = false;
            return;
        }
        var found = CountingMessages.Skip(SelectedMessage).FirstOrDefault(msg => msg.Content.Contains(GotoInput));
        if (found is null)
        {
            NoMessageFound = true;
            return;
        }
        SelectedMessage = CountingMessages.IndexOf(found);
        ShowGoToInput = false;
        GotoInput = string.Empty;
        MSG.Default.Send(new ScrollToCurrentItemMessage(), Token.PleaseScrollToCurrentItem);
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(GoToControlsIndex))]
    private bool showGoToInput = false;

    [ObservableProperty]
    private string gotoInput = string.Empty;

    public int GoToControlsIndex => ShowGoToInput ? 1000 : -1;

    [ObservableProperty]
    private bool noMessageFound = false;

    partial void OnNoMessageFoundChanged(bool value)
    {
        Task.Run(async () =>
        {
            await Task.Delay(2500);
        }).Await(() =>
        {
            NoMessageFound = false;
        });
    }
}

public enum JumpMode
{
    Filler,
    Message,
    ConfirmedFiller
}
