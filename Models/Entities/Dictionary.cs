using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Models.Entities
{
    [Table(nameof(Dictionary))]
    public class Dictionary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Index("IX_DictionaryCode", 1, IsUnique = true)]
        public long SourceId { get; set; }

        public virtual Language Source { get; set; }

        [Required]
        [Index("IX_DictionaryCode", 2, IsUnique = true)]
        public long DestinationId { get; set; }

        public virtual Language Destination { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<DictionaryEntry> Entries { get; set; }
    }
}