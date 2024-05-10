using MatrixInverterContract;
using System.Reflection;

namespace MatrixInverterMAUI;

public partial class MatrixInverter : ContentPage
{
    private string? _path;
    public static Assembly? CurrentAssembly;
    private bool _hasRealization;
    public MatrixInverter()
    {
        InitializeComponent();
    }

    private void CheckerBtn_OnClicked(object? sender, EventArgs e)
    {
        try
        {
            _path = Path.Text!;
            CurrentAssembly = Assembly.LoadFrom(_path);
            if (CurrentAssembly.GetTypes()
                .Any(type => type
                .GetInterfaces()
                .Contains(typeof(IMatrixInverter<double>))))
            {
                _hasRealization = true;
            }
        }
        catch
        {
            Guide.Text = "Введен неправильный путь или произошла ошибка";
        }
        if (_hasRealization)
        {
            Guide.Text = "Beb";
            Navigation.PushAsync(new NavigationPage(new MatrixSizeRegulator()));
        }
        else
        {
            DisplayAlert("ой-ой", "В dll-файле нет реализации контракта", "OK");
        }
    }


}