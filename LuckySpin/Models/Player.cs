using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LuckySpin.Models
{
    public class Player
    {
        public long Id { get; set; }

        [Required(ErrorMessage ="Please enter your Name")]
        public string FirstName { get; set; }

        [Range(1,9, ErrorMessage = "Choose a number")]
        public int Luck { get; set; }

        [Range(1,100, ErrorMessage ="Enter a balance between 1 and 100")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        //Each Player has a set of Spins, add these as an ICollection
        //(To refresh your DB, delete the LuckySpin database after you make this change.)
        public ICollection<Spin> Spins { get; set; }
    }
}