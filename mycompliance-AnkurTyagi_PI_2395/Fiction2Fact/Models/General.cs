using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fiction2Fact.Models
{
    public class General
    {
        [Table("TBL_COMPANY")]
        public class CompanyModel
        {
            public CompanyModel()
            {

            }
            [
                Column("CMP_ID"),
                Key,
                Display(Name = "Company Id")
            ]
            public int CompanyId { get; set; }

            [Column("CMP_NAME")]
            [Required]
            [Display(Name = "Company Namee")]
            [StringLength(255)]
            public string CompanyName { get; set; }

            [Column("CMP_SHORT_NAME")]
            [Required]
            [Display(Name = "Company Short Name")]
            [StringLength(255)]
            public string CompanyShortName { get; set; }

            [Column("CMP_ADDRESS")]
            [Display(Name = "Company Address")]
            [StringLength(255)]
            public string CompanyAddress { get; set; }

            [Column("CMP_LANDMARK")]
            [Display(Name = "Landmark")]
            [StringLength(255)]
            public string CompanyLandmark { get; set; }

            [Column("CMP_CITY")]
            [Display(Name = "City")]
            [StringLength(255)]
            public string CompanyCity { get; set; }

            [Column("CMP_STATE")]
            [Display(Name = "State")]
            [StringLength(255)]
            public string CompanyState { get; set; }

            [Column("CMP_PINCODE")]
            [Display(Name = "Pincode")]
            [StringLength(10)]
            public string CompanyPincode { get; set; }

            [Column("CMP_CONTACT_PERSON")]
            [Display(Name = "Contact Person")]
            [StringLength(255)]
            public string CompanyContactPerson { get; set; }

            [Column("CMP_CONTACT_NO")]
            [Display(Name = "Contact No.")]
            [StringLength(255)]
            public string CompanyContactNo { get; set; }

            [Column("CMP_MOBILE")]
            [Display(Name = "Mobile No.")]
            [StringLength(255)]
            public string CompanyMobile { get; set; }

            [Column("CMP_FAX")]
            [Display(Name = "Company FAX")]
            [StringLength(255)]
            public string CompanyFAX { get; set; }

            [Column("CMP_EMAIL")]
            [Display(Name = "Company Email")]
            [StringLength(255)]
            public string CompanyEmail { get; set; }

            [Column("CMP_LOGO")]
            [Display(Name = "Company Logo")]
            [StringLength(255)]
            public string CompanyLogo { get; set; }

            [Column("CMP_LOGO_MIME")]
            [Display(Name = "Logo MIME")]
            [StringLength(255)]
            public string CompanyLogoMIME { get; set; }

            [Column("CMP_ACTIVE")]
            [Required]
            [Display(Name = "Active")]
            [StringLength(1)]
            public string CMP_ACTIVE { get; set; }

            [Column("CMP_CURRENT")]
            [Required]
            [Display(Name = "Current")]
            [StringLength(1)]
            public string CompanyCurrent { get; set; }

            public ICollection<CompanyProductModel> CompanyProduct { get; set; }
        }

        [Table("tbl_product_mas")]
        public class ProductModel
        {
            public ProductModel()
            {
                this.Pages = new List<PageModel>();
                this.CompanyProduct = new List<CompanyProductModel>();
                this.ProductMenuMapping = new List<ProductMenuMappingModel>();
            }
            [
                Column("PM_ID"),
                Key,
                Display(Name = "Product Id")
            ]
            public int ProductId { get; set; }

            [
                Column("PM_NAME"),
                Required,
                StringLength(255),
                Display(Name = "Product Name")
            ]
            public string ProductName { get; set; }

            [
                Column("Status"),
                Required,
                StringLength(1),
                Display(Name = "Product Status")
            ]
            public string ProductStatus { get; set; }

            public ICollection<PageModel> Pages { get; set; }
            public ICollection<CompanyProductModel> CompanyProduct { get; set; }
            public ICollection<ProductMenuMappingModel> ProductMenuMapping { get; set; }
            //public ICollection<ProductModel> Products { get; set; }
        }

        [Table("tbl_page_mas")]
        public partial class PageModel
        {
            public PageModel()
            {
                this.CompanyProductFeature = new List<CompanyProductFeatureModel>();
            }
            [Column("PGM_ID")]
            [Key]
            [Display(Name = "Page Id")]
            public int PageId { get; set; }

            [Column("PGM_PM_ID")]
            [Required]
            [Display(Name = "Product")]
            public int ProductId { get; set; }

            [ForeignKey("ProductId")]
            public virtual ProductModel Product { get; set; }

            [Column("PGM_NAME")]
            [Required]
            [Display(Name = "Page Name")]
            [StringLength(255)]
            public string PageName { get; set; }

            [Column("PGM_ABSOLUTE_PATH")]
            [Required]
            [Display(Name = "Absolute Path")]
            [StringLength(255)]
            public string PageAbsolutePath { get; set; }

            [Column("PGM_LINK")]
            [Required]
            [Display(Name = "Page Link")]
            [StringLength(255)]
            public string PageLink { get; set; }

            [Column("Status")]
            [Display(Name = "Page Status")]
            [StringLength(1)]
            public string PageStatus { get; set; }

            public ICollection<CompanyProductFeatureModel> CompanyProductFeature { get; set; }
        }

        [Table("tbl_feature_mas")]
        public partial class FeatureModel
        {
            public FeatureModel()
            {
                this.CompanyProductFeature = new List<CompanyProductFeatureModel>();
            }
            [Column("FM_ID")]
            [Key]
            public int FeatureId { get; set; }

            [Column("FM_NAME")]
            [Required]
            [Display(Name = "Feature Name")]
            [StringLength(255)]
            public string FeatureName { get; set; }

            [Column("FM_DESCRIPTION")]
            [Display(Name = "Feature Description")]
            [StringLength(255)]
            public string FeatureDescription { get; set; }

            [Column("STATUS")]
            [Display(Name = "Feature Status")]
            [StringLength(1)]
            public string FeatureStatus { get; set; }

            public ICollection<CompanyProductFeatureModel> CompanyProductFeature { get; set; }
        }

        [Table("tbl_company_products")]
        public partial class CompanyProductModel
        {
            [Column("cp_id")]
            [Key]
            [Display(Name = "Company Product Id")]
            public int CompanyProductId { get; set; }

            [Column("cp_cmp_id")]
            [Required]
            public int CompanyId { get; set; }

            [ForeignKey("CompanyId")]
            public virtual CompanyModel Company { get; set; }

            [Column("cp_pm_id")]
            [Required]
            public int ProductId { get; set; }

            [ForeignKey("ProductId")]
            public virtual ProductModel Product { get; set; }

            public ICollection<CompanyProductFeatureModel> CompanyProductFeature { get; set; }
        }

        [Table("tbl_cp_features")]
        public partial class CompanyProductFeatureModel
        {
            public CompanyProductFeatureModel()
            {

            }
            [Column("CPF_ID")]
            [Key]
            [Display(Name = "Company Product Feature ID")]
            public int CompanyProductFeatureId { get; set; }

            [Column("CPF_FM_ID")]
            [Required]
            [Display(Name = "Feature")]
            public int FeatureId { get; set; }

            [ForeignKey("FeatureId")]
            public virtual FeatureModel Feature { get; set; }

            [Column("CPF_CP_ID")]
            [Required]
            [Display(Name = "Company Product")]
            public int CompanyProductId { get; set; }

            [ForeignKey("CompanyProductId")]
            public virtual CompanyProductModel CompanyProduct { get; set; }

            [Column("CPF_PGM_ID")]
            [Required]
            [Display(Name = "Page")]
            public int PageId { get; set; }

            [ForeignKey("PageId")]
            public virtual PageModel Page { get; set; }

            [Column("CPF_ENABLED")]
            [Required]
            [Display(Name = "Enabled")]
            [StringLength(1)]
            public string CPFeatureEnabled { get; set; }

            [Column("STATUS")]
            [Required]
            [Display(Name = "Feature Status")]
            [StringLength(1)]
            public string CPFeatureStatus { get; set; }

        }

        [Table("TBL_MENU_MAS")]
        public class MenuModel
        {
            public MenuModel()
            {
                this.Menus = new List<MenuModel>();
                this.ProductMenuMapping = new List<ProductMenuMappingModel>();
            }

            [Column("MM_ID")]
            [Key]
            public int MenuId { get; set; }

            [
                Column("MM_MENU_NAME"),
                Display(Name = "Menu Name"),
                StringLength(255)
            ]
            public string MenuName { get; set; }

            [
                Column("MM_MENU_LEVEL"),
                Display(Name = "Menu Level")
            ]
            public int MenuLevel { get; set; }

            [
                Column("MM_PARENT_ID"),
                Display(Name = "Parent Menu")
            ]
            public int? ParentMenuId { get; set; }

            [ForeignKey("ParentMenuId")]
            public virtual MenuModel Parent { get; set; }

            [
                Column("MM_SORT_ORDER"),
                Display(Name = "Sort Order")
            ]
            public int? MenuSortOrder { get; set; }

            [
                Column("MM_HTML"),
                Display(Name = "Menu HTML"),
                StringLength(255)
            ]
            public string MenuHtml { get; set; }

            [
                Column("MM_JAVASCRIPT"),
                Display(Name = "Menu Javascript"),
                StringLength(4000)
            ]
            public string MenuJavascript { get; set; }

            [
                Column("MM_HTML_OUTER"),
                Display(Name = "HTML Outer"),
                StringLength(4000)
            ]
            public string MenuHTMLOuter { get; set; }

            [
                Column("MM_JAVASCRIPT_OUTER"),
                Display(Name = "Javascript Outer"),
                StringLength(4000)
            ]
            public string MenuJavascriptOuter { get; set; }

            [
                Column("MM_JS_MENU_NAME"),
                Display(Name = "Javascript Menu Name"),
                StringLength(4000)
            ]
            public string JavascriptMenuName { get; set; }

            [
                Column("CREATE_BY"),
                Display(Name = "Created By"),
                StringLength(50)
            ]
            public string CreateBy { get; set; }

            [
                Column("CREATE_DT"),
                Display(Name = "Create Date")
            ]
            public DateTime? CreateDate { get; set; }

            [
                Column("UPDATE_BY"),
                Display(Name = "Updated By"),
                StringLength(50)
            ]
            public string UpdateddBy { get; set; }

            [
                Column("UPDATE_DT"),
                Display(Name = "Update Date")
            ]
            public DateTime? UpdateDate { get; set; }

            public ICollection<MenuModel> Menus { get; set; }
            public ICollection<ProductMenuMappingModel> ProductMenuMapping { get; set; }

            public IEnumerable<SelectListItem> FillParentMenuListItems()
            {
                using (F2FDbContext DB = new F2FDbContext())
                {
                    List<SelectListItem> tmpParentMenuListItems = new List<SelectListItem>();
                    //List<MenuModel> prs = DB.Menus.ToList();
                    //var data = prs.GroupBy(p => p.Product);

                    //foreach (var p in data)
                    //{
                    //    SelectListGroup optGroup = new SelectListGroup() { Name = p.Key.ProductName ?? "" };
                    //    foreach (MenuModel m in p)
                    //    {
                    //        tmpParentMenuListItems.Add(new SelectListItem() { Value = m.MenuId.ToString(), Text = m.MenuName, Group = optGroup });
                    //    }
                    //}


                    //List<SelectListItem> tmpParentMenuListItems = DB.Menus
                    //    .Where(s => s.MenuHtml.Equals("#"))
                    //    .Select(m => new SelectListItem { Value = m.MenuId.ToString(), Text = m.MenuName + " " + m.Parent.MenuName })
                    //    .ToList();

                    tmpParentMenuListItems.Insert(0, new SelectListItem { Value = null, Text = "--Select--" });
                    //return tmpParentMenuListItems;
                    return new SelectList(DB.Menus.ToList(), "MenuId", "MenuName", null, null, null);
                }

            }

            //internal ObjectResult<getMenuProductDetails> getMenuProductDetails(string sUserId)
            //{
            //    F2FDbContext DB = new F2FDbContext();
            //    var param = new ObjectParameter[]
            //    {
            //    new ObjectParameter("Username", sUserId),
            //    new ObjectParameter("CompanyId", Convert.ToInt16(HttpContext.Current.Session["CMP_ID"])),
            //    new ObjectParameter("PagePath", HttpContext.Current.Request.RawUrl),

            //    };
            //    return ((IObjectContextAdapter)DB).ObjectContext.ExecuteFunction<getMenuProductDetails>("getMenuProductDetails", param);
            //}
            public IEnumerable<SelectListItem> FillProductListItems()
            {
                using (F2FDbContext DB = new F2FDbContext())
                {
                    SelectListGroup grp = new SelectListGroup();
                    List<SelectListItem> tmpProductListItems = DB.Products
                        .Select(p => new SelectListItem { Value = p.ProductId.ToString(), Text = p.ProductName })
                        .ToList();
                    tmpProductListItems.Insert(0, new SelectListItem { Value = null, Text = "--Select--" });
                    return new SelectList(tmpProductListItems, "Value", "Text");
                }
            }
        }
        public class getMenuProductDetails
        {
            public string sMenuString { get; set; }
            public string sProductName { get; set; }
        }

        [Table("tbl_product_menu_mapping")]
        public partial class ProductMenuMappingModel
        {
            [Column("PMM_ID")]
            [Key]
            [Display(Name = "Product Menu Mapping Id")]
            public int ProductMenuMappingId { get; set; }

            [Column("PMM_PM_ID")]
            [Required]
            [Display(Name = "Product")]
            public int ProductId { get; set; }

            [ForeignKey("ProductId")]
            public virtual ProductModel Product { get; set; }

            [Column("PMM_MM_ID")]
            [Required]
            [Display(Name = "Menu")]
            public int MenuId { get; set; }

            [ForeignKey("MenuId")]
            public virtual MenuModel Menu { get; set; }
        }

        [Table("TBL_F2F_LOG")]
        public partial class F2FLogModel
        {
            [Column("FL_ID")]
            [Key]
            public long F2FLogId { get; set; }

            [Column("FL_TIMESTAMP")]
            public DateTime F2FLogTimestamp { get; set; }

            [Column("FL_FILENAME")]
            [StringLength(255)]
            public string F2FLogFileName { get; set; }

            [Column("FL_FUNCTION")]
            [StringLength(255)]
            public string F2FLogFunction { get; set; }

            [Column("FL_MESSAGE")]
            public string F2FLogMessage { get; set; }

            [Column("FL_INNER_MESSAGE")]
            public string F2FLogInnerMessage { get; set; }

            [Column("FL_STACK_TRACE")]
            public string F2FLogStackTrace { get; set; }

            [Column("FL_ADDITIONAL_INFO")]
            public string F2FLogAdditionalInfo { get; set; }
        }
    }
}