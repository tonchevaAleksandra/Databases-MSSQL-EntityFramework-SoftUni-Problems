
using System.ComponentModel.DataAnnotations;

namespace RealEstates.Models
{
    public class RealEstatePropertyTag
    {
     
        public int RealEstatePropertyId { get; set; }
        public virtual RealEstateProperty RealEstateProperty { get; set; }
     
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
