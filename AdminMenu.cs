using Orchard;
using Orchard.Environment.Extensions;
using Orchard.UI.Navigation;

namespace OrchardHUN.TrainingDemo
{
    /*
     * Yet another provider we have here.
     * Note that we derive from the Component class. This is just for convenience: it already includes a Localizer 
     * reference (and one for Logger, but we won't use that here).
     */
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class AdminMenu : Component, INavigationProvider
    {
        // The task of deciphering what the following line can mean is up to the gentle reader :-).
        public string MenuName { get { return "admin"; } }


        public void GetNavigation(NavigationBuilder builder)
        {
            builder
                // By defining an ImageSet, Orchard will automatically discover a corresponding CSS file, in which you
                // can apply your own properties to the menu items. In this case, we'll use it for adding icons to the
                // top-level menu item. The naming convention is the following: if you name your ImageSet "example", then
                // Orchard will look for a CSS file named menu.example-admin.css in the Styles folder. Let's check out
                // Styles/menu.person-list-dashboard-admin.css!
                .AddImageSet("person-list-dashboard")
                // We commonly use a separate method for actually building the menu: BuildMenu.
                .Add(T("Person List dashboard"), "5", BuildMenu);
        }


        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu
                // This means that the top-level menu item also will point to the action where it's first child item points.
                .LinkToFirstChild(true)

                // This means that the first child menu item will point to our Person List dashboard and be shown only to
                // users having the AccessPersonListDashboard permission.
                // Warning: this doesn't mean others won't be able to access it directly: we have to check in the
                // controller too!
                .Add(subitem => subitem
                    .Caption(T("Create"))
                    .Action("PersonListDashboard", "ContentsAdmin", new { area = "OrchardHUN.TrainingDemo" })
                    .Permission(Permissions.AccessPersonListDashboard)
                )
                .Add(subitem => subitem
                    .Caption(T("View"))
                    .LinkToFirstChild(true)

                    .Add(subsubitem => subsubitem
                        .Caption(T("Most recent one"))
                        .Action("LatestPersonList", "ContentsAdmin", new { area = "OrchardHUN.TrainingDemo" })
                        // This will make the item not appear as a child item in left-side menu, but as a tab on the top.
                        .LocalNav(true)
                        .Permission(Permissions.AccessPersonListDashboard)
                    )

                    .Add(subsubitem => subsubitem
                        .Caption(T("Latest lists"))
                        .Action("LatestPersonLists", "ContentsAdmin", new { area = "OrchardHUN.TrainingDemo" })
                        .LocalNav(true)
                        .Permission(Permissions.AccessPersonListDashboard)
                    )
                );

        }

        // NEXT STATION: Let's head back to Controllers/ContentsAdminController!
    }
}