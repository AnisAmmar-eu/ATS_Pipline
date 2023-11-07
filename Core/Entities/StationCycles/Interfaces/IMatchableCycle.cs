using Core.Entities.StationCycles.Dictionaries;

namespace Core.Entities.StationCycles.Interfaces;

public interface IMatchableCycle
{
	public SignMatchStatus MatchingCamera1 { get; set; }
	public SignMatchStatus MatchingCamera2 { get; set; }
}