namespace CountingJournal.Helpers;

public static class TaskExtensions
	{
		public static async void Await(this Task task, Action? onComplete = null, Action<Exception>? onError = null)
		{
			try
			{
				await task;
				onComplete?.Invoke();
			}
			catch (Exception e)
			{
				onError?.Invoke(e);
			}
		}
	}
