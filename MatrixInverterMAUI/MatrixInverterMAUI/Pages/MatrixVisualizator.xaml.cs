using MatrixInverterContract;

namespace MatrixInverterMAUI;

public partial class MatrixVisualizator : ContentPage
{
    private Semaphore sema = new Semaphore(0, 1);
    private int _size;
    private double[][] _matrix = (double[][])MatrixInputPage.Matrix.Clone();
    public MatrixVisualizator()
    {
        InitializeComponent();
        HelpInitialize();
        BackgroundColor = Color.FromArgb("#0A1F32");
    }
    public static async Task<double[][]> ReflectionMethodCall(double[][] matrix)
    {
        double[][] doubles = new double[matrix.Length][];
        await Task.Run(() =>
        {
            var type = MatrixInverter.CurrentAssembly!
            .GetTypes()
            .First(t => t.GetInterfaces()
            .Contains(typeof(IMatrixInverter<double>)));
            var ctor = type.GetConstructors().First();
            var obj = ctor.Invoke(new object[0]);
            var method = type.GetMethod("InvertMatrix");
            var t = (double[][])method!.Invoke(obj, new object[] { matrix });
            doubles = t;
        });
        return doubles;
    }
    public async void HelpInitialize()
    {
        await Task.Run(() =>
        {

            _size = MatrixSizeRegulator.MatrixSize;
            Label[][] labels = new Label[_size][];
            for (int i = 0; i < _size; i++)
            {
                MatrixInputPage.Matrix[i] = new double[_size];
                labels[i] = new Label[_size];
            }
            StackLayout matrixLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 25,
                BackgroundColor = Color.FromArgb("#0A1F32")
            };
            for (int i = 0; i < _size; i++)
            {
                StackLayout rowLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 25,
                };

                for (int j = 0; j < _size; j++)
                {
                    Label label = new Label
                    {
                        Text = _matrix[i][j].ToString(),
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = 25,
                        TextColor = Colors.White,
                    };
                    labels[i][j] = label;
                    rowLayout.Children.Add(label);
                }

                matrixLayout.Children.Add(rowLayout);
            }

            Button submitButton = new Button
            {
                Text = "Начать визуализацию",
                TextColor = Colors.White,
                BackgroundColor = Color.FromArgb("#cf6679"),
            };

            double[][] outMatrix = ReflectionMethodCall(_matrix).Result;
            submitButton.Clicked += async (sender, e) =>
            {
                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        MainThread.BeginInvokeOnMainThread(() => labels[i][j].Text = outMatrix[i][j].ToString());
                        await Task.Delay(500);
                    }
                }

            };
            Button changeThemeButton = new Button
            {
                Text = "Смена темы :)",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                TextColor = Colors.White,
                BackgroundColor = Color.FromArgb("#ff74d9"),
            };
            changeThemeButton.Clicked += async (sender, e) =>
            {
                await Task.Run(() =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (matrixLayout.BackgroundColor.Equals(Color.FromArgb("#0A1F32")))
                        {
                            BackgroundColor = Colors.White;
                            matrixLayout.BackgroundColor = Colors.White;
                            foreach (var item in labels)
                            {
                                foreach (var label in item)
                                {
                                    label.TextColor = Colors.Black;
                                }
                            }
                            submitButton.BackgroundColor = Color.FromArgb("#e054b8");
                            changeThemeButton.BackgroundColor = Color.FromArgb("#309054");

                        }
                        else
                        {
                            BackgroundColor = Color.FromArgb("#0A1F32");
                            matrixLayout.BackgroundColor = Color.FromArgb("#0A1F32");
                            foreach (var item in labels)
                            {
                                foreach (var label in item)
                                {
                                    label.TextColor = Colors.White;
                                }
                            }
                            submitButton.BackgroundColor = Color.FromArgb("#cf6679");

                            changeThemeButton.BackgroundColor = Color.FromArgb("#ff74d9");
                        }
                    });
                });
            };
            matrixLayout.Children.Add(submitButton);
            matrixLayout.Children.Add(changeThemeButton);
            matrixLayout.Children.Add(new Image
            {
                Source = "beb.gif",
                WidthRequest = 200,
                HeightRequest = 200,
            });
            MainThread.BeginInvokeOnMainThread(() => Content = new ScrollView
            {
                Content = matrixLayout,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.FromArgb("#0A1F32")
            });
        });
    }
}
   