using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CheesedStorage.Local
{
	public interface ICheeseStorageService
	{
		Task<Uri> SavePhotoAsync(byte[] photo);
		Task<Uri> SaveAudioAsync(byte[] audio);
		Task<Stream> LoadBlobAsync(Uri location);
		Task<byte[]> LoadBlobBytesAsync (Uri location);
	}
}

