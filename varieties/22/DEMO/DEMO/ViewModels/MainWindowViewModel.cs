using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using System.Linq;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления окна для получения и проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Экземпляр HTTP-клиента для выполнения запросов к API.
    /// </summary>
    private readonly HttpClient httpClientTwentySecond = new();

    /// <summary>
    /// ФИО клиента, отображаемое в интерфейсе.
    /// </summary>
    private string fullNameTwentySecond = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwentySecond;
        set => SetProperty(ref fullNameTwentySecond, value);
    }

    /// <summary>
    /// Текстовый итог текущей валидации.
    /// </summary>
    private string validationResultTwentySecond = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwentySecond;
        set => SetProperty(ref validationResultTwentySecond, value);
    }

    /// <summary>
    /// Выполняет запрос к сервису и обновляет значение ФИО.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwentySecond = await LoadFullNameFromApiTwentySecondAsync();
        FIO = loadedFullNameTwentySecond;
    }

    /// <summary>
    /// Выполняет валидацию текущего ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwentySecond(FIO);
    }

    /// <summary>
    /// Выполняет проверку строки ФИО и возвращает итоговое сообщение.
    /// </summary>
    private string BuildValidationMessageTwentySecond(string fioValue)
    {
        var containsDigitTwentySecond = HasDigitInFullNameTwentySecond(fioValue);
        var containsSpecialCharTwentySecond = HasSpecialSymbolInFullNameTwentySecond(fioValue);

        if (containsDigitTwentySecond || containsSpecialCharTwentySecond)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Определяет, есть ли в ФИО числовые символы.
    /// </summary>
    private bool HasDigitInFullNameTwentySecond(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Определяет, есть ли в ФИО символы из набора !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwentySecond(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Получает ФИО из удаленного эмулятора по заданному URL.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwentySecondAsync()
    {
        var apiResponseTwentySecond = await httpClientTwentySecond.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwentySecond = await apiResponseTwentySecond.Content.ReadFromJsonAsync<Response>();
        return responseModelTwentySecond?.Value ?? string.Empty;
    }
}
