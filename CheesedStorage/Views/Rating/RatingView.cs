﻿using System;

using Xamarin.Forms;

namespace CheesedStorage.Local
{
	public class RatingView : ContentView
	{
		Image wedge1;
		Image wedge2;
		Image wedge3;
		Image wedge4;
		Image wedge5;

		const string BLANK_WEDGE = "BlankWedge";
		const string SELECTED_WEDGE = "Wedge";

		public int WedgeRating {
			get {
				return (int)GetValue (WedgeRatingProperty);
			}
			set {
				try {
					SetValue (WedgeRatingProperty, value);
				} catch (ArgumentException ex) {
					// We need to do something here to let the user know
					// the value passed in failed databinding validation
				}					
			}
		}





		public static readonly BindableProperty WedgeRatingProperty = 
			BindableProperty.Create<RatingView, int> (
				rv => rv.WedgeRating,
				1,
				BindingMode.TwoWay,
				(bindable, value) => {
					// Here we would validate the new value against
					// whatever criteria we choose.
					// Note: "bindable" in this case is an object with a "RatingView" type.
					return true;
				},
				(bindable, oldValue, newValue) => {
					// Property has changed

					// We can call something here to update the UI
					var thisView = (RatingView)bindable;

					// Update the UI to display
					// the correct number of highlighted
					//cheeses
					thisView.UpdateRatings ();

				},
				(bindable, oldValue, newValue) => {
					// Property is changing
				},
				(bindable, value) => {
					// Coerce the property
					// Here we would override what the passed in value is
					// And return something else - if we wanted to
					return value;
				}, (bindable) => {					
				// This is the default creator
				return 1;
			});

		public RatingView (bool enabled)
		{
			
			wedge1 = new Image { Source = SELECTED_WEDGE };
			wedge2 = new Image { Source = BLANK_WEDGE };
			wedge3 = new Image { Source = BLANK_WEDGE };
			wedge4 = new Image { Source = BLANK_WEDGE };
			wedge5 = new Image { Source = BLANK_WEDGE };

			if (enabled) {
				var tapRecognizerOne = new TapGestureRecognizer ();
				tapRecognizerOne.NumberOfTapsRequired = 1;
				tapRecognizerOne.Tapped += (sender, e) => {
					WedgeRating = 1;
				};
				wedge1.GestureRecognizers.Add (tapRecognizerOne);

				var tapRecognizerTwo = new TapGestureRecognizer ();
				tapRecognizerTwo.NumberOfTapsRequired = 2;
				tapRecognizerTwo.Tapped += (sender, e) => {
					WedgeRating = 2;
				};
				wedge2.GestureRecognizers.Add (tapRecognizerTwo);

				var tapRecognizerThree = new TapGestureRecognizer ();
				tapRecognizerThree.NumberOfTapsRequired = 3;
				tapRecognizerThree.Tapped += (sender, e) => {
					WedgeRating = 3;
				};
				wedge3.GestureRecognizers.Add (tapRecognizerThree);

				var tapRecognizerFour = new TapGestureRecognizer ();
				tapRecognizerFour.NumberOfTapsRequired = 1;
				tapRecognizerFour.Tapped += (sender, e) => {
					WedgeRating = 4;
				};
				wedge4.GestureRecognizers.Add (tapRecognizerFour);

				var tapRecognizerFive = new TapGestureRecognizer ();
				tapRecognizerFive.NumberOfTapsRequired = 1;
				tapRecognizerFive.Tapped += (sender, e) => {
					WedgeRating = 5;
				};					
				wedge5.GestureRecognizers.Add (tapRecognizerFive);
			}



			Content = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					wedge1, wedge2, wedge3, wedge4, wedge5
				}
			};

		}

		private void UpdateRatings ()
		{			
			Console.WriteLine ("inside update ratings");

			wedge1.Source = SELECTED_WEDGE;
			wedge2.Source = BLANK_WEDGE;
			wedge3.Source = BLANK_WEDGE;
			wedge4.Source = BLANK_WEDGE;
			wedge5.Source = BLANK_WEDGE;

			switch (WedgeRating) {
			case 1:
				wedge1.Source = SELECTED_WEDGE;
				break;
			case 2:
				wedge1.Source = SELECTED_WEDGE;
				wedge2.Source = SELECTED_WEDGE;
				break;
			case 3:
				wedge1.Source = SELECTED_WEDGE;
				wedge2.Source = SELECTED_WEDGE;
				wedge3.Source = SELECTED_WEDGE;
				break;
			case 4:
				wedge1.Source = SELECTED_WEDGE;
				wedge2.Source = SELECTED_WEDGE;
				wedge3.Source = SELECTED_WEDGE;
				wedge4.Source = SELECTED_WEDGE;
				break;
			case 5:
				wedge1.Source = SELECTED_WEDGE;
				wedge2.Source = SELECTED_WEDGE;
				wedge3.Source = SELECTED_WEDGE;
				wedge4.Source = SELECTED_WEDGE;
				wedge5.Source = SELECTED_WEDGE;
				break;

			default: 
				wedge1.Source = SELECTED_WEDGE;
				wedge2.Source = BLANK_WEDGE;
				wedge3.Source = BLANK_WEDGE;
				wedge4.Source = BLANK_WEDGE;
				wedge5.Source = BLANK_WEDGE;
				break;
			}
		}
	}
}


