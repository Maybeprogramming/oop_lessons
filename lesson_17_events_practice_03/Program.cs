namespace lesson_17_events_practice_03
{
    class Program
    {
        static void Main()
        {
            var mailManager = new MailManager();
            mailManager.NewMail += MailManagerNewMail;

            Console.WriteLine("Введите ваше имя: ");
            var sender = Console.ReadLine();

            Console.WriteLine("Кому отправить? Введите имя: ");
            var target = Console.ReadLine();

            Console.WriteLine("Введите текст сообщения: ");
            var message = Console.ReadLine();

            mailManager.SimulatedNewMail(sender, target, message);

            Console.ReadLine();
        }

        private static void MailManagerNewMail(object? sender, NewMailEventsArgs e)
        {
            var sms = new SMS();
            sms.Send(e.Subject);

            var printer = new Printer();
            printer.Print($"Сообщение от {e.From} для {e.To}. \r\n{e.Subject}");
        }
    }

    class NewMailEventsArgs : EventArgs
    {
        public string From { get; }
        public string To { get; }
        public string Subject { get; }

        public NewMailEventsArgs(string from, string to, string subject)
        {
            From = from;
            To = to;
            Subject = subject;
        }

        public override string ToString()
        {
            return $"Письмо от {From} для {To}: {Subject}";
        }
    }

    class MailManager
    {
        public event EventHandler<NewMailEventsArgs>? NewMail;

        protected virtual void OnNewMail (NewMailEventsArgs e) 
        { 
            //_ = e ?? throw new ArgumentNullException(nameof(e));

            NewMail?.Invoke (this, e);
        }

        public void SimulatedNewMail (string from, string to, string subject)
        {
            var e = new NewMailEventsArgs (from, to, subject);

            OnNewMail (e);
        }
    }

    class Printer
    {
        public void Print (string message)
        {
            Console.WriteLine($"Печатаем сообщение на бумаге: \r\n{message}\r\n");
        }
    }

    class SMS
    {
        public void Send(string message)
        {
            Console.WriteLine($"Отправляем SMS сообщение: \r\n{message}\r\n");
        }
    }
}

//CLR via C# Рихтер (Глава 11)
//С канала CODE BLOG