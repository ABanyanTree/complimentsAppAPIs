namespace LikeKero.Contract.Responses.SystemRole
{
    public class AssignmentSearchResponse :BaseResponse
    {
        public string SearchonGroupId { get; set; }
        public string Group1Name { get; set; }
        public string Group2Name { get; set; }
        public string Group3Name { get; set; }
        public string Group4Name { get; set; }
        public string Group5Name { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public int TotalAdmin { get; set; }
        public bool Status { get; set; }
    }
}
