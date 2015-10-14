using System;
using Xamarin.Forms.Platform.iOS;
using AVFoundation;
using UIKit;
using Foundation;
using CoreGraphics;
using Xamarin.Forms;

[assembly:ExportRenderer(typeof(CheesedStorage.Local.CameraView), typeof(CheesedStorage.Local.iOS.CameraView))]
namespace CheesedStorage.Local.iOS
{
	public class CameraView : PageRenderer
	{
		AVCaptureSession captureSession;
		AVCaptureDeviceInput captureDeviceInput;
		UIView liveCameraStream;
		AVCaptureStillImageOutput stillImageOutput;
		UIButton takePhotoButton;

		CheesedStorage.Local.AudioVideoViewModel viewModel;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			SetupUserInterface ();
			SetupEventHandlers ();

			AuthorizeCameraUse ();
			SetupLiveCameraStream ();

			viewModel = new AudioVideoViewModel ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		public async void AuthorizeCameraUse ()
		{
			var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus (AVMediaType.Video);

			if (authorizationStatus != AVAuthorizationStatus.Authorized) {
				await AVCaptureDevice.RequestAccessForMediaTypeAsync (AVMediaType.Video);
			}
		}

		public void SetupLiveCameraStream ()
		{
			captureSession = new AVCaptureSession ();

			var viewLayer = liveCameraStream.Layer;
			var videoPreviewLayer = new AVCaptureVideoPreviewLayer (captureSession) {
				Frame = liveCameraStream.Bounds
			};
			liveCameraStream.Layer.AddSublayer (videoPreviewLayer);

			var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType (AVMediaType.Video);
			ConfigureCameraForDevice (captureDevice);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice (captureDevice);

			var dictionary = new NSMutableDictionary();
			dictionary[AVVideo.CodecKey] = new NSNumber((int) AVVideoCodec.JPEG);
			stillImageOutput = new AVCaptureStillImageOutput () {
				OutputSettings = new NSDictionary ()
			};

			captureSession.AddOutput (stillImageOutput);
			captureSession.AddInput (captureDeviceInput);
			captureSession.StartRunning ();
		}

		public async void CapturePhoto ()
		{
			var videoConnection = stillImageOutput.ConnectionFromMediaType (AVMediaType.Video);
			var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync (videoConnection);

			var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData (sampleBuffer);

			viewModel.PhotoBytes = jpegImageAsNsData.ToArray ();
			viewModel.SavePhotoToAzure ();

			await App.Current.MainPage.Navigation.PopModalAsync ();		
		}
			
		public void ConfigureCameraForDevice (AVCaptureDevice device)
		{
			var error = new NSError ();
			if (device.IsFocusModeSupported (AVCaptureFocusMode.ContinuousAutoFocus)) {
				device.LockForConfiguration (out error);
				device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
				device.UnlockForConfiguration ();
			} else if (device.IsExposureModeSupported (AVCaptureExposureMode.ContinuousAutoExposure)) {
				device.LockForConfiguration (out error);
				device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
				device.UnlockForConfiguration ();
			} else if (device.IsWhiteBalanceModeSupported (AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance)) {
				device.LockForConfiguration (out error);
				device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
				device.UnlockForConfiguration ();
			}
		}
			
		public AVCaptureDevice GetCameraForOrientation (AVCaptureDevicePosition orientation)
		{
			var devices = AVCaptureDevice.DevicesWithMediaType (AVMediaType.Video);

			foreach (var device in devices) {
				if (device.Position == orientation) {
					return device;
				}
			}

			return null;
		}

		private void SetupUserInterface ()
		{
			var centerButtonX = View.Bounds.GetMidX () - 35f;
			var topLeftX = View.Bounds.X + 25;
			var topRightX = View.Bounds.Right - 65;
			var bottomButtonY = View.Bounds.Bottom - 200;
			var topButtonY = View.Bounds.Top + 15;
			var buttonWidth = 70;
			var buttonHeight = 70;

			liveCameraStream = new UIView () {
				Frame = new CGRect (0f, 0f, View.Bounds.Width, View.Bounds.Height)
			};

			takePhotoButton = new UIButton () {
				Frame = new CGRect (centerButtonX, bottomButtonY, buttonWidth, buttonHeight)
			};
			takePhotoButton.SetBackgroundImage (UIImage.FromFile ("noun_record_174906.png"), UIControlState.Normal);


			View.Add (liveCameraStream);
			View.Add (takePhotoButton);

		}

		private void SetupEventHandlers ()
		{
			takePhotoButton.TouchUpInside += (object sender, EventArgs e) =>  {
				CapturePhoto ();
			};
				
		}
			
	}
}

