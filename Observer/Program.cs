using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        CarroRoubado carroRoubado = new CarroRoubado();
        CarroPolicia carroPolicia = new CarroPolicia();

        carroRoubado.RegistrarObserver(carroPolicia); // Carro de polícia observa o carro roubado

        // Mudanças no estado do carro roubado que notificam o carro de polícia
        carroRoubado.Frente();
        carroRoubado.Direita();
        carroRoubado.Esquerda();
        carroRoubado.Para();
    }
}

public interface IObserver
{
    void Update(string estado);
}

public interface IObservable
{
    void RegistrarObserver(IObserver observer);
    void RemoverObserver(IObserver observer);
    void NotificarObservers();
}

public interface ICarro
{
    void Frente();
    void Direita();
    void Esquerda();
    void Para();
}

class CarroPolicia : ICarro, IObserver
{
    public void Frente()
    {
        Console.WriteLine("O carro da polícia está indo para frente.");
    }

    public void Direita()
    {
        Console.WriteLine("O carro da polícia está virando à direita.");
    }

    public void Esquerda()
    {
        Console.WriteLine("O carro da polícia está virando à esquerda.");
    }

    public void Para()
    {
        Console.WriteLine("O carro da polícia parou.");
    }

    public void Update(string estado)
    {
        Console.WriteLine("Carro da polícia recebeu atualização: " + estado);
    }
}

class CarroRoubado : ICarro, IObservable
{
    private List<IObserver> observers = new List<IObserver>();
    private string acao;

    public void RegistrarObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoverObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotificarObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.Update(acao);
        }
    }

    public void Frente()
    {
        acao = "Frente";
        Console.WriteLine("Carro Roubado vai à frente");
        NotificarObservers();
    }

    public void Direita()
    {
        acao = "Direita";
        Console.WriteLine("Carro Roubado vira à direita");
        NotificarObservers();
    }

    public void Esquerda()
    {
        acao = "Esquerda";
        Console.WriteLine("Carro Roubado vira à esquerda");
        NotificarObservers();
    }

    public void Para()
    {
        acao = "Para";
        Console.WriteLine("Carro Roubado parou");
        NotificarObservers();
    }
}
