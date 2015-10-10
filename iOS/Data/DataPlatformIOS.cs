using System;
using System.IO;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(CheesedStorage.Local.iOS.DataPlatformIOS))]
namespace CheesedStorage.Local.iOS
{
	
	public class DataPlatformIOS : IPlatform
	{
		public DataPlatformIOS ()
		{
		}

		#region IPlatform implementation

		public string DatabasePath {
			get {
				return Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cheese.db3");
			}
		}

		public ISQLitePlatform OSPlatform {
			get {
				return new SQLitePlatformIOS ();
			}
		}

		#endregion
	}
}

