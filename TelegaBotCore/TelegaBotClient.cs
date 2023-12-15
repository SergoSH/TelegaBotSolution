using Microsoft.VisualBasic;
using System.Configuration;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace TelegaBotCore
{
    public static class TelegaBotClient
    {
        //private const string TOKEN = "597275428:AAHILLBY37b15mmoRc6PkI4d4dT9truF8kg";

        private static ITelegramBotClient instance;
        private static List<Credentials> credentialList = new List<Credentials>(); 
        public static ITelegramBotClient getInstance()
        {
            if (instance == null)
            {
                string token = ConfigurationManager.AppSettings["id"] + ":" + ConfigurationManager.AppSettings["apiKey"];
                if (token == ":") throw new Exception("Не задан ApiKey бота в app.config");
                instance = new TelegramBotClient(token);
            }
            return instance;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                
                string answer = "";
                var idUser = message.Chat.Id;
                if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    
                    switch (message.Text.ToLower())
                    {
                        case Commands.start:
                            answer = "Добро пожаловать на борт, добрый путник!";
                            break;
                        case Commands.confirmCredentials:
                            if(!credentialList.Exists(x=> x.Id == idUser))
                            {
                                answer = "Введите пароль доступа";
                                credentialList.Add(new Credentials(idUser, DateTime.Now));
                            }
                            else if(credentialList.Exists(x => x.Id == idUser && x.IsApproved == true))
                            {
                                answer = "Ваши полномочия не требуют подтверждения";
                            }
                            
                            break;
                        case Commands.update1Day:
                            if (checkCredentials(idUser))
                            {
                                answer = "Выгрузка данных за вчерашний день запущена.";
                                await botClient.SendTextMessageAsync(message.Chat, answer);
                                answer = await executeExport(System.DateTime.Today.AddDays(-1), System.DateTime.Today.AddDays(-1));
                            }
                            else
                            {
                                answer = "Введите пароль доступа через команду \"Подтвердить полномочия\"";
                            }
                            
                            break;
                        case Commands.update1Week:
                            if (checkCredentials(idUser))
                            {
                                answer = "Выгрузка данных за последние 7 дней запущена.";
                                string res = await executeExport(System.DateTime.Today.AddDays(-8), System.DateTime.Today.AddDays(-1));
                            }
                            else
                            {
                                answer = "Введите пароль доступа через команду \"Подтвердить полномочия\"";
                            }
                            
                            break;
                        case Commands.updateTillYesterday:
                            if (checkCredentials(idUser))
                            {
                                answer = "Выгрузка данных с начала месяца по вчерашний день запущена.";
                                DateTime curDate = System.DateTime.Today.AddDays(-1); //не совсем курДэйт, выгрузку делаем за вчера и ранее
                                                                                      
                                //начало месяца относительно вчерашнего дня
                                executeExport(new DateTime(curDate.Year, curDate.Month, 1), curDate);
                            }
                            else
                            {
                                answer = "Введите пароль доступа через команду \"Подтвердить полномочия\"";
                            }
                            
                            break;

                        default:
                            if(message.Text == Commands.checkPassword)
                            {
                                var credential = credentialList.Find(x=>x.Id == idUser);
                                if(credential == null)
                                {
                                    credential = new Credentials(idUser);
                                    credentialList.Add(credential);
                                }
                                credential.DateOfApproving = DateTime.Now;
                                credential.IsApproved = true;
                                answer = "Ваши полномочия подтверждены";

                            }
                            else
                            {
                                answer = "Даже и не знаю что сказать!";
                            }
                            break;
                    }
                }
                

                await botClient.SendTextMessageAsync(message.Chat, answer);
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public static void startReceivingMessages(ReceiverOptions receiverOptions, CancellationToken cancellationToken)
        {
            getInstance();
            instance.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            
        }

        private static bool checkCredentials(long id)
        {
            return true;
            return credentialList.Exists(x => x.Id == id && x.IsApproved == true);
        }

        private static async Task<string> executeExport(DateTime dateStart, DateTime dateEnd)
        {
            string cmd = "test.bat";
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                UseShellExecute = false,
                WorkingDirectory = @"C:\temp",
                FileName = @"C:\Windows\System32\cmd.exe",
                Arguments = "/c " + cmd,
                WindowStyle = ProcessWindowStyle.Normal,
                RedirectStandardOutput = true
            };
            try
            {
                Process proc = Process.Start(psi);
                StreamReader srResult = proc.StandardOutput;
                //Task.FromResult(await srResult.ReadToEndAsync());
                var res = await srResult.ReadToEndAsync();
                return res;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return ex.Message;
            }
            
            
            //todo put result into log
        }

    }
}