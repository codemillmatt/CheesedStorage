using System;

using Xamarin.Forms;

namespace CheesedStorage.Local
{
	// "Card" cell view via:
	// https://www.syntaxismyui.com/xamarin-forms-in-anger-cards/

	public class CardStatusView : ContentView
	{
		BoxView statusBoxView;


		public CardStatusView ()
		{
			statusBoxView = new BoxView {
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill,
				BackgroundColor = CheeseColors.LIGHT_GREEN
			};
					
			Content = statusBoxView;
		}
	}
}


