using Core.Shared.Models.DTOs.Kernel;

namespace Core.Shared.Models.DB.Kernel
{
    public class BaseChoice<T, TLang> : BaseEntity
        where T : BaseEntity
        where TLang : BaseLang<T, TLang>
    {
        public string RID { get; set; } = "";

        public ICollection<TLang> C_Langs { get; set; } = new List<TLang>();

        public override DTOBaseChoice ToDTO(string? languageRID = null)
        {
            TLang? c_lang = C_Langs.SingleOrDefault(choiceLang => choiceLang.Language.RID == languageRID);
            return new DTOBaseChoice
            {
                ID = ID,
                RID = RID,
                Name = c_lang?.Name ?? "",
                Description = c_lang?.Description ?? "",
                TS = TS
            };
        }
    }
}
