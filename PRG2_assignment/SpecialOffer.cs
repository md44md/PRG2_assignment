using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_assignment
{
    class SpecialOffer
    {
        public string OfferCode { get; set; }
        public string OfferDesc { get; set; }
        public double Discount { get; set; }

        public SpecialOffer() 
        {
            OfferCode = "";
            OfferDesc = "";
            Discount = 0.0;
        }

        public SpecialOffer(string offerCode, string offerDesc, double discount)
        {
            OfferCode = offerCode;
            OfferDesc = offerDesc;
            Discount = discount;
        }

        public override string ToString()
        {
            return $"Offer code: {OfferCode} Offer description: {OfferDesc} Discount: {Discount}%";
        }
    }
}
