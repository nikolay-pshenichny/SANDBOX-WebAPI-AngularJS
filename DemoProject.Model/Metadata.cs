using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProject.Model
{
    using DemoProject.Model.Enums;

    public class Metadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(256)]
        public string FileName { get; set; }

        public DateTime At { get; set; }

        public string ProcessingResult { get; set; }

        public Guid FileStorageUid { get; set; }

        public ChecksumType ChecksumType { get; set; }

        [MaxLength(256)]
        public string Checksum { get; set; }
    }
}
