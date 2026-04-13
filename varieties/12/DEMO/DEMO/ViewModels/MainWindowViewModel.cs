using System.Net.Http;
using System.Linq;
using DEMO.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace DEMO.ViewModels;

/// <summary>
/// Модель представления окна для получения и проверки ФИО.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// ФИО клиента, отображаемое в интерфейсе.
    /// </summary>
    private string fullNameTwelfth = string.Empty;

    /// <summary>
    /// Текущее ФИО на форме.
    /// </summary>
    public string FIO
    {
        get => fullNameTwelfth;
        set => SetProperty(ref fullNameTwelfth, value);
    }

    /// <summary>
    /// Текстовый итог текущей валидации.
    /// </summary>
    private string validationResultTwelfth = string.Empty;

    /// <summary>
    /// Результат проверки ФИО на форме.
    /// </summary>
    public string Result
    {
        get => validationResultTwelfth;
        set => SetProperty(ref validationResultTwelfth, value);
    }

    /// <summary>
    /// Выполняет запрос к сервису и обновляет значение ФИО.
    /// </summary>
    [RelayCommand]
    public async Task GetFio()
    {
        var loadedFullNameTwelfth = await LoadFullNameFromApiTwelfthAsync();
        FIO = loadedFullNameTwelfth;
    }

    /// <summary>
    /// Выполняет валидацию текущего ФИО.
    /// </summary>
    [RelayCommand]
    public void SendTestResult()
    {
        Result = BuildValidationMessageTwelfth(FIO);
    }

    /// <summary>
    /// Выполняет проверку строки ФИО и возвращает итоговое сообщение.
    /// </summary>
    private string BuildValidationMessageTwelfth(string fioValue)
    {
        var containsDigitTwelfth = HasDigitInFullNameTwelfth(fioValue);
        var containsSpecialCharTwelfth = HasSpecialSymbolInFullNameTwelfth(fioValue);

        if (containsDigitTwelfth || containsSpecialCharTwelfth)
        {
            return "ФИО содержит запрещённые символы";
        }

        return "ФИО валидно";
    }

    /// <summary>
    /// Определяет, есть ли в ФИО числовые символы.
    /// </summary>
    private bool HasDigitInFullNameTwelfth(string fioValue)
    {
        return fioValue.Any(char.IsDigit);
    }

    /// <summary>
    /// Определяет, есть ли в ФИО символы из набора !@#$%^&*.
    /// </summary>
    private bool HasSpecialSymbolInFullNameTwelfth(string fioValue)
    {
        return fioValue.Any(character => "!@#$%^&*".Contains(character));
    }

    /// <summary>
    /// Получает ФИО из удаленного эмулятора по заданному URL.
    /// </summary>
    private async Task<string> LoadFullNameFromApiTwelfthAsync()
    {
        var requestClient = new HttpClient();
        var apiResponseTwelfth = await requestClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var responseModelTwelfth = await apiResponseTwelfth.Content.ReadFromJsonAsync<Response>();
        return responseModelTwelfth?.Value ?? string.Empty;
    }
}
