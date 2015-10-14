using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace CheesedStorage.Local
{
	public class RatingDetailViewModel : INotifyPropertyChanged, IDisposable
	{
		readonly ICheeseDataService _dataService;
		readonly ICheeseStorageService _storageService;

		ContentPage Page;

		public string Title {
			get;
			private set;
		}

		public CheeseAndRating RatingDetails {
			get;
			private set;
		}

		public string RatingDescription {
			get;
			set;
		}

		public string CheeseName {
			get;
			set;
		}

		public string DairyName {
			get;
			set;
		}

		public Stream CheesePhoto {
			get;
			set;
		}

		public Uri CheesePhotoUri {
			get;
			set;
		}

		public byte[] PhotoBytes {
			get;
			set;
		}

		public RatingDetailViewModel (CheeseAndRating rating, ContentPage page)
		{
			_dataService = DependencyService.Get<ICheeseDataService> ();
			_storageService = DependencyService.Get<ICheeseStorageService> ();

			Page = page;

			RatingDetails = rating;
		}

		private async Task ExecuteGetRatingDetailsCommand ()
		{
			try {
				GetRatingDetailsCommand.ChangeCanExecute ();

				var cheeseDetail = await _dataService.GetCheeseDetailsAsync (RatingDetails.CheeseId);

				RatingDetails.CheeseName = cheeseDetail.CheeseName;
				this.CheeseName = cheeseDetail.CheeseName;
				OnPropertyChanged ("CheeseName");

				RatingDetails.DairyName = cheeseDetail.DairyName;
				this.DairyName = cheeseDetail.DairyName;
				OnPropertyChanged ("DairyName");

				RatingDescription = string.Format ("This {0} cheese was rated on {1}, and was given {2} wedges." +
				System.Environment.NewLine + "The taste complexities were noted by the following: {3}",
					RatingDetails.CheeseName, RatingDetails.DateRated.ToShortDateString (), RatingDetails.WedgeRating,
					RatingDetails.Notes);

				OnPropertyChanged ("RatingDescription");

				Title = RatingDetails.CheeseName;
				OnPropertyChanged ("Title");

				// Grab the cheese photo blob
				if (string.IsNullOrEmpty (RatingDetails.PhotoUrl) == false) {
					CheesePhotoUri = new Uri(RatingDetails.PhotoUrl);
					OnPropertyChanged("CheesePhotoUri");
				}

			} catch (NoInternetException) {
				await Page.DisplayAlert ("No Internet!", "Cannot Access The Internet!", "Darn!");

			} finally {
				GetRatingDetailsCommand.ChangeCanExecute ();
			}
		}

		private Command _GetRatingDetailsCommand;

		public Command GetRatingDetailsCommand {
			get {
				return _GetRatingDetailsCommand ?? (_GetRatingDetailsCommand = new Command (async () => await ExecuteGetRatingDetailsCommand ()));
			}
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			if (CheesePhoto != null) {
				CheesePhoto.Dispose ();
				CheesePhoto = null;
			}
		
		}

		#endregion


		#region INotifyPropertyChanged implementation

		protected virtual void OnPropertyChanged (string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}

