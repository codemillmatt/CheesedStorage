using System;

using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;

namespace CheesedStorage.Local
{
	public class AudioVideoViewModel 
	{
		readonly ICheeseStorageService _storageService;

		public AudioVideoViewModel ()
		{
			_storageService = DependencyService.Get<ICheeseStorageService> ();
		}

		public byte[] AudioBytes {
			get;
			set;
		}

		public byte[] PhotoBytes {
			get;
			set;
		}

		public async Task SavePhotoToAzure() 
		{
			if (PhotoBytes != null && PhotoBytes.Length > 0) {
				// Use the storage service to save to Azure
				var azurePhotoId = await _storageService.SavePhotoAsync (PhotoBytes);

				// Broadcast a message with the id that was saved
				var msgArgs = new StorageCompleteMessage() { StorageUrl = azurePhotoId.AbsoluteUri};
				Console.WriteLine (azurePhotoId);
				MessagingCenter.Send<AudioVideoViewModel, StorageCompleteMessage> (this, AzureConstants.PhotoStorageComplete, msgArgs);
			}
		}

		public async Task SaveAudioToAzure()
		{
			if (AudioBytes != null && AudioBytes.Length > 0) {
				// Use the storage service to save to Azure
				var azureAudioId = await _storageService.SaveAudioAsync (AudioBytes);

				// Broadcast a message with the id that was saved
				var msgArgs = new StorageCompleteMessage() { StorageUrl = azureAudioId.AbsoluteUri};
				MessagingCenter.Send<AudioVideoViewModel, StorageCompleteMessage> (this, AzureConstants.AudioStorageComplete, msgArgs);
			}
		}
	}
}

