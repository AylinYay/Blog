#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class TagModel : RecordBase
    {
        [Required]
        [StringLength(75)]
        public string Name { get; set; }

        public bool IsPopular { get; set; }
    }
}
