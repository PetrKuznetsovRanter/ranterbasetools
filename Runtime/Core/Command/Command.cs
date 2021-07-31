using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICommand
{
    ICommand Do();
    ICommand Undo();
}


public abstract class Command : ICommand
{
    public ICommand Do()
    {
        return null;
    }

    public ICommand Undo()
    {
        return null;
    }
}

public class CommandBuilder
{
    private Command command;

    public CommandBuilder()
    {
        //command=new 
    }
    public CommandBuilder(Command command)
    {
        this.command = command;
    }
    
    public static implicit operator Command(CommandBuilder commandBuilder)
    {
        return commandBuilder.command;
    }
}
