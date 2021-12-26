using System;
using Common;

namespace Client
{
    public class Parser
    {
        public Message Parse(string input)
        {
            var splitInput = input.Split(" ");
            if (splitInput.Length == 1 && splitInput[0] == "print")
            {
                return new Message
                {
                    CommandType = CommandType.Print
                };
            }

            if (splitInput.Length == 2)
            {
                if (splitInput[0] == "add" && Enum.TryParse(typeof(ElemType), splitInput[1], out var elemType))
                {
                    return new Message
                    {
                        CommandType = CommandType.AddElem,
                        ElemType = (ElemType)elemType
                    };
                }

                if (splitInput[0] == "show" && int.TryParse(splitInput[1], out var id))
                {
                    return new Message
                    {
                        CommandType = CommandType.Show,
                        Id = id
                    };
                }
                
                if (splitInput[0] == "login")
                {
                    return new Message
                    {
                        CommandType = CommandType.Login,
                        Name = splitInput[1]
                    };
                }

                var secondArgument = splitInput[1].Split("--");
                if (splitInput[0] == "connect" && secondArgument.Length == 2 &&
                    int.TryParse(secondArgument[0], out var inputId) &&
                    int.TryParse(secondArgument[1], out var outputId))
                {
                    return new Message
                    {
                        CommandType = CommandType.Connection,
                        InputId = inputId,
                        OutputId = outputId
                    };
                }
                
            }

            if (splitInput.Length == 3)
            {
                if (splitInput[0] == "add" && (splitInput[1] == "in" || splitInput[1] == "out"))
                {
                    return new Message
                    {
                        CommandType = CommandType.AddIO,
                        IsInput = splitInput[1] == "in",
                        Name = splitInput[2]
                    };
                }

                if (splitInput[0] == "set" && bool.TryParse(splitInput[2], out var value))
                {
                    return new Message
                    {
                        CommandType = CommandType.Set,
                        Name = splitInput[1],
                        Value = value
                    };
                }
            }

            return null;
        }
    }
}