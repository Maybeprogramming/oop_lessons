namespace lesson_18_events_pattern_practice_04
{

    class Program
    {
        static void Main()
        {
            var mailManager = new MailManager();
            mailManager.NewMail += MailManagerNewMail;

            // Второй вариант реализации
            var sms = new SMS(mailManager);
            //конец второй реализации

            Console.WriteLine("Введите ваше имя: ");
            var sender = Console.ReadLine();

            Console.WriteLine("Кому отправить? Введите имя: ");
            var target = Console.ReadLine();

            Console.WriteLine("Введите текст сообщения: ");
            var message = Console.ReadLine();

            mailManager.SimulatedNewMail(sender, target, message);

            #region использование системы событий

            var typeWithLotsEvents = new TypeWithLotsEvents();
            typeWithLotsEvents.Foo += TypeWithLotsEvents_Foo;
            typeWithLotsEvents.SimulateFoo();

            #endregion

            Console.ReadLine();
        }

        #region использование системы событий
        private static void TypeWithLotsEvents_Foo(object? sender, FooEventsArgs e)
        {
            Console.WriteLine("\n" + new string('-', 50));
            Console.WriteLine("Произошло событие Foo");
        }
        #endregion

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

        protected virtual void OnNewMail(NewMailEventsArgs e)
        {
            //_ = e ?? throw new ArgumentNullException(nameof(e));

            NewMail?.Invoke(this, e);
        }

        public void SimulatedNewMail(string from, string to, string subject)
        {
            var e = new NewMailEventsArgs(from, to, subject);

            OnNewMail(e);
        }
    }

    class Printer
    {
        public Printer() { }
        public Printer(MailManager mailManager)
        {
            mailManager.NewMail += OnNewMailEvent;
        }

        private void OnNewMailEvent(object? sender, NewMailEventsArgs e)
        {
            Console.WriteLine("Получено новое сообщение. Выводим на печать:");
            Console.WriteLine($"Письмо от {e.From} для {e.To}");
            Console.WriteLine(e.Subject);
        }

        public void Unregister(MailManager mailManager)
        {
            mailManager.NewMail -= OnNewMailEvent;
        }

        public void Print(string message)
        {
            Console.WriteLine($"Печатаем сообщение на бумаге: \r\n{message}\r\n");
        }
    }

    class SMS
    {
        public SMS()
        {

        }

        //Второй варинат реализации
        public SMS(MailManager mailManager)
        {
            mailManager.NewMail += OnNewMail;
        }
        //Конец второй реализации

        private void OnNewMail(object? sender, NewMailEventsArgs e)
        {
            Console.WriteLine($"{sender}");
        }

        public void Send(string message)
        {
            Console.WriteLine($"Отправляем SMS сообщение: \r\n{message}\r\n");
        }
    }

    class EventKey
    {

    }

    class EventSet
    {
        private readonly Dictionary<EventKey, Delegate> events = new Dictionary<EventKey, Delegate>();

        public void Add (EventKey eventKey, Delegate handler)
        {
            Monitor.Enter(events);

            if (events.TryGetValue(eventKey, out var d))
            {
                events[eventKey] = Delegate.Combine(d, handler);
            }
            else
            {
                events.Add(eventKey, handler);
            }

            Monitor.Exit(events);
        }

        public void Remove (EventKey eventKey, Delegate handler)
        {
            Monitor.Enter(events);

            if (events.TryGetValue(eventKey, out var d))
            {
                d = Delegate.Remove(d, handler);
            }

            if (d!= null)
            {
                events[eventKey] = d;
            }
            else
            {
                events.Remove(eventKey);
            }

            Monitor.Exit(events);
        }

        public void Raise (EventKey eventKey, object sender, EventArgs e)
        {
            Monitor.Enter(events);

            if (events.TryGetValue(eventKey, out var d))
            {
                d.DynamicInvoke(new object[] { sender, e });
            }

            Monitor.Exit(events);
        }
    }

    class FooEventsArgs: EventArgs { }

    class TypeWithLotsEvents
    {
        protected EventSet EventSet { get; } = new EventSet();

        #region FooEvent
        protected static readonly EventKey fooEventKey = new EventKey();

        public event EventHandler<FooEventsArgs> Foo
        {
            add { EventSet.Add(fooEventKey, value); }
            remove { EventSet.Remove(fooEventKey, value); }
        }

        protected virtual void OnFoo(FooEventsArgs e)
        {
            EventSet.Raise(fooEventKey, this, e);
        }

        public void SimulateFoo()
        {
            OnFoo(new FooEventsArgs());
        }
        #endregion

    }
}