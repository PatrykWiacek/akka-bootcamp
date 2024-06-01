using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace WinTail
{

    public class TailCoordinatorActor : UntypedActor
    {
        #region Message types
        public class StartTail
        {
            public StartTail(string filePath, IActorRef reporterActor)
            {
                FilePath = filePath;
                ReporterActor = reporterActor;
            }

            public string FilePath { get; private set; }

            public IActorRef ReporterActor { get; private set; }
        }

        public class StopTail
        {
            public StopTail(string filePath)
            {
                FilePath = filePath;
            }

            public string FilePath { get; private set; }
        }
        #endregion

        protected override void OnReceive(object message)
        {
            if (message is StartTail)
            {
                var msg = message as StartTail;
                Context.ActorOf(Props.Create(
                    () => new TailActor(msg.ReporterActor, msg.FilePath)));
            }
        }
    }
}
