using StocksApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.ServiceContracts.DTOs
{
    public class SellOrderRequest : IValidatableObject
    {
        [Required(ErrorMessage = "StockSymbol is Required")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "StockName is Required")]
        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity should be between 1 and 100000")]
        public UInt32 Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Price should be between 1 and 10000")]
        public double Price { get; set; }

        public SellOrder ToSellOrder()
        {
            return new SellOrder
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                Price = Price,
                Quantity = Quantity,
                DateAndTimeOfOrder = DateAndTimeOfOrder
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
            {
                result.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000.\""));
            }

            // 2. When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
            // 3. When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
            if (Quantity <= 0 || Quantity > 100000)
            {
                result.Add(new ValidationResult("The Quantity should be between 1 and 100000"));
            }

            // 4. When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
            // 5. When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
            if (Price <= 0 || Price >10000)
            {
                result.Add(new ValidationResult("The Price should be between 1 and 10000"));
            }

            return result;
        }
    }
}
