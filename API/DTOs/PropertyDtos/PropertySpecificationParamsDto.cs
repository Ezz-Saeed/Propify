namespace API.DTOs.PropertyDtos
{
    public class PropertySpecificationParamsDto
    {
        public int? TypeId { get; set; }
        public int? CategoryId { get; set; }
        public string? Sort { get; set; }
        public string? OwnerId { get; set; }
        private string? _search;
        public string? Search
        {
            get => _search;
            set => _search = value?.ToLower();
        }

        public const int MaxPageSize = 30;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
