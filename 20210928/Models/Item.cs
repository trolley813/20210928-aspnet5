using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _20210928.Models
{
    public class Item
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [Display(Name = "Reviews")]
        public List<Review> Reviews { get; set; }

        public override string ToString()
        {
            return $"{Id}";
        }
    }
}
