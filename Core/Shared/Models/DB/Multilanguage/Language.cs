using Core.Shared.Models.DB.Kernel;

namespace Core.Shared.Models.DB.Multilanguage;

public class Language : BaseEntity
{
    public string RID { get; set; } = "";
    public string Name { get; set; } = "";
}