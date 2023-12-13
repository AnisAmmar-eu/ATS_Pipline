namespace Core.Shared.SignalR.CameraHub;

public interface ICameraHub : IBaseHub
{
	/// <summary>
	/// Refreshes test images.
	/// </summary>
	Task RefreshTestImages();
}