﻿using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace CheesedStorage.Local
{
	public class AddRatingViewModel
	{
		readonly ICheeseDataService _dataService;
		ContentPage Page;

		public CheeseAndRating RatingToAdd {
			get;
			set;
		}

		public AddRatingViewModel (Cheese cheese, ContentPage page)
		{
			_dataService = DependencyService.Get<ICheeseDataService> ();

			MessagingCenter.Subscribe<AudioVideoViewModel, StorageCompleteMessage> (
				this, AzureConstants.PhotoStorageComplete,
				(avvm, scm) => {
					RatingToAdd.PhotoUrl = scm.StorageUrl;
				});

			MessagingCenter.Subscribe<AudioVideoViewModel, StorageCompleteMessage> (
				this, AzureConstants.AudioStorageComplete,
				(avvm, scm) => {
					RatingToAdd.AudioUrl = scm.StorageUrl;
				});

			RatingToAdd = new CheeseAndRating () {
				CheeseId = cheese.CheeseId,
				CheeseName = cheese.CheeseName,
				DairyName = cheese.DairyName
			};

			Page = page;
		}

		private async Task ExecuteSaveRatingCommand ()
		{
			try {
				AddRatingCommand.ChangeCanExecute ();

				var rating = new Rating () {
					CheeseId = RatingToAdd.CheeseId,
					DateRated = DateTime.Now,
					Notes = RatingToAdd.Notes,
					WedgeRating = RatingToAdd.WedgeRating,
					PhotoUrl = RatingToAdd.PhotoUrl ?? string.Empty,
					AudioUrl = RatingToAdd.AudioUrl ?? string.Empty
				};
						
				rating = await _dataService.RateCheeseAsync (rating);

				RatingToAdd.RatingId = rating.RatingId;

				Device.BeginInvokeOnMainThread (async () => await Page.Navigation.PopModalAsync (true));
			} catch (NoInternetException) {
				await Page.DisplayAlert ("No Internet!", "Cannot Access The Internet!", "Darn!");

			} finally {
				AddRatingCommand.ChangeCanExecute ();
			}
		}

		private Command _addRatingCommand;

		public Command AddRatingCommand {
			get {
				return _addRatingCommand ?? (_addRatingCommand = new Command (async () => {
					await ExecuteSaveRatingCommand ();
				}));
			}
		}



	}
}

