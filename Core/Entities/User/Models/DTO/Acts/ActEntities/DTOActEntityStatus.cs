namespace Core.Entities.User.Models.DTO.Acts.ActEntities
{
    public class DTOActEntityStatus
    {
        public DTOAct? Act { get; set; }
        public int? EntityID { get; set; }
        public int? ParentID { get; set; }
        public string? SignatureType { get; set; }
        public bool? Visible { get; set; }
    }

}
