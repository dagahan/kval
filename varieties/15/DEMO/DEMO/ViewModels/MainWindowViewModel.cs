using CommunityToolkit.Mvvm.Input;
using DEMO.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DEMO.ViewModels;

/// <summary>
/// Класс, который управляет получением и проверкой входных данных.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Текст ФИО, который пришел от внешнего источника.
    /// </summary>
    private string fullNameFifteenth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameFifteenth;
        set => SetProperty(ref fullNameFifteenth, value);
    }

    /// <summary>
    /// Итоговое сообщение, показываемое пользователю.
    /// </summary>
    private string validationResultFifteenth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultFifteenth;
        set => SetProperty(ref validationResultFifteenth, value);
    }

    /// <summary>
    /// Запрашивает ФИО и сохраняет его в состоянии экрана.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameFifteenth = await LoadFullNameFromApiFifteenthAsync();
        FIO = loadedFullNameFifteenth;
    }

    /// <summary>
    /// Применяет правила валидации к текущему ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageFifteenth(FIO);
    }

    /// <summary>
    /// Проводит валидацию ФИО по фиксированным условиям.
    /// </summary>
    private string BuildValidationMessageFifteenth(string fioValue)
    {
        var containsDigitFifteenth = HasDigitInFullNameFifteenth(fioValue);
        var containsSpecialCharFifteenth = HasSpecialSymbolInFullNameFifteenth(fioValue);

        if (containsDigitFifteenth || containsSpecialCharFifteenth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Ищет числовые символы в тексте ФИО.
    /// </summary>
    private bool HasDigitInFullNameFifteenth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Ищет специальные символы из фиксированного списка.
    /// </summary>
    private bool HasSpecialSymbolInFullNameFifteenth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Делает вызов API и возвращает полученное ФИО.
    /// </summary>
    private async Task<string> LoadFullNameFromApiFifteenthAsync()
    {
        var requestClient = new HttpClient();
        var apiResponseFifteenth = await requestClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelFifteenth = await apiResponseFifteenth.Content.ReadFromJsonAsync<Response>();
        return responseModelFifteenth?.Value ?? string.Empty;
    }
}
