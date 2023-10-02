namespace Core.Shared.Models.DB.Kernel.Interfaces;

public interface IBaseADS<T, TStruct>
{
	public T ToModel();
}