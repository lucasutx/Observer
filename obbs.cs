using System;
using System.Collections.Generic;
using System.IO;

// Gerenciamento de inscrições e notificação
class EventManager
{
    private Dictionary<string, List<IEventListener>> listeners = new Dictionary<string, List<IEventListener>>();

    public void Subscribe(string eventType, IEventListener listener)
    {
        if (!listeners.ContainsKey(eventType))
        {
            listeners[eventType] = new List<IEventListener>();
        }
        listeners[eventType].Add(listener);
    }

    public void Unsubscribe(string eventType, IEventListener listener)
    {
        if (listeners.ContainsKey(eventType))
        {
            listeners[eventType].Remove(listener);
        }
    }

    public void Notify(string eventType, string data)
    {
        if (listeners.ContainsKey(eventType))
        {
            foreach (var listener in listeners[eventType])
            {
                listener.Update(data);
            }
        }
    }
}

// Editor concreto com lógica de negócio
class Editor
{
    public EventManager Events { get; private set; }
    private string file;

    public Editor()
    {
        Events = new EventManager();
    }

    public void OpenFile(string path)
    {
        file = path;
        Events.Notify("open", file);
    }

    public void SaveFile()
    {
        // Lógica de salvar arquivo
        File.WriteAllText(file, "Conteúdo salvo");
        Events.Notify("save", file);
    }
}

// Interface do assinante
interface IEventListener
{
    void Update(string filename);
}

// Assinante concreto para registrar logs
class LoggingListener : IEventListener
{
    private string logFile;
    private string message;

    public LoggingListener(string logFile, string message)
    {
        this.logFile = logFile;
        this.message = message;
    }

    public void Update(string filename)
    {
        var logMessage = message.Replace("%s", filename);
        File.AppendAllText(logFile, logMessage + Environment.NewLine);
    }
}

// Assinante concreto para alertas por email
class EmailAlertsListener : IEventListener
{
    private string email;
    private string message;

    public EmailAlertsListener(string email, string message)
    {
        this.email = email;
        this.message = message;
    }

    public void Update(string filename)
    {
        var emailMessage = message.Replace("%s", filename);
        Console.WriteLine($"Enviando email para {email}: {emailMessage}");
    }
}

// Configuração da aplicação
class Application
{
    public void Config()
    {
        var editor = new Editor();

        var logger = new LoggingListener("/path/to/log.txt", "Alguém abriu o arquivo: %s");
        editor.Events.Subscribe("open", logger);

        var emailAlerts = new EmailAlertsListener("admin@example.com", "Alguém mudou o arquivo: %s");
        editor.Events.Subscribe("save", emailAlerts);

        editor.OpenFile("documento.txt");
        editor.SaveFile();
    }
}

// Exemplo de execução
class Program
{
    static void Main()
    {
        var app = new Application();
        app.Config();
    }
}
