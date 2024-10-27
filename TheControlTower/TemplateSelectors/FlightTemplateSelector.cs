using System.Windows.Controls;
using System.Windows;
using TheControlTowerBLL.Models;

namespace TheControlTower.TemplateSelectors
{
    public class FlightTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FlightTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Flight)
            {
                return FlightTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
