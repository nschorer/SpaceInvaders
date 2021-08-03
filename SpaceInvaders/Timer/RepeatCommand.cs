using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class RepeatCommand : Command
    {
        public RepeatCommand(String txt, float deltaRepeatTime)
        {
            this.pString = txt;
            this.repeatDelta = deltaRepeatTime;
        }

        public override void Execute(float deltaTime)
        {
            Debug.WriteLine(" {0} time:{1} ", this.pString, TimerMan.GetCurrTime());

            // Add itself back to timer
            TimerMan.Add(TimeEvent.Name.RepeatSample, this, this.repeatDelta);
        }

        private String pString;
        private float repeatDelta;
    }
}
