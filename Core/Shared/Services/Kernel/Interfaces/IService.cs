namespace Core.Shared.Services;

public interface IService<T, TDTO>
{
	public Task<TDTO> Add(T model);
}