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
            //Props validationActorProps = Props.Create( () => new ValidatorActor(consoleWriterActor));
            //IActorRef validationActor = MyActorSystem.ActorOf(validationActorProps, "validationActor");
            Props tailCoordinatorProps = Props.Create( () => new TailCoordinatorActor());
            IActorRef tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorProps");

            Props fileValidatorActorProps = Props.Create( () => new FileValidatorActor(consoleWriterActor, tailCoordinatorActor));
            IActorRef validationActor = MyActorSystem.ActorOf(fileValidatorActorProps,
                "validationActor");
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
