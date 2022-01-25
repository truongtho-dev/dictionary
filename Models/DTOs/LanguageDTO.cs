using System.Collections.Generic;

namespace Dictionary.Models.DTOs
{
    public class LanguageCollectionDTO
    {
        public virtual ICollection<LanguageDTO> Languages { get; set; }
    }

    public class LanguageDTO
    {
        public long Id { get; set; }
        
        public string Code { get; set; }

        public string Name { get; set; }

        public ICollection<DictionaryDTO> SourceDictionaries { get; set; }
    }
}