using Core.Shared.Models.DB.Multilanguage;

namespace Core.Shared.Models.DB.Kernel
{
    public class BaseLang<T, TLang> : BaseEntity
        where T : BaseEntity
        where TLang : BaseLang<T, TLang>
    {
        public int LanguageID { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }

        private Language? _language;
        public Language Language
        {
            set => _language = value;
            get => _language
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Language));
        }

        public int EntityID { get; set; }

        // Nav properties
        private T? _entity;
        public T Entity
        {
            set => _entity = value;
            get => _entity
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(T));
        }

        public BaseLang()
        {
        }
        public BaseLang(int id, Language language, string name, string? description = null, int? lastActID = null)
        {
            EntityID = id;
            Language = language;
            Name = name;
            Description = description;
        }
    }
}
