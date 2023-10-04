using Core.Entities.User.Models.DTO.Auth.Login;

namespace Core.Entities.User.Models.DTO.Acts.ActEntities
{
    public class DTOActEntityToValid
    {
        public DTOAct? Act { get; set; }
        public int? EntityID { get; set; }
        public int? ParentID { get; set; }
        // public int Priority { get; set; }
        public DTOLogin? Login { get; set; }
    }

}
