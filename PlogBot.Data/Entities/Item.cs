using PlogBot.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlogBot.Data.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ItemType ItemType { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
    }
}
