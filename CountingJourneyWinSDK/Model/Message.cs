using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace CountingJournal.Model;
public partial class Message : ObservableObject
{
    [ObservableProperty]
    private User? sender;

    [ObservableProperty]
    private DateTime sendAt;

    [ObservableProperty]
    private string? content;

    [ObservableProperty]
    private string? attachments;
}

public partial class User : ObservableObject
{
    [ObservableProperty]
    private long userID;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Name))]
    [NotifyPropertyChangedFor(nameof(Discriminator))]
    string userName = string.Empty;

    [JsonIgnore]
    public string Name
    {
        get
        {
            if (string.IsNullOrEmpty(UserName) || !UserName.Contains('#'))
            {
                return string.Empty;
            }

            var span = UserName.AsSpan();
            return new string(span[..^5]);
        }
    }

    [JsonIgnore]
    public int Discriminator
    {
        get
        {
            if (string.IsNullOrEmpty(UserName) || !UserName.Contains('#'))
            {
                return -1;
            }

            var span = UserName.AsSpan();
            var succeed = int.TryParse(span[^4..], out var result);
            return succeed ? result : -1;
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj is not User)
            return false;

        if (obj is null)
            return false;

        if (obj is User compare)
        {
            if (Name == compare.Name
                && Discriminator == compare.Discriminator)
                return true;
            else
            {
                if (UserID == compare.UserID)
                    return true;
            }
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class MessageMapper : ClassMap<Message>
{
    public MessageMapper()
    {
        Map(m => m.Sender.UserName).Name("Author");
        Map(m => m.Sender.UserID).Name("AuthorID");
        Map(m => m.SendAt).TypeConverter<DateTimeConverter>().Name("Date");
        Map(m => m.Content).Name("Content");
        Map(m => m.Attachments).Name("Attachments");
    }
}

public class DateTimeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text.Contains("AM") || text.Contains("PM")) //28-Apr-22 07:25 PM
            return DateTime.ParseExact(text, "dd-MMM-yy hh:mm:ss tt", CultureInfo.InvariantCulture.DateTimeFormat);
        return DateTime.ParseExact(text, "dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture.DateTimeFormat);
    }

    public enum month
    {
        Jan = 1,
        Feb = 2,
        Mar = 3,
        Apr = 4,
        May = 5,
        Jun = 6,
        Jul = 7,
        Aug = 8,
        Sep = 9,
        Oct = 10,
        Nov = 11,
        Dec = 12
    }
}