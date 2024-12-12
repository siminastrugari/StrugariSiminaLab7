using Microsoft.Maui.Devices.Sensors;
using Plugin.LocalNotification;
using StrugariSiminaLab7.Models;
namespace StrugariSiminaLab7;

public partial class ShopPage : ContentPage
{
	public ShopPage()
	{
		InitializeComponent();
	}
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }
    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;
        var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat" };
        var shoplocation = locations?.FirstOrDefault();
        /* var shoplocation= new Location(46.7492379, 23.5745597);//pentru
        Windows Machine */

        var myLocation = await Geolocation.GetLocationAsync();
        /* var myLocation = new Location(46.7731796289, 23.6213886738);
       //pentru Windows Machine */
        var distance = myLocation.CalculateDistance(shoplocation,
       DistanceUnits.Kilometers);
        if (distance < 5)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de facut cumparaturi in apropiere!",
                Description = address,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }


        await Map.OpenAsync(shoplocation, options);
        }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

        // Confirm the deletion with the user
        var confirm = await DisplayAlert("Delete Shop",
            $"Are you sure you want to delete the shop '{shop.ShopName}'?",
            "Yes",
            "No");

        if (confirm)
        {
            try
            {
                // Delete the shop from the database
                await App.Database.DeleteShopAsync(shop);

                // Navigate back after successful deletion
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Handle any exceptions and show an error message
                await DisplayAlert("Error", $"Failed to delete the shop: {ex.Message}", "OK");
            }
        }
    }
}