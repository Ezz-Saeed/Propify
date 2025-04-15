namespace API.DTOs
{
    public class PaginatedResultDto<T> where T : class
    {
        public PaginatedResultDto(int pageNumber, int pageSize, int totlaCount, IReadOnlyList<T> data)
        {
            Data = data;
            Pagination = new Pagination(pageNumber, pageSize, totlaCount);
        }
        public Pagination Pagination { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }

    public class Pagination
    {
        public Pagination(int pageNumber, int pageSize, int totlaCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totlaCount;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
