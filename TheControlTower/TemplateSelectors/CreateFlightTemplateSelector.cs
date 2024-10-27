using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TheControlTowerBLL.Models;

namespace TheControlTower.TemplateSelectors
{
    class CreateFlightTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FlightCreateTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Flight)
            {
                return FlightCreateTemplate;
            }

            return base.SelectTemplate(item, container);
        }

    }
}
