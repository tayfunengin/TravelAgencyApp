using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Tools
{
    public class Notification
    {

        public static string Message(/*NotificationType notificationType*/ string notificationType, string message)
        {
            string notify = $"toastr.{notificationType/*.ToString()*/}('{message}')";           
            return notify;
        }
    }
}
