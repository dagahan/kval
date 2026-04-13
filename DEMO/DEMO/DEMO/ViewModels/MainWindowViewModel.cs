using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DEMO.Models;

namespace DEMO.ViewModels;

/// Сделать запрос
/// Провалидировать ответ
/// Записать валидацию в тесткейс
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Св-во для хранения ФИО.
    /// </summary>
    [ObservableProperty] private string fIO;
    [ObservableProperty] private string result;
    
    /// <summary>
    /// Метод получения ФИО.
    /// </summary>
    public async Task GetFio()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var content = await response.Content.ReadFromJsonAsync<Response>();
    
        FIO = content.Value;
    
        Validation();
    }
    
    public void Validation()
    {
        var containsDigit = FIO.Any(char.IsDigit);
    
        if (containsDigit)
        {
            Result = "ФИО содержит запрещённые символы";
        }
        else
        {
            Result = "ФИО валидно";
        }
    }
}