using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class SampleCommand1 : Command
    {
        public SampleCommand1(String txt)
        {
            this.pString = txt;
        }

        public override void Execute(float deltaTime)
        {
            Debug.WriteLine(" {0} time:{1} ", this.pString, TimerMan.GetCurrTime());
        }

        private String pString;
    }
}
