using MatrixInverterContract;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MatrixInverterMAUI;

public partial class MatrixSizeRegulator : ContentPage
{
	public static int MatrixSize;
	public MatrixSizeRegulator()
	{
		InitializeComponent();
	}

    private async void RefBtn_Clicked(object sender, EventArgs e)
    {
		try
		{
			MatrixSize = Int32.Parse(Size.Text);
		}
		catch
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
                DisplayAlert("ой-ой", "Вероятно, вы ввели не число", "OK");
            });
		}

		if (MatrixSize <= 0)
		{
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Guide.Text = "Введена неверная размерность матрицы (>= 1)";
            });
        }
        else
		{
            await Task.Run(() =>
            {
                var page = new NavigationPage(new MatrixInputPage());
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(page);
                });
            });
        }
    }
}

//D:\hw\MatrixInverterMAUI\MatrixInverter.dll