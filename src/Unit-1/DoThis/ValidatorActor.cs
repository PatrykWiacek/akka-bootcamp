using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace WinTail
{
    public class ValidatorActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidatorActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                _consoleWriterActor.Tell(new Messages.NullInputError("No input received."));
            }
            else
            {
                var valid = IsValid(msg);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Nessage was valid"));
                }
                else
                {
                    _consoleWriterActor.Tell(new Messages.ValidationError("Invalid: input had odd number of characters"));
                }
            }
            Sender.Tell(new Messages.ContinueProcessing());

        }

        private static bool IsValid(string msg)
        {
            var valid = msg.Length % 2 == 0;
            return valid;
        }
    }
}
