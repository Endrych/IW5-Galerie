using System.ComponentModel.DataAnnotations;

namespace MyPhotoBL.Enums
{
    public enum CompareRealtion
    {
        [Display(Description="=")]
        Equal,
        [Display(Description="<>")]
        NorEqual,
        [Display(Description="<")]
        Less,
        [Display(Description=">")]
        Greater,
        [Display(Description="<=")]
        LessEqual,
        [Display(Description=">=")]
        GreaterEqual
    }
}