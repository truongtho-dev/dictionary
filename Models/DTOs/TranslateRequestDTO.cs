using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dictionary.Models.DTOs
{
    public class TranslateRequestDTO
    {
        [Required]
        [MinLength(1)]
        public string Keyword { get; set; }

        public ICollection<long> DictionaryIds { get; set; } = new List<long>();
    }
}