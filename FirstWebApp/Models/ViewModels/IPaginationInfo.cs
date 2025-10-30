namespace FirstWebApp.Models.ViewModels
{
    public interface IPaginationInfo
    {
        int CurrentPage { get; set; }
        int TotalResult { get; set; }
        int ResultsPerPage { get; set; }
        string Search {  get; set; }
        string OrderBy { get; set; }
        bool Ascending { get; set; }
    }
}
