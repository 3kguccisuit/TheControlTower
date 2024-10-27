using System.Windows.Controls;

namespace TheControlTower.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
