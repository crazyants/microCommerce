namespace microCommerce.Domain.Common
{
    public class SearchTerm : BaseEntity
    {
        /// <summary>
        /// Gets or sets the keyword
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets search count
        /// </summary>
        public int Count { get; set; }
    }
}