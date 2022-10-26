using System.Globalization;
using CountingDiffCheck.Model;

Dictionary<long, List<User>> nameHistory = new();

var file = Path.Join(Environment.CurrentDirectory, "count_prev.csv");
var newFile = Path.Join(Environment.CurrentDirectory, "count_now.csv");

List<Message> GetMessages(string path)
{
    using var reader = new StreamReader(path);
    using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);
    csv.Context.RegisterClassMap<MessageMapper>();
    return csv.GetRecords<Message>().ToList();
}

var oldMessages = GetMessages(file);
var newMessages = GetMessages(newFile);

var oldUsers = oldMessages.Select(message => message.Sender).Distinct();
var newUsers = newMessages.Select(message => message.Sender).Distinct();

foreach (var user in oldUsers)
{
    if (!nameHistory.ContainsKey(user.UserID))
        nameHistory.Add(user.UserID, new()
        {
            user
        });
}

foreach (var user in newUsers)
{
    if (nameHistory.ContainsKey(user.UserID))
    {
        var prev = nameHistory[user.UserID];
        bool rename = true;
        foreach (var historyName in prev)
        {
            if (Equals(historyName.UserName, user.UserName))
                rename = false;
        }
        if (rename)
            nameHistory[user.UserID].Add(user);
    }
}

foreach (var name in nameHistory)
{
    Console.WriteLine($"{name.Key}: {string.Join(", ", name.Value.Select(user => user.UserName))}");
}