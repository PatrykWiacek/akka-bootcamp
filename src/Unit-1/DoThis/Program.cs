using System;
﻿using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            MyActorSystem = ActorSystem.Create("MyActorSystem");
            
            //props + IActorRef for ea Actor
            Props consoleWriterProps = Props.Create<ConsoleWriterActor>();
            IActorRef consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps,"consoleWriterActor");
           
            //props + IActorRef for ea Actor
            Props validationActorProps = Props.Create( () => new ValidatorActor(consoleWriterActor));
            IActorRef validationActor = MyActorSystem.ActorOf(validationActorProps, "validationActor");

            //props + IActorRef for ea Actor
            Props consoleReaderProps = Props.Create<ConsoleReaderActor>(validationActor);
            IActorRef consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps,"consoleReaderActor");
            

            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            consoleReaderActor.Tell(new Messages.ContinueProcessing());

            MyActorSystem.WhenTerminated.Wait();
        }
    }
    #endregion
}
