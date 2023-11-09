namespace Core.Shared.Models.DB.Kernel.Interfaces;

public interface IBaseADS<out T>
{
	public T ToModel();
}