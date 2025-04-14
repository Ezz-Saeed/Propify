namespace API.DTOs
{
    public class PaginatedResultDto<T> where T : class
    {
        public PaginatedResultDto(int pageNumber, int pageSize, int totlaCount, IReadOnlyList<T> data)
        {
            PageNamber = pageNumber;
            PageSize = pageSize;
            TotalCount = totlaCount;
            Data = data;
        }
        public int PageNamber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
