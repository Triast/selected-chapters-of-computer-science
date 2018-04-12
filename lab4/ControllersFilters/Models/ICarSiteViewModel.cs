using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ControllersFilters.Models
{
    public interface ICarSiteViewModel<TEntity>
    {
        TEntity Entity { get; }
        IEnumerable<TEntity> Collection { get; set; }
        IEnumerable<SelectListItem> SelectListItems { get; set; }
    }
}
