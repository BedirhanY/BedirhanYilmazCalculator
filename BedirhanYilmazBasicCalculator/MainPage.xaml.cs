using System.Globalization;

namespace BedirhanYilmazBasicCalculator;

public partial class MainPage : ContentPage
{
    double? _first;
    string? _op;

    public MainPage()
    {
        InitializeComponent();
    }

    bool TryReadNumber(out double value)
    {
        value = 0;
        var text = NumberEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(text))
            return false;

        if (double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
            return true;

        var swapped = text.Replace(',', '.');
        return double.TryParse(swapped, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    void SelectOp(string op)
    {
        if (!TryReadNumber(out var n))
        {
            ResultLabel.Text = "Geçerli bir sayı giriniz.";
            return;
        }

        _first = n;
        _op = op;
        NumberEntry.Text = string.Empty;
        ResultLabel.Text = $"İşlem: {_first} {_op} …";
    }

    void OnAddClicked(object sender, EventArgs e)      => SelectOp("+");
    void OnSubtractClicked(object sender, EventArgs e) => SelectOp("-");
    void OnMultiplyClicked(object sender, EventArgs e) => SelectOp("*");
    void OnDivideClicked(object sender, EventArgs e)   => SelectOp("/");

    void OnEqualsClicked(object sender, EventArgs e)
    {
        if (_first is null || string.IsNullOrEmpty(_op))
        {
            ResultLabel.Text = "Önce bir işlem seçiniz (+, -, ×, ÷).";
            return;
        }

        if (!TryReadNumber(out var second))
        {
            ResultLabel.Text = "Geçerli ikinci sayıyı giriniz.";
            return;
        }

        double result;

        switch (_op)
        {
            case "+": result = _first.Value + second; break;
            case "-": result = _first.Value - second; break;
            case "*": result = _first.Value * second; break;
            case "/":
                if (second == 0)
                {
                    ResultLabel.Text = "Sıfıra bölme yapılamaz.";
                    return;
                }
                result = _first.Value / second;
                break;
            default:
                ResultLabel.Text = "Bilinmeyen işlem.";
                return;
        }

        var culture = CultureInfo.CurrentCulture;
        ResultLabel.Text = $"Result: {result.ToString(culture)}";
        NumberEntry.Text = result.ToString(culture);

        _first = null;
        _op = null;
    }
}
