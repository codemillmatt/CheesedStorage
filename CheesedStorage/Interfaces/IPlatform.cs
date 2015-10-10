using System;

using SQLite.Net.Interop;

namespace CheesedStorage.Local
{
	public interface IPlatform
	{
		string DatabasePath { get; }
		ISQLitePlatform OSPlatform { get; }
	}
}

