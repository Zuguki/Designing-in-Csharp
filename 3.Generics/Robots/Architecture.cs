using System;
using System.Collections.Generic;

namespace Generics.Robots
{
    public interface IRobotAI<out T>
    {
        T GetCommand();
    }

    public class ShooterAI : IRobotAI<ShooterCommand>
    {
        int counter = 1;

        public ShooterCommand GetCommand()
        {
            return ShooterCommand.ForCounter(counter++);
        }
    }

    public class BuilderAI : IRobotAI<BuilderCommand>
    {
        int counter = 1;

        public BuilderCommand GetCommand()
        {
            return BuilderCommand.ForCounter(counter++);
        }
    }

    public interface IDevice<in T>
    {
        string ExecuteCommand(T command);
    }

    public class Mover : IDevice<IMoveCommand>
    {
        public string ExecuteCommand(IMoveCommand command)
        {
            if (command == null)
                throw new ArgumentException();
            return $"MOV {command.Destination.X}, {command.Destination.Y}";
        }
    }

    public class ShooterMover : IDevice<IShooterMoveCommand>
    {
        public string ExecuteCommand(IShooterMoveCommand _command)
        {
            if (_command == null)
                throw new ArgumentException();
            
            var hide = _command.ShouldHide ? "YES" : "NO";
            return $"MOV {_command.Destination.X}, {_command.Destination.Y}, USE COVER {hide}";
        }
    }

    public class Robot
    {
        private readonly IRobotAI<object> ai;
        private readonly IDevice<object> device;
        private Type _type;

        private Robot(IRobotAI<object> ai, IDevice<object> executor)
        {
            this.ai = ai;
            device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
            for (var i = 0; i < steps; i++)
            {
                var command = ai.GetCommand();
                if (command == null)
                    break;
                
                yield return device.ExecuteCommand(command as IMoveCommand);
            }
        }

        public static Robot Create<TCommand>(IRobotAI<object> ai, IDevice<TCommand> executor)
        {
            return new Robot(ai, executor);
        }
    }
}
