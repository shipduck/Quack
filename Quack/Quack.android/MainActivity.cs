using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Quack.android
{
	[Activity (Label = "Quack", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			base.RequestWindowFeature (WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			Button writeButton = FindViewById<Button> (Resource.Id.write_button);
			writeButton.Click += delegate {
				
			};
		}


	}
}


