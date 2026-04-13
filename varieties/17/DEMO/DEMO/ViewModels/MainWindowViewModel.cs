using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DEMO.Models;

namespace DEMO.ViewModels;

/// <summary>
/// Логика взаимодействия интерфейса и правил валидации ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Общий HTTP-клиент для запросов к API.
    /// </summary>
    private static readonly HttpClient sharedHttpClientSeventeenth = new();

    /// <summary>
    /// ФИО клиента, отображаемое в интерфейсе.
    /// </summary>
    private string fullNameSeventeenth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameSeventeenth;
        set => SetProperty(ref fullNameSeventeenth, value);
    }

    /// <summary>
    /// Текстовый итог текущей валидации.
    /// </summary>
    private string validationResultSeventeenth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultSeventeenth;
        set => SetProperty(ref validationResultSeventeenth, value);
    }

    /// <summary>
    /// Выполняет запрос к сервису и обновляет значение ФИО.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameSeventeenth = await LoadFullNameFromApiSeventeenthAsync();
        FIO = loadedFullNameSeventeenth;
    }

    /// <summary>
    /// Выполняет валидацию текущего ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageSeventeenth(FIO);
    }

    /// <summary>
    /// Выполняет проверку строки ФИО и возвращает итоговое сообщение.
    /// </summary>
    private string BuildValidationMessageSeventeenth(string fioValue)
    {
        var containsDigitSeventeenth = HasDigitInFullNameSeventeenth(fioValue);
        var containsSpecialCharSeventeenth = HasSpecialSymbolInFullNameSeventeenth(fioValue);

        if (containsDigitSeventeenth || containsSpecialCharSeventeenth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Определяет, есть ли в ФИО числовые символы.
    /// </summary>
    private bool HasDigitInFullNameSeventeenth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Определяет, есть ли в ФИО символы из набора !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameSeventeenth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Получает ФИО из удаленного эмулятора по заданному URL.
    /// </summary>
    private async Task<string> LoadFullNameFromApiSeventeenthAsync()
    {
        var apiResponseSeventeenth = await sharedHttpClientSeventeenth.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelSeventeenth = await apiResponseSeventeenth.Content.ReadFromJsonAsync<Response>();
        return responseModelSeventeenth?.Value ?? string.Empty;
    }
}
