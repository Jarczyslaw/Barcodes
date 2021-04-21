using System;
using System.Collections.Generic;

namespace JToolbox.Core.Results
{
    public class Messages : List<Message>
    {
        public void AddInformation(string information, int code = 0)
        {
            Add(new Message
            {
                Type = MessageType.Information,
                Content = information,
                Code = code
            });
        }

        public void AddWarning(string warning, int code = 0)
        {
            Add(new Message
            {
                Type = MessageType.Warning,
                Content = warning,
                Code = code
            });
        }

        public void AddError(string error, int code = 0)
        {
            Add(new Message
            {
                Type = MessageType.Error,
                Content = error,
                Code = code
            });
        }

        public void AddError(Exception exc, int code = 0)
        {
            Add(new Message
            {
                Type = MessageType.Error,
                Content = exc.Message,
                Code = code
            });
        }
    }
}