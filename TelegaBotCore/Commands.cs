using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegaBotCore
{
    internal static class Commands
    {
        public const string start = @"/start";
        public const string confirmCredentials = @"/confirm_credentials";
        public const string checkPassword = @"DerParol";
        public const string update1Day = @"/update1day";
        public const string update1Week = @"/update1week";
        public const string updateTillYesterday = @"/update_till_yesterday";
    }
}
/*
confirm_credentials - Подтвердить полномочия
check_password - Проверить пароль
update1day - Обновить за вчера
update1week - Обновить за прошлую неделю
update_till_yesterday - Обновить с начала месяца по вчера
*/
