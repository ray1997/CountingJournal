using CountingJournal.Model;

namespace CountingJournal.Helpers.Number;
public interface ICounting
{
    int Validate(Message input, int expectedNumber);
}