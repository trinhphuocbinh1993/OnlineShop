namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Credential")]
    [Serializable] //thuoc tinh nay dung de phan giai du lieu phu hop voi viec tao moi 1 SESSION
    public partial class Credential
    {
        [Key]
        [StringLength(50)]
        public string UserGroupID { get; set; }
        [StringLength(50)]
        public string RoleID { get; set; }

    }
}
