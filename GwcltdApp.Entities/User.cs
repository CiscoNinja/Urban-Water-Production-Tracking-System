using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Entities
{
    /// <summary>
    /// GwcltdApp User Account
    /// </summary>
    public class User : IEntityBase
    {
        public User()
        {
            UserRoles = new List<UserRole>();
        }
        public int ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime DateCreated { get; set; }
        public int GwclRegionID { get; set; }
        public int  GwclStationId { get; set; }
        public virtual GwclStation GwclStation { get; set; }
        public virtual GwclRegion GwclRegion { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
