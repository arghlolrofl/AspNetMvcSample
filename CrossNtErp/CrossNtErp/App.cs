
using CrossNtErp.PageModels;
using Xamarin.Forms;

namespace CrossNtErp {
    public class App : Application {
        public App() {
            //var contactList = FreshMvvm.FreshPageModelResolver.ResolvePageModel<ContactListPageModel>();
            //var navContainer = new FreshMvvm.FreshNavigationContainer(contactList);
            //MainPage = navContainer;

            MainPage = FreshMvvm.FreshPageModelResolver.ResolvePageModel<BrowserPageModel>();
        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }
}
