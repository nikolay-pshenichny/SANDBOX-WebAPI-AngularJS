using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoProject.Model.Enums;

namespace DemoProject.Model
{
    [Table("Metadata")]
    public class Metadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(256)]
        public string FileName { get; set; }

        public DateTime AtUtc { get; set; }

        public string ProcessingResult { get; set; }

        public Guid FileStorageUid { get; set; }

        public ChecksumType ChecksumType { get; set; }

        [MaxLength(256)]
        public string Checksum { get; set; }
    }
}
