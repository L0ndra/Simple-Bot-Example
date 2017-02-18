using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using ukrpostbot.Models;

namespace ukrpostbot.Dialogs
{
    [Serializable]
    public class PoolDialog : IDialog<object>
    {
        private string _city;
        private string _depertment;
        private string _date;
        private string _typeOfProblem;
        private string _description;

        private IEnumerable<string> _choises = new[]
        {
            "Посилка йшла надто довго",
            "Незручний графік",
            "Неввічливий персонал",
            "Посилка ушкоджена",
            "Черги у відділенні",
            "Надто високі тарифи",
            "Незручне розташування",
            "Довге оформлення",
            "Вiдсутня упаковка",
            "Висока якість обслуговування"
        };
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            PromptDialog.Confirm(context, AfterRequestRewievAsync, "Ви хочете залити відгук");
        }

        public async Task AfterRequestRewievAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var condition = await argument;
            if (condition)
            {
                PromptDialog.Text(context, AfterRequestCityAsync, "Введіть місто");
            }
            else
            {
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterRequestCityAsync(IDialogContext context, IAwaitable<string> argument)
        {
            _city = await argument;
            PromptDialog.Text(context, AfterRequestDepartmentAsync, "Введіть відділ");
        }

        public async Task AfterRequestDepartmentAsync(IDialogContext context, IAwaitable<string> argument)
        {
            _depertment = await argument;
            PromptDialog.Choice(context, AfterRequestTypeOfProblemAsync, _choises, "");
        }

        public async Task AfterRequestTypeOfProblemAsync(IDialogContext context, IAwaitable<string> argument)
        {
            _typeOfProblem = await argument;
            PromptDialog.Text(context, AfterRequestDescriptionAsync, "Введіть додаткову інформацію");
        }

        public async Task AfterRequestDescriptionAsync(IDialogContext context, IAwaitable<string> argument)
        {
            _description = await argument;

            await ReviewDal.AddNewReviewToTable(new ReviewModel(_city, _depertment)
            {
                TypeOfProblem = _typeOfProblem,
                Description = _description
            });

            await context.PostAsync("Ваш відгук збережено");
            context.Wait(MessageReceivedAsync);
        }

    }
}