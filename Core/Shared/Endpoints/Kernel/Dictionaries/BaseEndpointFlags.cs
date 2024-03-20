namespace Core.Shared.Endpoints.Kernel.Dictionaries;

[Flags]
public enum BaseEndpointFlags : short
{
	None = 0,
	Create = 1,
	Read = 2,
	Update = 4,
	Delete = 8,
	ToLogs = 16,
}