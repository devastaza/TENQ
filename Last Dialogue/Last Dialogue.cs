using Xamarin.Forms;

namespace CSharp_Shell
{
    public class Program 
    {
        public static void Main()
        {
        	Ui.RunOnUiThread(()=>
        	{
        		Ui.LoadApplication(new App());
        	});
        }
    }
}