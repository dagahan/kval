using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using DEMO.Models;

namespace DEMO.ViewModels;

/// <summary>
/// Основной ViewModel для экрана проверки корректности ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Текст ФИО, который пришел от внешнего источника.
    /// </summary>
    private string fullNameThirtieth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameThirtieth;
        set => SetProperty(ref fullNameThirtieth, value);
    }

    /// <summary>
    /// Итоговое сообщение, показываемое пользователю.
    /// </summary>
    private string validationResultThirtieth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultThirtieth;
        set => SetProperty(ref validationResultThirtieth, value);
    }

    /// <summary>
    /// Запрашивает ФИО и сохраняет его в состоянии экрана.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameThirtieth = await LoadFullNameFromApiThirtiethAsync();
        FIO = loadedFullNameThirtieth;
    }

    /// <summary>
    /// Применяет правила валидации к текущему ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageThirtieth(FIO);
    }

    /// <summary>
    /// Проводит валидацию ФИО по фиксированным условиям.
    /// </summary>
    private string BuildValidationMessageThirtieth(string fioValue)
    {
        var containsDigitThirtieth = HasDigitInFullNameThirtieth(fioValue);
        var containsSpecialCharThirtieth = HasSpecialSymbolInFullNameThirtieth(fioValue);

        if (containsDigitThirtieth || containsSpecialCharThirtieth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Ищет числовые символы в тексте ФИО.
    /// </summary>
    private bool HasDigitInFullNameThirtieth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Ищет специальные символы из фиксированного списка.
    /// </summary>
    private bool HasSpecialSymbolInFullNameThirtieth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Делает вызов API и возвращает полученное ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiThirtiethAsync()
    {
        var requestClient = new HttpClient();
        var apiResponseThirtieth = await requestClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelThirtieth = await apiResponseThirtieth.Content.ReadFromJsonAsync<Response>();
        return responseModelThirtieth?.Value ?? string.Empty;
    }
}
