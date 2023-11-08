namespace Core.Shared.Models.DB.Kernel.Interfaces;

public interface IBaseKPI<TValue>
{
	public TValue GetValue();
	public string[] GetKPICRID();
	public Func<List<TValue>, string[]> GetComputedValues();
}