using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ControllersFilters
{
    public class SelectListItemEqualityComparer : IEqualityComparer<SelectListItem>
    {
        public bool Equals(SelectListItem x, SelectListItem y) =>
            x.Value == y.Value;

        public int GetHashCode(SelectListItem obj) =>
            obj.Value.GetHashCode();
    }
}
