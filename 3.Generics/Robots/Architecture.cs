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
        private int _counter = 1;

        public ShooterCommand GetCommand()
        {
            return ShooterCommand.ForCounter(_counter++);
        }
    }

    public class BuilderAI : IRobotAI<BuilderCommand>
    {
        private int _counter = 1;

        public BuilderCommand GetCommand()
        {
            return BuilderCommand.ForCounter(_counter++);
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
        public string ExecuteCommand(IShooterMoveCommand command)
        {
            if (command == null)
                throw new ArgumentException();

            var hide = command.ShouldHide ? "YES" : "NO";
            return $"MOV {command.Destination.X}, {command.Destination.Y}, USE COVER {hide}";
        }
    }

    public class Robot<T>
    {
        private readonly IRobotAI<T> _ai;
        private readonly IDevice<T> _device;

        public Robot(IRobotAI<T> ai, IDevice<T> executor)
        {
            _ai = ai;
            _device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
            for (var i = 0; i < steps; i++)
            {
                var command = _ai.GetCommand();
                if (command == null)
                    break;

                yield return _device.ExecuteCommand(command);
            }
        }
    }

    public static class Robot
    {
        public static Robot<TCommand> Create<TCommand>(IRobotAI<TCommand> ai, IDevice<TCommand> executor)
        {
            return new Robot<TCommand>(ai, executor);
        }
    }
}