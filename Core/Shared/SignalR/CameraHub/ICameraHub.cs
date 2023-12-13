namespace Core.Shared.SignalR.CameraHub;

public interface ICameraHub
{
	/// <summary>
	/// Refreshes test images.
	/// </summary>
	Task RefreshTestImages();
}