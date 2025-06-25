using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fiction2Fact.Models.Advertisement
{
    public class AdvertisementMasters
    {
        [Table("TBL_ADVT_CATEGORY_MAS")]
        public class CategoryModel
        {
            [Key]
            [Column("ACM_ID")]
            public int CategoryId { get; set; }
            [
                Column("ACM_CATEGORY_NAME"),
    
                Required,
                StringLength(255),
                Display(Name = "Category Name")
            ]
            public string CategoryName { get; set; }
            [
                Column("ACM_DESCRIPTION"),
                StringLength(255),
                Display(Name = "Category Description")
            ]
            public string CategoryDescription { get; set; }
            [
                Column("ACM_STATUS"),
                StringLength(1),
                Display(Name = "Category Status")
            ]
            public string CategoryStatus { get; set; }
            public int CREATED_BY { get; set; }
            public DateTime CREATED_DT { get; set; }
            public int UPDATED_BY { get; set; }
            public DateTime UPDATE_DT { get; set; }
        }

        [Table("TBL_ADVT_MEDIA_MAS")]
        public class MediaModel
        {
            [Key]
            [Column("AMM_ID")]
            public int MediaId { get; set; }

            [
                Column("AMM_NAME_OF_MEDIA"),
                Required,
                Display(Name = "Media Name"),
                StringLength(255)
            ]
            public string MediaName { get; set; }

            [
                Column("AMM_MEDIA_CODE"),
                StringLength(100),
                Display(Name = "Media Code")
            ]
            public string MediaCode { get; set; }

            [
                Column("AMM_DESCRIPTION"),
                StringLength(255),
                Display(Name = "Media Description")
            ]
            public string AMM_DESCRIPTION { get; set; }

            [
                Column("AMM_STATUS"),
                StringLength(1),
                Display(Name = "Media Status")
            ]
            public string MediaStatus { get; set; }

            public int CREATED_BY { get; set; }
            public DateTime CREATED_DT { get; set; }
            public int UPDATED_BY { get; set; }
            public DateTime UPDATE_DT { get; set; }
        }

        [Table("TBL_ADVT_NATURE_MAS")]
        public class NatureModel
        {
            [
                Column("ANM_ID"),
                Key,
                Display(Name = "Nature Id")
            ]
            public int NatureId { get; set; }

            [
                Column("ANM_NATURE_OF_ADVT"),
    
                Required,
                Display(Name = "Nature Name"),
                StringLength(255)
            ]
            public string NatureName { get; set; }

            [
                Column("ANM_DESCRIPTION"),
                StringLength(255),
                Display(Name = "Nature Description")
            ]
            public string NatureDescription { get; set; }

            [
                Column("ANM_STATUS"),
                Required,
                StringLength(1),
                Display(Name = "Nature Status")
            ]
            public string NatureStatus { get; set; }

            public int CREATED_BY { get; set; }
            public DateTime CREATED_DT { get; set; }
            public int UPDATED_BY { get; set; }
            public DateTime UPDATE_DT { get; set; }
        }

        [Table("TBL_ADVT_TYPE_MAS")]
        public class TypeModel
        {
            [
                Column("ATM_ID"),
                Key,
                Display(Name = "Type Id")
            ]
            public int TypeId { get; set; }

            [
                Column("ATM_TYPE_OF_ADVT"),
                Required,
                Display(Name = "Type Name"),
                StringLength(255)
            ]
            public string TypeName { get; set; }

            [
                Column("ATM_DESCRIPTION"),
                StringLength(255),
                Display(Name = "Type Description")
            ]
            public string TypeDescription { get; set; }

            [
                Column("ATM_STATUS"),
                Required,
                StringLength(1),
                Display(Name = "Type Status")
            ]
            public string TypeStatus { get; set; }

            public int CREATED_BY { get; set; }
            public DateTime CREATED_DT { get; set; }
            public int UPDATED_BY { get; set; }
            public DateTime UPDATE_DT { get; set; }
        }
    }
}
