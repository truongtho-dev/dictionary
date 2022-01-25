using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Models.Entities
{
    [Table(nameof(DictionaryEntry))]
    public class DictionaryEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long DictionaryId { get; set; }

        public virtual Dictionary Dictionary { get; set; }

        [Index("IX_Word", IsUnique = false)]
        [MaxLength(100)]
        public string Word { get; set; }

        public string Meaning { get; set; }
    }
}