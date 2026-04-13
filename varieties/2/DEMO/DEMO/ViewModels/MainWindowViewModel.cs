using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using DEMO.Models;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления окна для получения и проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Общий HTTP-клиент для запросов к API.
    /// </summary>
    private static readonly HttpClient sharedHttpClientSecond = new();

    /// <summary>
    /// ФИО клиента, отображаемое в интерфейсе.
    /// </summary>
    private string fullNameSecond = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameSecond;
        set => SetProperty(ref fullNameSecond, value);
    }

    /// <summary>
    /// Текстовый итог текущей валидации.
    /// </summary>
    private string validationResultSecond = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultSecond;
        set => SetProperty(ref validationResultSecond, value);
    }

    /// <summary>
    /// Выполняет запрос к сервису и обновляет значение ФИО.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameSecond = await LoadFullNameFromApiSecondAsync();
        FIO = loadedFullNameSecond;
    }

    /// <summary>
    /// Выполняет валидацию текущего ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageSecond(FIO);
    }

    /// <summary>
    /// Выполняет проверку строки ФИО и возвращает итоговое сообщение.
    /// </summary>
    private string BuildValidationMessageSecond(string fioValue)
    {
        var containsDigitSecond = HasDigitInFullNameSecond(fioValue);
        var containsSpecialCharSecond = HasSpecialSymbolInFullNameSecond(fioValue);

        if (containsDigitSecond || containsSpecialCharSecond)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Определяет, есть ли в ФИО числовые символы.
    /// </summary>
    private bool HasDigitInFullNameSecond(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Определяет, есть ли в ФИО символы из набора !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameSecond(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Получает ФИО из удаленного эмулятора по заданному URL.
    /// </summary>
    private async Task<string> LoadFullNameFromApiSecondAsync()
    {
        var apiResponseSecond = await sharedHttpClientSecond.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelSecond = await apiResponseSecond.Content.ReadFromJsonAsync<Response>();
        return responseModelSecond?.Value ?? string.Empty;
    }
}
