namespace Core.Shared.Endpoints.Kernel.Dictionaries;

[Flags]
public enum BaseEndpointFlags : short
{
	Create = 1 << 0,
	Read = 1 << 1,
	Update = 1 << 2,
	Delete = 1 << 3
}