using System.Collections.Generic;

namespace Dictionary.Models.DTOs
{
    public class TranslateResultDTO
    {
        public string Keyword { get; set; }

        public ICollection<DictionaryQueryResultDTO> Results { get; set; }
    }

    public class DictionaryQueryResultDTO
    {
        public long DictionaryId { get; set; }

        public string DictionaryName { get; set; }

        public ICollection<DictionaryEntryDTO> Results { get; set; }
    }

    public class DictionaryEntryDTO
    {
        public long Id { get; set; }

        public string Word { get; set; }

        public int WordDifference { get; set; }

        public string Meaning { get; set; }
    }
}