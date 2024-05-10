using MatrixInverterContract;

namespace MatrixInverterMAUI;

public partial class MatrixInputPage : ContentPage
{
	public static double[][] Matrix = new double[MatrixSizeRegulator.MatrixSize][];
    private int _size;
    public static bool IsReversable(double[][] matrix)
    {
        bool outBool = true;
        var type = MatrixInverter.CurrentAssembly!
        .GetTypes()
        .First(t => t.GetInterfaces()
        .Contains(typeof(IMatrixInverter<double>)));
        var ctor = type.GetConstructors().First();
        var obj = ctor.Invoke(new object[0]);
        var method = type.GetMethod("FindDeterminant");
        var t = (double)method!.Invoke(obj, new object[] { matrix });
        outBool = t != 0;
        return outBool;
    }
    public MatrixInputPage()
	{
		InitializeComponent();
        HelpInitialize();
	}
    public async void HelpInitialize()
    {
        await Task.Run(() =>
        {

            _size = MatrixSizeRegulator.MatrixSize;
            Entry[][] entries = new Entry[_size][];
            for (int i = 0; i < _size; i++)
            {
                Matrix[i] = new double[_size];
                entries[i] = new Entry[_size];
            }
            StackLayout matrixLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Margin = 25,
                Spacing = 25
            };
            var label = new Label { Text = "Введите элементы матрицы:", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center};
            matrixLayout.Children.Add(label);
            for (int i = 0; i < _size; i++)
            {
                StackLayout rowLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 25
                };

                for (int j = 0; j < _size; j++)
                {
                    Entry entry = new Entry
                    {
                        Keyboard = Keyboard.Numeric,
                        Placeholder = "n"
                    };

                    entries[i][j] = entry;
                    rowLayout.Children.Add(entry);
                }

                matrixLayout.Children.Add(rowLayout);
            }

            Button submitButton = new Button
            {
                Text = "Готово",
                BackgroundColor = Colors.Gray,
                TextColor = Colors.White
            };

            submitButton.Clicked += (sender, e) =>
            {
                try
                {
                    for (int i = 0; i < _size; i++)
                    {
                        for (int j = 0; j < _size; j++)
                        {
                            Matrix[i][j] = Int32.Parse(entries[i][j].Text);
                        }
                    }
                }
                catch
                {
                    DisplayAlert("ой-ой", "Убедитесь, что вы ввели все элементы матрицы верно", "OK");
                    return;
                }
                if (!IsReversable(Matrix))
                {
                    DisplayAlert("ой-ой", "Матрица необратима", "OK");
                    return;
                }
                MainThread.BeginInvokeOnMainThread(() => Navigation.PushAsync(new MatrixVisualizator()));
            };

            matrixLayout.Children.Add(submitButton);

            MainThread.BeginInvokeOnMainThread(() => Content = new ScrollView { Content = matrixLayout,
            HorizontalOptions = LayoutOptions.Center});
        });
    }
}