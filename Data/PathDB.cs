

namespace MauiApp1.Data
{
	public class PathDB
	{
		public static string GetDatabasePath(string DbName)
		{
			string pathDbSQLite = string.Empty;

			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				pathDbSQLite = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				pathDbSQLite = Path.Combine(pathDbSQLite, DbName);
			}
			else if (DeviceInfo.Platform == DevicePlatform.iOS)
			{
				pathDbSQLite = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				pathDbSQLite = Path.Combine(pathDbSQLite, "..", "Library", DbName);
			}
			else
			{
				pathDbSQLite = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				pathDbSQLite = Path.Combine(pathDbSQLite, DbName);
			}
			return pathDbSQLite;
		}
	}
}
