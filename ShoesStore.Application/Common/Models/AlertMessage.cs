using ShoesStore.Application.Common.Enums;

namespace ShoesStore.Application.Common.Models
{
    public class AlertMessage
    {
        public AlertType Type { get; set; }
        public string Message { get; set; } = string.Empty;

        public static AlertMessage Success(string message) => new() { Type = AlertType.Success, Message = message };
        public static AlertMessage Error(string message) => new() { Type = AlertType.Error, Message = message };
        public static AlertMessage Warning(string message) => new() { Type = AlertType.Warning, Message = message };
        public static AlertMessage Info(string message) => new() { Type = AlertType.Info, Message = message };
    }
}