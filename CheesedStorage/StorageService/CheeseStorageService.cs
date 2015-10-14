using System;

using System.Net.Http;
using System.Threading;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;

[assembly: Dependency (typeof(CheesedStorage.Local.CheeseStorageService))]
namespace CheesedStorage.Local
{
	public class CheeseStorageService : ICheeseStorageService
	{
		#region ICheeseStorageService Implementation

		public async Task<Uri> SavePhotoAsync (byte[] photo)
		{
			return await SaveToAzure (photo, true, AzureConstants.StoragePhotoContainerUrl);
		}

		public async Task<Uri> SaveAudioAsync (byte[] audio)
		{
			return await SaveToAzure (audio, false, AzureConstants.StorageAudioContainerUrl);
		}

		public async Task<Stream> LoadBlobAsync (Uri location)
		{
			// The container is setup for public read access
			// do not need to generate credentials
			var cloudBlob = new CloudBlockBlob(location);

			MemoryStream imageStream = new MemoryStream ();

			await cloudBlob.DownloadToStreamAsync (imageStream);

			return imageStream;
		}

		public async Task<byte[]> LoadBlobBytesAsync(Uri location)
		{
			var blob = new CloudBlob (location);

			await blob.FetchAttributesAsync ();

			byte[] target = new byte[blob.Properties.Length];

			await blob.DownloadToByteArrayAsync (target, 0);
		
			return target;
		}

		#endregion

		private async Task<StorageCredentials> GetStorageCredentials (bool forPhotos)
		{
			// First get the SAS Token from Mobile Services API
			var mobileServiceClient = new MobileServiceClient (AzureConstants.MobileServiceUrl, 							
				                          AzureConstants.MobileServiceAppKey);

			// Determine which container we want and create a dictionary to request it
			var containerName = forPhotos ? AzureConstants.StoragePhotoContainerName : AzureConstants.StorageAudioContainerName;

			var requestDict = new Dictionary<string,string> () { { "container", containerName } };

			// Invoke the custom API in the mobile services to get the storage SAS token
			var token = await mobileServiceClient
				.InvokeApiAsync<string> (AzureConstants.MobileServiceAPISasName, HttpMethod.Get, requestDict);

			// Create the storage credentials
			return new StorageCredentials (token);
		}
			
		private async Task<Uri> SaveToAzure(byte[] stream, bool isPhoto, string containerUrl) 
		{
			var creds = await GetStorageCredentials (isPhoto);

			var container = new CloudBlobContainer (new Uri (containerUrl), creds);

			var blockBlob = container.GetBlockBlobReference (Guid.NewGuid ().ToString ());

			await blockBlob.UploadFromByteArrayAsync (stream, 0, stream.Length);

			return blockBlob.Uri;
		}
	}
}

