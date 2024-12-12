using StrugariSiminaLab7.Models;
namespace StrugariSiminaLab7;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
       await Navigation.PushAsync(new ProductPage((ShopList)
       this.BindingContext)
        {
            BindingContext = new Product()
        });

    }

  

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }

    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
     
        var selectedProduct = listView.SelectedItem as Product;

        if (selectedProduct != null)
        {
      
            var shopList = (ShopList)BindingContext;
            var listProducts = await App.Database.GetListProductsByShopListIDAsync(shopList.ID);
            var listProductToDelete = listProducts.FirstOrDefault(lp => lp.ProductID == selectedProduct.ID);

            if (listProductToDelete != null)
            {
              
                await App.Database.DeleteListProductAsync(listProductToDelete);

               
                listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
            }
            else
            {
                await DisplayAlert("Error", "Unable to find the selected product in the shopping list.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Please select a product to delete.", "OK");
        }
    }


}