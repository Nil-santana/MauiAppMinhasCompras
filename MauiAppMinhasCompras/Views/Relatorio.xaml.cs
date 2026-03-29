using MauiAppMinhasCompras.Helpers;
namespace MauiAppMinhasCompras.Views;
public partial class Relatorio : ContentPage
{
    public Relatorio() => InitializeComponent();
    
    protected async override void OnAppearing()
    {
        try
        {
            
            List<RelatorioItem> itens =
                await App.Db.GetTotalPorCategoria();
            
            lst_relatorio.ItemsSource = itens;
           
            double totalGeral = itens.Sum(i => i.Total);
            lbl_total_geral.Text = $"Total Geral: {totalGeral:C}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}