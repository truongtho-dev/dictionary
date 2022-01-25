using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DictionaryEntity = Dictionary.Models.Entities.Dictionary;

namespace Dictionary.Models.Entities
{
    [Table(nameof(Language))]
    public class Language
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Index("IX_Code", IsUnique = true)]
        [MaxLength(100)]
        public string Code { get; set; }

        public string Name { get; set; }

        public virtual ICollection<DictionaryEntity>  SourceDictionaries { get; set; }

        public virtual ICollection<DictionaryEntity>  DestinationDictionaries { get; set; }
    }
}