namespace Core.Shared.Models.DB.Kernel.Interfaces;

public interface IBaseKPI<TValue>
{
	public static abstract string[] GetKPICRID();
	public TValue GetValue();
	public Func<List<TValue>, string[]> GetComputedValues();
}