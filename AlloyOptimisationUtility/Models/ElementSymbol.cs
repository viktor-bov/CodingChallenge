using System.ComponentModel.DataAnnotations;

namespace AlloyOptimisationUtility.Models
{
    /// <summary>
    /// Chemical element symbols used in alloy compositions. The underlying integer value is
    /// the element's atomic number in the periodic table (e.g. <see cref="Co"/> = 27).
    /// </summary>
    public enum ElementSymbol
    {
        [Display(Name = "Vanadium")]
        V = 23,
        [Display(Name = "Chromium")]
        Cr = 24,
        [Display(Name = "Manganese")]
        Mn = 25,
        [Display(Name = "Iron")]
        Fe = 26,
        [Display(Name = "Cobalt")]
        Co = 27,
        [Display(Name = "Nickel")]
        Ni = 28,
        [Display(Name = "Niobium")]
        Nb = 41,
        [Display(Name = "Molybdenum")]
        Mo = 42,
    }
}
