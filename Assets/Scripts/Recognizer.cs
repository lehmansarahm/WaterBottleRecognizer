using UnityEngine;
using Vuforia;

public class Recognizer : MonoBehaviour, ITrackableEventHandler {
	private TrackableBehaviour mTrackableBehaviour;
	private AndroidJavaObject toastDebugger;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour) {
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}

		#if UNITY_ANDROID
		toastDebugger = new AndroidJavaObject("edu.temple.gamemanager.ToastDebugger");
		SetActivityInNativePlugin();
		ShowMessage("Ready to look for water bottles!");
		#else
		Debug.Log("Tracking placeholder for Play Mode (not running on Android device)");
		#endif
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () { /* do nothing */ }

	/// <summary>
	/// Called when the trackable state has changed.
	/// </summary>
	/// <param name="previousStatus">Previous status.</param>
	/// <param name="newStatus">New status.</param>
	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus) {
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			OnTrackingFound();
		}
	}

	/// <summary>
	/// Raises the tracking found event.
	/// </summary>
	private void OnTrackingFound() {
		if (mTrackableBehaviour is ObjectTargetAbstractBehaviour) {
			ObjectTargetAbstractBehaviour otb = mTrackableBehaviour as ObjectTargetAbstractBehaviour;
			ObjectTarget ot = otb.ObjectTarget;

			#if UNITY_ANDROID
			ShowMessage("Water bottle found!");
			#else
			Debug.Log("Tracking placeholder for Play Mode (not running on Android device)");
			#endif
		}
	}

	/// <summary>
	/// Private internal method to initialize the activity property for our debugger toast maker
	/// </summary>
	private void SetActivityInNativePlugin() {
		AndroidJavaClass jclass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = jclass.GetStatic<AndroidJavaObject>("currentActivity");
		toastDebugger.Call("setActivity", activity);
	}

	/// <summary>
	/// Private internal method to display a debugger message to the user via Android toast
	/// </summary>
	/// <param name="message">Message to display on the toast</param>
	private void ShowMessage(string message) {
		toastDebugger.Call("showMessage", message);
	}
}