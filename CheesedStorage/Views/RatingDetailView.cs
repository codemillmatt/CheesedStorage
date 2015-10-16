using System;

using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;

namespace CheesedStorage.Local
{
	// Overall UI design look & feel inspired via
	// https://www.syntaxismyui.com/xamarin-forms-in-anger-phoenix-peaks/

	public class RatingDetailView : ContentPage
	{
		RatingDetailViewModel _viewModel;
		Image _backgroundImage;

		public RatingDetailView (CheeseAndRating theRating)
		{
			_viewModel = new RatingDetailViewModel (theRating, this);
			BindingContext = _viewModel;

			this.SetBinding (ContentPage.TitleProperty, "Title");

			AbsoluteLayout cheeseInfoLayout = new AbsoluteLayout {
				HeightRequest = 250,
				BackgroundColor = CheeseColors.PURPLE
			};

			var cheeseName = new Label {				
				FontSize = 30,
				FontFamily = "AvenirNext-DemiBold",
				TextColor = Color.White
			};
			cheeseName.SetBinding (Label.TextProperty, "CheeseName");

			var dairyName = new Label { 				
				TextColor = Color.FromHex ("#ddd"),
				FontFamily = "AvenirNextCondensed-Medium" 
			};
			dairyName.SetBinding (Label.TextProperty, "DairyName");

			UriImageSource uriImgSource = new UriImageSource ();
			uriImgSource.SetBinding (UriImageSource.UriProperty, "CheesePhotoUri");

			_backgroundImage = new Image () {
				Source = uriImgSource,
				Aspect = Aspect.AspectFill,
			};
				 
			var overlay = new BoxView () {
				Color = Color.Black.MultiplyAlpha (.7f)
			};
					
			var notesLabel = new Label () {
				FontSize = 14,
				TextColor = Color.FromHex ("#ddd")
			};

			notesLabel.SetBinding (Label.TextProperty, "RatingDescription");

			var description = new Frame () {
				Padding = new Thickness (10, 5),
				HasShadow = false,
				BackgroundColor = Color.Transparent,
				Content = notesLabel
			};
					
			AbsoluteLayout.SetLayoutFlags (overlay, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (overlay, new Rectangle (0, 1, 1, 0.3));

			AbsoluteLayout.SetLayoutFlags (_backgroundImage, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds (_backgroundImage, new Rectangle (0f, 0f, 1f, 1f));

			AbsoluteLayout.SetLayoutFlags (cheeseName, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (cheeseName, 
				new Rectangle (0.1, 0.85, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)
			);

			AbsoluteLayout.SetLayoutFlags (dairyName, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (dairyName, 
				new Rectangle (0.1, 0.95, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)
			);
				
			cheeseInfoLayout.Children.Add (_backgroundImage);
			cheeseInfoLayout.Children.Add (overlay);
			cheeseInfoLayout.Children.Add (cheeseName);
			cheeseInfoLayout.Children.Add (dairyName);

			Content = new StackLayout () {
				BackgroundColor = Color.FromHex ("#333"),
				Children = {
					cheeseInfoLayout, description
				}
			};					
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			// Since when this page disappears it will always be reloaded (always being popped off the nav stack), 
			// let's dispose of the view model
			_viewModel.Dispose ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_viewModel.GetRatingDetailsCommand.Execute (null);
		}
	}
}


