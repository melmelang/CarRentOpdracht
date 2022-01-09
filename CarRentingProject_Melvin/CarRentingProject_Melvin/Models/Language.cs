using System.ComponentModel.DataAnnotations;

namespace CarRentingProject_Melvin.Models
{
    public class Language
    {
        [Key]
        [StringLength(2, MinimumLength = 2)]
        public string AppLangId { get; set; }
        public static List<Language> AppSystemLang { get; set; }
        public static List<Language> AppAllLang { get; set; }

        public static string[] AppSuppLang { get; set; }

        public static Dictionary<string, Language> AppLangWiki { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string AppLangName { get; set; }

        public string AppCultures { get; set; }
        public Boolean AppIsSystemLang { get; set; }
    }
}
