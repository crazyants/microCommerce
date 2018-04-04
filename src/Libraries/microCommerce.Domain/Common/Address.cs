using System;

namespace microCommerce.Domain.Common
{
    public class Address : BaseEntity
    {
        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the country identifier
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the state/province identifier
        /// </summary>
        public int? StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the state/province name
        /// </summary>
        public string StateProvinceName { get; set; }

        /// <summary>
        /// Gets or sets the district identifier
        /// </summary>
        public int? DistrictId { get; set; }

        /// <summary>
        /// Gets or sets the district name
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// Gets or sets the address line
        /// </summary>
        public string AddressLine { get; set; }

        /// <summary>
        /// Gets or sets the zip/postal code
        /// </summary>
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the company fax number
        /// </summary>
        public string FaxNumber { get; set; }

        /// <summary>
        /// Gets or sets the company tax offixe
        /// </summary>
        public string TaxOffice { get; set; }

        /// <summary>
        /// Gets or sets company tax number
        /// </summary>
        public string TaxNumber { get; set; }

        /// <summary>
        /// Gets or sets the citizenship number
        /// </summary>
        public string CitizenShipNumber { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedDateUtc { get; set; }

        /// <summary>
        /// Gets the full name
        /// </summary>
        [ColumnIgnore]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        /// <summary>
        /// Gets the formatted address
        /// </summary>
        [ColumnIgnore]
        public string FormattedAddress
        {
            get
            {
                return string.Format("{0} {1} {2}/{3}", AddressLine, PhoneNumber, DistrictName, StateProvinceName);
            }
        }
    }
}