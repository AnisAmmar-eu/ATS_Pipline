using Core.Shared.Models.DTO.Kernel;

namespace Core.Shared.Models.DB.Kernel;

public class BaseChoice<T, TLang> : BaseEntity
	where T : BaseEntity
	where TLang : BaseLang<T, TLang>
{
	public string RID { get; set; } = "";

	public ICollection<TLang> CLangs { get; set; } = new List<TLang>();

	public override DTOBaseChoice ToDTO(string? languageRID = null)
	{
		TLang? cLang = CLangs.SingleOrDefault(choiceLang => choiceLang.Language.RID == languageRID);
		return new DTOBaseChoice
		{
			ID = ID,
			RID = RID,
			Name = cLang?.Name ?? "",
			Description = cLang?.Description ?? "",
			TS = TS
		};
	}
}