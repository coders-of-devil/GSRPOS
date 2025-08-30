using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServiceClass
{
    public class NotificationService
    {
        public event Action<string, string>? OnNotify;

        public void Notify(string message, string type = "info")
        {
            OnNotify?.Invoke(message, type);
        }

        public void NotifyError(Exception ex, string context = "")
        {
            var msg = !string.IsNullOrEmpty(context)
                ? $"❌ Error in {context}: {ex.Message}"
                : $"❌ {ex.Message}";

            Notify(msg, "error");
        }
    }
}
